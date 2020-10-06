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


namespace TkrainDesigns.Tiles.Control
{


    public class PlayerController : BaseController
    {
        [SerializeField] bool debug = false;

        public  UnityEvent onItemPickedUp;

        
        public Dictionary<Vector2Int, bool> obstacleList=new Dictionary<Vector2Int, bool>();

        public System.Action<BaseController> onTargetChanged;

        PerformableActionItem currentActionItem = null;

        List<Vector2Int> path;

        bool turnChosen = false;
        
        bool moveSelected = false;
        bool actionSelected = false;
        System.Action finishLocated;
        Vector2Int finish;

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
            transform.position = position;
        }

        void Update()
        {
            
            if (IsCurrentTurn && !turnChosen)
            {
                SMovementRequest request = CheckMovementRequest();
                if (request.Path == null) return;
                {
                    ResetAllColorChangers();
                    if (request.Perform)
                    {
                        onTargetChanged(request.target);
                        if (currentActionItem != null)
                        {
                            Debug.Log($"{name} calling {currentActionItem.displayName}.BeginAction");
                            actionPerformer.BeginAction(currentActionItem, request.target.GetComponent<Health>(), request.Path, PathComplete);
                        }
                        else
                        {
                            Fighter.BeginAttackAction(request.target.GetComponent<Health>(), request.Path,
                                                      PathComplete);
                        }

                        
                    }
                    else
                    {
                        onTargetChanged(null);
                        Mover.BeginMoveAction(request.Path, PathComplete);
                    }
                    
                }
            }
        }

        public override void BeginTurn()
        {
            base.BeginTurn();
            GetComponent<ActionStore>().BeginTurn();
            turnChosen = false;
            FindMoveableTiles();
        }

        void PathComplete()
        {
            GetComponent<ActionStore>().EndTurn();
            Vector2Int currentPosition = TileUtilities.GridPosition(transform.position);
            GridPathFinder<Pickup>.ConductInventory();
            var pickups = GridPathFinder<Pickup>.GetItemsAt(currentPosition);
            foreach (Pickup pickup in pickups)
            {
                pickup.PickupItem();
                onItemPickedUp?.Invoke();
            }

            currentActionItem = null;
            if (currentPosition == finish)
            {
                finishLocated?.Invoke();
            }
            FinishTurn();
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
                        c.HighlightMaterial();
                    }
                    else
                    
                    {
                        if(q.Count<=moverdistance)
                             c.SelectedMaterial();
                    }
                }
                else
                {
                    //Debug.Log($"{tile} appears to have no path");
                }
            }
        }

        protected override void Die()
        {
            Anim.enabled = false;
            base.Die();
        }

        ColorChanger lastChanger = null;
        SMovementRequest CheckMovementRequest()
        {
            SMovementRequest request = new SMovementRequest();
            request.Path = null;
            request.Perform = false;
            if (lastChanger)
            {
                lastChanger.SetMouseOver(false);
                lastChanger = null;
            }
            if (!IsCurrentTurn)
            {
                return new SMovementRequest();
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return new SMovementRequest();
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Tile tile = hit.transform.GetComponent<Tile>();
                if (tile)
                {
                    ColorChanger changer = tile.GetComponentInChildren<ColorChanger>();
                    if (changer)
                    {
                        changer.SetMouseOver(true);
                        lastChanger = changer;
                    }
                }
            }
            else
            {
                return new SMovementRequest();
            }
            if (Input.GetMouseButtonDown(0))
            {

                var obstacles = GetObstacles();
                Vector2Int currentPosition = TileUtilities.GridPosition(transform.position);
                //Debug.Log("Click Detected");
                
                //if (Physics.Raycast(ray, out  hit))
                {
                    
                    Vector2Int clickedGridLocation = TileUtilities.GridPosition(hit.transform.position);
                    //Debug.Log($"Clicked on {clickedGridLocation}");
                    if (obstacles.ContainsKey(clickedGridLocation))
                    {
                        obstacles.Remove(clickedGridLocation);
                        request.Perform = true;
                        request.target = LocateControllerAt(clickedGridLocation);
                        Debug.Log($"{request.target} at {clickedGridLocation}");
                    }
                    GridPathFinder<Tile>.ConductInventory();
                    int availableMoves = Mover.MaxStepsPerTurn + 1;
                    if (currentActionItem)
                    {
                        Debug.Log($"{name} has {availableMoves-1}+{currentActionItem.Range(gameObject)} range.");
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
            }

            return new SMovementRequest();
        }

        

        BaseController LocateControllerAt(Vector2Int loc)
        {
            //var map = GridPathFinder<AIController>.GetLocationsWithT();
            //if (map.ContainsKey(loc)) return map[loc];
            foreach (AIController enemy in GridPathFinder<AIController>.GetItemsAt(loc))
            {
                if (enemy.IsAlive) return enemy;
            }
            Debug.Log($"{name} failed to find an enemy at {loc}");
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
                        changer.SelectedMaterial();
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
                    c.HighlightMaterial();
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
