using System.Collections.Generic;
using GameDevTV.Inventories;
using JetBrains.Annotations;
using RPG.Inventory;
using TkrainDesigns.Grids.Stats;
using TkrainDesigns.ScriptableEnums;
using TkrainDesigns.Stats;
using TkrainDesigns.Tiles.Actions;
using TkrainDesigns.Tiles.Combat;
using Tkraindesigns.Tiles.Core;
using TkrainDesigns.Tiles.Grids;
using TkrainDesigns.Tiles.Movement;
using TkrainDesigns.Tiles.Pathfinding;
using TkrainDesigns.Tiles.Skills;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TkrainDesigns.Tiles.Control
{
    public struct SMovementRequest
    {
        public bool Perform;  //renamed from PerformAttack to Perform as it really means do the current action
        public List<Vector2Int> Path;
        public BaseController target;
    }
    [RequireComponent(typeof(PersonalStats))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(GridMover))]
    [RequireComponent(typeof(GridFighter))]
    [RequireComponent(typeof(ActionPerformer))]
    [RequireComponent(typeof(RandomItemDropper))]
    [RequireComponent(typeof(CooldownManager))]
    [RequireComponent(typeof(CombatTarget))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(BreakingHitSender))]
    [RequireComponent(typeof(ActionSpellStore))]
    [RequireComponent(typeof(SkillStore))]
    [RequireComponent(typeof(EnemyVisibility))]
    [RequireComponent(typeof(BreakingHitSender))]
    
    public abstract class BaseController : MonoBehaviour
    {
        protected System.Action OnTurnFinished = null;
        [Header("Used to calculate whose turn is next.")]
        [SerializeField] [Range(1,100)]protected float speed = 1.0f;
        [Header("Place Speed ScriptableStat here")] [SerializeField]
        ScriptableStat speedStat;
        [SerializeField] protected bool ragdoll = false;

        
        public System.Action<BaseController> onTargetChanged;
        public event System.Action onTurnEnded ;
        
        public Vector2Int currentVector2Int;
        Vector3 currentPosition;
        PersonalStats stats;
        protected Animator Anim { get; private set; }
        protected GridMover Mover { get; private set; }
        protected GridFighter Fighter { get; private set; }
        protected Health health { get; private set; }
        protected ActionPerformer actionPerformer { get; private set; }
        protected CooldownManager cooldownManager { get; private set; }
        protected ActionStore actionStore { get; private set; }

        private float nextTurn = 0;
        public bool IsCurrentTurn { get; private set; } = false;

        public float NextTurn
        {
            get { return nextTurn; }
            protected set { nextTurn = value; }
        }





        public bool IsAlive => health.IsAlive;

        protected float Speed
        {
            get { return stats != null && speedStat != null ? stats.GetStatValue(speedStat) : speed; }
        }

        protected virtual void Awake()
        {
            Mover = GetComponent<GridMover>();
            Anim = GetComponent<Animator>();
            Fighter = GetComponent<GridFighter>();
            health = GetComponent<Health>();
            stats = GetComponent<PersonalStats>();
            actionPerformer = GetComponent<ActionPerformer>();
            cooldownManager = GetComponent<CooldownManager>();
            actionStore = GetComponent<ActionStore>();
            currentPosition = transform.position;
        }

        void OnEnable()
        {
            health.onDeath.AddListener(OnDeath);
        }

        protected virtual void OnDeath()
        {
            Invoke(nameof(Die), .25f);
        }

        protected virtual void Die()
        {
            if (ragdoll)
            {
                Anim.enabled = false;
                RagdollMinder minder = GetComponent<RagdollMinder>();
                if (minder)
                {
                    minder.RagDollEnabled();
                }
            }
            else
            {
                Anim.SetTrigger("Dead");
            }
            //Destroy(this);
        }

        protected virtual void Start()
        {
            CalculateNextTurn();
            currentVector2Int = TilePosition();
        }

        public virtual void BeginTurn()
        {
            //Debug.Log($"{name} BeginTurn()");
            if (cooldownManager)
            {
                cooldownManager.AdvanceTimers();
            }
            IsCurrentTurn = true;
        }

        protected static bool CheckForObstacles(List<Vector2Int> path, Vector2Int tile, float range)
        {
            if (path.Count <(int) range+1) return false; //clearly there are no obstacles if we are standing next to the tile.
            Vector3 finish = TileUtilities.IdealWorldPosition(tile);
            for (int i = path.Count - (int)range; i > 0; i--)
            {
                Vector3 start = TileUtilities.IdealWorldPosition(path[i]);
                Vector3 direction = finish - start;
                Ray ray = new Ray(start,direction);
                int layermask = 1 << 10;
                if (!Physics.Raycast(ray, out RaycastHit hit, Vector3.Distance(start, finish), layermask))
                {
                    return false;
                }
            }

            return true;
        }

        
        public Vector3 GetCurrentPosition() => currentPosition;

        public virtual void RestoreCurrentPosition()
        {
            Anim.enabled = false;
            transform.position = currentPosition;
            Anim.enabled = true;
            Debug.Log($"Restoring current position to {currentPosition} - {transform.position}");
        }

        protected virtual void FinishTurn()
        {
            //Debug.Log($"{name} FinishTurn()");
            if(this==null) ControllerCoordinator.BeginNextControllerTurn();
            IsCurrentTurn = false;
            CalculateNextTurn();
            currentVector2Int = TilePosition();
            currentPosition = transform.position;
            onTurnEnded?.Invoke();
            ControllerCoordinator.BeginNextControllerTurn();
        }

        public virtual void ResetTurn()
        {
            nextTurn = 0;
            CalculateNextTurn();
        }

        public virtual float CalculateNextTurn()
        {
            float timeToWaitBasedOnSpeed= (100.0f / Speed);
            float randomInitiativeRoll = Random.Range(-timeToWaitBasedOnSpeed / 25.0f, timeToWaitBasedOnSpeed / 25.0f); //Initiative
            nextTurn += timeToWaitBasedOnSpeed + randomInitiativeRoll;
            return nextTurn;
        }

        public Vector2Int TilePosition()
        {
            return TileUtilities.GridPosition(transform.position);
        }

        public static BaseController FindControllerAt(Vector2Int location)
        {
            
            foreach (BaseController controller in FindObjectsOfType<BaseController>())
            {
                if (controller.TilePosition() == location)
                    return controller;
            }

            return null;
        }

        protected bool TestPotentialAction(PerformableActionItem item, Transform testTarget, List<Vector2Int> path)
        {
            if (item.AIHealingSpell()) return true;
            int firstPlaceToFire = Mathf.Max(path.Count - item.Range(gameObject),0);
            int lastPlaceToFire = Mathf.Min(Mover.MaxStepsPerTurn, path.Count - 1);
            if (lastPlaceToFire - firstPlaceToFire <= 1) 
            {
                return true;
            }

            Vector2Int targetLocation = TileUtilities.GridPosition(testTarget.position);
            for (int i = firstPlaceToFire; i < lastPlaceToFire; i++)
            {
                if (Vector2Int.Distance(path[i], targetLocation) >= (float) (path.Count - i))
                {
                    return true;
                }
            }

            return false;
        }

        protected Dictionary<Vector2Int, BaseController> EnemiesStillAlive()
        {
            Dictionary<Vector2Int, BaseController> result = new Dictionary<Vector2Int, BaseController>();
            foreach (var tile in GridPathFinder<BaseController>.GetLocations())
            {
                foreach (var item in GridPathFinder<BaseController>.GetItemsAt(tile.Key))
                {
                    if (item.IsAlive)
                    {
                        result[tile.Key] = item;
                    } 
                }
            }

            return result;
        }

        [NotNull]
        protected Dictionary<Vector2Int, bool> GetObstacles()
        {
            Dictionary<Vector2Int, bool> result = new Dictionary<Vector2Int, bool>();
            foreach (var pair in EnemiesStillAlive())
            {
                if(pair.Value!=this)
                    result.Add(pair.Key, true);
            }

            return result;
        }

    }
}