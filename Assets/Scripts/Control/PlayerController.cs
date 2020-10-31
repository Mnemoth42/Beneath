using GameDevTV.Inventories;
using System.Collections.Generic;
using System.Linq;

using TkrainDesigns.Grids.Stats;
using TkrainDesigns.Tiles.Actions;
using TkrainDesigns.Tiles.Core;
using TkrainDesigns.Tiles.Grids;
using TkrainDesigns.Tiles.Pathfinding;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


namespace TkrainDesigns.Tiles.Control
{


    public class PlayerController : BaseController
    {
        [SerializeField] bool debug = false;

        [SerializeField] float pickupRadius;

        public  UnityEvent onItemPickedUp;
        public UnityEvent<Vector3, string> onItemPickedTextSpawn;
        public UnityEvent onBeginTurnEvent;
        public UnityEvent onEndTurnEvent;
        
        public Dictionary<Vector2Int, bool> obstacleList=new Dictionary<Vector2Int, bool>();
        
        

        PerformableActionItem currentActionItem = null;

        List<Vector2Int> path;

        bool turnChosen = false;
        float clickDetection = 0.0f;
        Vector2Int tileLastClicked;
        bool hasHit = false;
        BaseController rayCastController = null;
        RaycastHit currentHit;
        

        System.Action finishLocated;
        Vector2Int finish;

        protected override void Awake()
        {
            base.Awake();
            Mover.onMoveStepCompleted += TestVisibility;
        }

        public void SetFinish(Vector2Int point, System.Action resetAction)
        {
            finish = point;
            finishLocated = resetAction;
        }

        public bool SetCurrentAction(PerformableActionItem action)
        {
            if (currentActionItem == action)
            {
                currentActionItem = null;
                FindMoveableTiles();
                return false;
            }

            currentActionItem = action;
            FindMoveableTiles();
            return true;
        }

        public void InitializePlayer(Vector3 position)
        {
            health.SetHealthToMaxHealth();
            transform.position = position;
        }

        void Update()
        {
            clickDetection -= Time.deltaTime;
            PrepRects();
            ProcessPointer();
            if (InteractWithUI()) return;
            if (!CheckRayCastHit()) return;
            SetMouseover();
            if (ProcessSingleClick()) return;
            if (IsCurrentTurn && !turnChosen)
            {
                SMovementRequest request = CheckMovementRequest();
                if (request.Path == null) return;
                PerformRequest(request);
            }
        }

        void PerformRequest(SMovementRequest request)
        {
            ResetAllColorChangers();
            if (request.Perform)
            {
                onTargetChanged(request.target);
                if (currentActionItem != null)
                {
                    actionPerformer.BeginAction(currentActionItem, request.target.GetComponent<Health>(),
                                                request.Path, PathComplete);
                }
                else
                {
                    Fighter.BeginAttackAction(request.target.GetComponent<Health>(), request.Path,
                                              PathComplete);
                }
            }
            else
            {
                onTargetChanged?.Invoke(null);
                Mover.BeginMoveAction(request.Path, PathComplete);
            }
        }

        public override void BeginTurn()
        {
            base.BeginTurn();
            GetComponent<ActionStore>().BeginTurn();
            onBeginTurnEvent?.Invoke();
            turnChosen = false;
            TestVisibility();
            FindMoveableTiles();
        }

        public override void RestoreCurrentPosition()
        {
            Anim.rootPosition = GetCurrentPosition();
        }

        void PathComplete()
        {
            onEndTurnEvent?.Invoke();
            Vector2Int currentPosition = TileUtilities.GridPosition(transform.position);
            GridPathFinder<Pickup>.ConductInventory();
            //var pickups = GridPathFinder<Pickup>.GetItemsAt(currentPosition);
            var pickups = FindObjectsOfType<Pickup>()
                .Where(p => Vector3.Distance(p.transform.position, transform.position) < pickupRadius);
            foreach (Pickup pickup in pickups)
            {
                
                onItemPickedUp?.Invoke();
                onItemPickedTextSpawn?.Invoke(pickup.transform.position, pickup.GetItem().GetDisplayName());
                pickup.PickupItem();
            }

            TestVisibility();
            currentActionItem = null;
            if (currentPosition == finish)
            {
                finishLocated?.Invoke();
            }
            FinishTurn();
        }

        void TestVisibility()
        {
            foreach (Visibility visibility in FindObjectsOfType<Visibility>())
            {
                visibility.TestVisibility(TileUtilities.GridPosition(transform.position));
            }
        }

        void FindMoveableTiles()
        {
            ResetAllColorChangers();
            float moverdistance;
            float distanceToCheck = moverdistance = Mover.MaxStepsPerTurn + 1;
            if (currentActionItem != null)
            {
                distanceToCheck += currentActionItem.Range(gameObject);
            }
            var tiles = GridPathFinder<Tile>.GetLocationsList()
                                            .Where(t => Vector2Int.Distance(t, TileUtilities.GridPosition(transform.position)) <= distanceToCheck)
                                            .ToList();
            var enemies = EnemiesStillAlive();
            var obstacles = GetObstacles();
            obstacles.Remove(TileUtilities.GridPosition(transform.position));
            
            foreach (Vector2Int tile in tiles)
            {

                var q = GridPathFinder<Tile>.FindPath(tile, TileUtilities.GridPosition(transform.position),obstacles);
                if (q.Count > 1 && q.Count <= distanceToCheck)
                {
                    ColorChanger c = GridPathFinder<Tile>.Map[tile].GetComponentInChildren<ColorChanger>();
                    if (enemies.ContainsKey(tile))
                    {
                        //if(!CheckForObstacles(q, tile, distanceToCheck-moverdistance))
                            c.SetMaterialOccupied();
                    }
                    else
                    
                    {
                        if(q.Count<=moverdistance)
                             c.SetMaterialWalkable();
                    }
                }
                else
                {
                }
            }
        }

        

        protected override void Die()
        {
            Anim.enabled = false;
            Invoke(nameof(ReloadGame), 2.0f);
            base.Die();
        }

        public override void ResetTurn()
        {
            NextTurn = 0;
            health.SetHealthToMaxHealth();
            cooldownManager.ResetCooldowns();
        }

        void ReloadGame()
        {
            SceneManager.LoadScene(0);
        }

        ColorChanger lastChanger = null;
        
        SMovementRequest CheckMovementRequest()
        {
            SMovementRequest request = new SMovementRequest();
            request.Path = null;
            request.Perform = false;

            if (!IsCurrentTurn)
            {
                return request;
            }

            if (InteractWithUI())
            {
                return request;
            }


            if (!hasHit)
            {
                return new SMovementRequest();
            }


//#if UNITY_EDITOR
//            if(Input.GetMouseButtonDown(0) && clickDetection>0.0f)
//#elif UNITY_ANDROID
//            if(Input.touchCount>0 && Input.touches[0].tapCount>1)
//#else       
            if(Input.GetMouseButtonDown(0) && clickDetection>0.0f)
//#endif
            {

                Dictionary<Vector2Int, bool> obstacles = GetObstacles();
                Vector2Int currentPosition = TileUtilities.GridPosition(transform.position);
                Vector2Int clickedGridLocation = TileUtilities.GridPosition(currentHit.transform.position);
                if (rayCastController)
                    clickedGridLocation = TileUtilities.GridPosition(rayCastController.transform.position);
                if (obstacles.ContainsKey(clickedGridLocation))
                {
                    obstacles.Remove(clickedGridLocation);
                    request.Perform = true;
                    request.target = rayCastController == null
                                         ? LocateControllerAt(clickedGridLocation)
                                         : rayCastController;
                }

                GridPathFinder<Tile>.ConductInventory();
                int availableMoves = Mover.MaxStepsPerTurn + 1;
                if (currentActionItem)
                {
                    availableMoves += currentActionItem.Range(gameObject);
                }

                var potentialPath = GridPathFinder<Tile>.FindPath(currentPosition, clickedGridLocation, obstacles);
                if (potentialPath.Count > 1 && potentialPath.Count <= availableMoves)
                {
                    if (debug)
                    {
                        ResetAllColorChangers();
                        HighlightColorChangers(potentialPath);
                    }

                    path = potentialPath;
                    request.Path = potentialPath;
                    if (lastChanger != null)
                    {
                        lastChanger.SetMouseOver(false);
                    }

                    turnChosen = true;
                    return request;
                }
            }

            return new SMovementRequest();
        }

        void SetMouseover()
        {
            if (lastChanger)
            {
                lastChanger.SetMouseOver(false);
                lastChanger = null;
            }
            //Tile tile = currentHit.transform.GetComponent<Tile>();
            if (currentHit.transform.TryGetComponent(out Tile tile))
            {
                ColorChanger changer = tile.GetComponentInChildren<ColorChanger>();
                if (changer)
                {
                    changer.SetMouseOver(true);
                    lastChanger = changer;
                }
            }
            else if (currentHit.transform.TryGetComponent(out AIController ai))
            {
                tile = GridPathFinder<Tile>.GetItemAt(TileUtilities.CalcTileLocation(ai.transform.position));
                if (tile && tile.TryGetComponent(out ColorChanger changer))
                {
                    changer.SetMouseOver(true);
                    lastChanger = changer;
                }

                onTargetChanged(ai);
            }

        }

        //Kludge workaround because Unity can't make up it's mind about IsPointerOverGameObject.
        public void CancelClicks()
        {
            clickDetection = 0.0f;
        }

        static bool InteractWithUI()
        {
#if UNITY_EDITOR
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }
#elif UNITY_ANDROID
            if(Input.touchCount>0)
            {
                for(int i=0; i<Input.touchCount;i++)
                {
                    if (EventSystem.current.IsPointerOverGameObject(Input.touches[i].fingerId)) return true;
                }
            }  
#else
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }
#endif
            

            return false;
        }

        Rect upperLeft;
        Rect upperRight;
        Rect lowerLeft;
        Rect lowerRight;
        List<Rect> rects;  
        void PrepRects()
        {
            rects=new List<Rect>();
            float centerX = (float)Screen.width / 2.0f;
            float centerY = (float) Screen.height / 2.0f;
            rects.Add(new Rect(0,0,centerX, centerY));
            rects.Add(new Rect(centerX, 0, Screen.width, centerY));
            rects.Add(new Rect(centerX, centerY, Screen.width, Screen.height));
            rects.Add(new Rect(0,centerY, centerX, Screen.height));
            
        }

        int FindRect(Vector3 position)
        {
            for (int i = 0; i < 4; i++)
            {
                if (rects[i].Contains(position))
                {
                    return i;
                }
            }

            return -1;
        }

        Vector3 dragPoint;
        int dragRect;
        bool dragging = false;
        void ProcessPointer()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (InteractWithUI())
                {
                    dragging = false;
                    return;
                }
                dragPoint = Input.mousePosition;
                dragRect = FindRect(dragPoint);
                dragging = true;
                return;
            }

            if (Input.GetMouseButton(0))
            {
                if (!dragging) return;
                int newRect = FindRect(Input.mousePosition);
                if ( newRect == dragRect) return;
                int delta = newRect - dragRect;
                if (Mathf.Abs(delta) > 2) delta *= -1;
                FindObjectOfType<CameraTurner>().TurnCamera(delta<0 ? true: false);
                dragRect = newRect;
            }

            
            
        }

        bool ProcessSingleClick()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonUp(0) && clickDetection <= 0.0f)
#elif UNITY_ANDROID
            if(Input.touchCount>0 && Input.touches[0].tapCount==1)
#else
            if(Input.GetMouseButtonUp(0) && clickDetection <=0.0f)
#endif
            {
                clickDetection = .5f;
                tileLastClicked = TileUtilities.GridPosition(currentHit.point);
                onTargetChanged(rayCastController == this ? null : rayCastController);
                return true;
            }
            return false;
        }

        bool CheckRayCastHit()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask mask = new LayerMask {value =  5 << 9};
            hasHit = Physics.Raycast(ray, out RaycastHit hit, 1000,mask);
            currentHit = hit;
            if (hasHit)
            {
                BaseController controller = hit.transform.GetComponentInParent<BaseController>();
                if (controller != null && controller.IsAlive)
                {
                    //Debug.Log($"Found Controller {controller.name}");
                    rayCastController = controller;
                }
                else rayCastController = null;
            }
            else
            {
                rayCastController = null;
            }
            return hasHit;
        }

        BaseController LocateControllerAt(Vector2Int loc)
        {
            //var map = GridPathFinder<AIController>.GetLocationsWithT();
            //if (map.ContainsKey(loc)) return map[loc];
            foreach (AIController enemy in GridPathFinder<AIController>.GetItemsAt(loc))
            {
                if (enemy.IsAlive) return enemy;
            }
            //Debug.Log($"{name} failed to find an enemy at {loc}");
            return null;
        }

        void HighlightColorChangers(List<Vector2Int> potentialPath)
        {
            foreach (Vector2Int location in potentialPath)
            {
                Tile tile = GridPathFinder<Tile>.Map[location];
                if (tile)
                {
                    ColorChanger changer = tile.GetComponentInChildren<ColorChanger>();
                    if (changer)
                    {
                        changer.SetMaterialWalkable();
                    }

                    Visibility visibility = tile.GetComponent<Visibility>();
                    if (visibility)
                    {
                        visibility.TestVisibility(TileUtilities.GridPosition(transform.position));
                    }
                }
            }

            Vector2Int finish = potentialPath[potentialPath.Count - 1];
            Tile finishTile = GridPathFinder<Tile>.Map[finish];
            if (finishTile)
            {
                ColorChanger c = finishTile.GetComponentInChildren<ColorChanger>();
                if (c)
                {
                    c.SetMaterialOccupied();
                }
            }
        }

        void ResetAllColorChangers()
        {
            GridPathFinder<Tile>.ConductInventory();
            Dictionary<Vector2Int, Tile> tiles = GridPathFinder<Tile>.Map;
            foreach (KeyValuePair<Vector2Int, Tile> pair in tiles)
            {
                ColorChanger changer = pair.Value.GetComponentInChildren<ColorChanger>();
                if (changer)
                {
                    changer.ResetMaterial();
                }
            }
        }
    } 
}
