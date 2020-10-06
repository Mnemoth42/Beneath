using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TkrainDesigns.Grids.Stats;
using TkrainDesigns.ScriptableEnums;
using TkrainDesigns.Stats;
using TkrainDesigns.Tiles.Actions;
using TkrainDesigns.Tiles.Combat;
using TkrainDesigns.Tiles.Grids;
using TkrainDesigns.Tiles.Movement;
using TkrainDesigns.Tiles.Pathfinding;
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
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(GridMover))]
    [RequireComponent(typeof(GridFighter))]
    [RequireComponent(typeof(ActionPerformer))]
    public abstract class BaseController : MonoBehaviour
    {
        protected System.Action OnTurnFinished = null;
        [Header("Used to calculate whose turn is next.")]
        [SerializeField] [Range(1,100)]protected float speed = 1.0f;

        [Header("Place Speed ScriptableStat here")] [SerializeField]
        ScriptableStat speedStat;

        [SerializeField] protected bool ragdoll = false;

        PersonalStats stats;

        private float nextTurn = 0;
        public Vector2Int currentVector2Int;

        public bool IsCurrentTurn { get; private set; } = false;

        public float NextTurn => nextTurn;

        protected Animator Anim { get; private set; }

        protected GridMover Mover { get; private set; }
        protected GridFighter Fighter { get; private set; }
        protected Health health { get; private set; }
        protected ActionPerformer actionPerformer { get; private set; }

        protected CooldownManager cooldownManager { get; private set; }


        public bool IsAlive => health.IsAlive;

        protected float Speed
        {
            get
            {
                if (stats && speedStat)
                {
                    return stats.GetStatValue(speedStat);
                }
                return speed;
            }
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
            if (cooldownManager)
            {
                cooldownManager.AdvanceTimers();
            }
            IsCurrentTurn = true;
        }

        protected virtual void FinishTurn()
        {
            if(this==null) ControllerCoordinator.BeginNextControllerTurn();
            //Debug.Log($"{name}/{GetInstanceID()} is ending turn.");
            IsCurrentTurn = false;
            CalculateNextTurn();
            currentVector2Int = TilePosition();
            ControllerCoordinator.BeginNextControllerTurn();
        }

        public void ResetTurn()
        {
            nextTurn = 0;
            CalculateNextTurn();
        }

        public virtual float CalculateNextTurn()
        {
            float timeToWaitBasedOnSpeed= (100.0f / speed);
            float randomInitiativeRoll = Random.Range(-timeToWaitBasedOnSpeed / 50.0f, timeToWaitBasedOnSpeed / 50.0f); //Initiative
            nextTurn += timeToWaitBasedOnSpeed + randomInitiativeRoll;
            return nextTurn;
        }

        public Vector2Int TilePosition()
        {
            return TileUtilities.CalcTileLocation(transform.position);
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