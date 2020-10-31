using System.Collections.Generic;
using TkrainDesigns.Grids.Stats;
using TkrainDesigns.Tiles.Actions;
using TkrainDesigns.Tiles.Grids;
using TkrainDesigns.Tiles.Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TkrainDesigns.Tiles.Control
{
    public class AIController : BaseController
    {
        [SerializeField] float pursuitDistance = 10;

        PlayerController player;

        protected override void Awake()
        {
            base.Awake();
            player = FindObjectOfType<PlayerController>();
        }

        public override void BeginTurn()
        {
            base.BeginTurn();
            if (player == null)
            {
                ConsiderRandomMove();
                return;
            }

            if (player.GetComponent<Health>().IsDead)
            {
                ConsiderRandomMove();
                return;
            }
            if (Vector3.Distance(player.transform.position, transform.position) < pursuitDistance)
            {
                ConsiderMove();
            }
            else
            {
                ConsiderRandomMove();
            }
        }

        void ConsiderMove()
        {

            Vector2Int playerPosition = TileUtilities.GridPosition(player.transform.position);
            Vector2Int ourPosition = TileUtilities.GridPosition(transform.position);
            
            Dictionary<Vector2Int, bool> others = GetObstacles();
            others.Remove(playerPosition);
            PerformableActionItem potentialActionItem = GetAvailableAction();
            int maxSteps = Mover.MaxStepsPerTurn+1 + (potentialActionItem ? potentialActionItem.Range(gameObject) : 0);
            var path = GridPathFinder<Tile>.FindPath(ourPosition, playerPosition, others);
            
            int radius = PositionInList(path, playerPosition);
            if (radius >= 0)
            {
                if (radius > maxSteps)
                {
                    path.Remove(playerPosition);
                    Mover.BeginMoveAction(path, TurnCompleted);
                    return;
                }

                if (potentialActionItem && TestPotentialAction(potentialActionItem, player.transform, path))
                {
                    actionPerformer.BeginAction(potentialActionItem, player.GetComponent<Health>(), path,TurnCompleted);
                    return;
                }
                onTargetChanged?.Invoke(player);
                Fighter.BeginAttackAction(player.GetComponent<Health>(), path, TurnCompleted);
                return;
            }

            TurnCompleted();
        }



        /// <summary>
        /// The AI will cycle through the available actions in the ActionStore and choose either the first available combat spell or
        /// the first available healing/buff type spell that it can use.  If no actions are available, no action is returned.
        /// Priority is always given to Attack spells if available.
        /// </summary>
        /// <returns></returns>
        public PerformableActionItem GetAvailableAction()
        {
            if (!actionStore)
            {
                return null;
            }
            PerformableActionItem result = null;
            for (int i = 0; i < 6; i++)
            {
                PerformableActionItem potentialAction = actionStore.GetAction(i) as PerformableActionItem;

                if (potentialAction == null)
                {
                    continue;
                }

                if (!potentialAction.CanUse(gameObject))
                {
                    continue;
                }

                if (potentialAction.AIRangedAttackSpell())
                {
                    return potentialAction;
                }

                if (potentialAction.AIHealingSpell() && result == null)
                {
                    result = potentialAction;
                }
            }
            return result;
        }

        


        void ConsiderRandomMove()
        {
            Vector2Int ourPosition = TileUtilities.GridPosition(transform.position);
            Vector2Int potentialPosition = new Vector2Int(ourPosition.x+Random.Range(-5,5),ourPosition.y+Random.Range(-2,2));
            var others = GridPathFinder<BaseController>.GetLocations();
            others.Remove(ourPosition);
            var path = GridPathFinder<Tile>.FindPath(ourPosition, potentialPosition, others);
            if (path.Count > 0)
            {
                Mover.BeginMoveAction(path, TurnCompleted);
                return;
            }
            TurnCompleted();
        }

        int PositionInList(List<Vector2Int> path, Vector2Int playerPosition)
        {
            
            for(int i=0;i<path.Count; i++)
            {
                if (path[i] == playerPosition) return i;
            }
            return -1;
        }

        void TurnCompleted()
        {
            
            FinishTurn();
        }
    } 
}
