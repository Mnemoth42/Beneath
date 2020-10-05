using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TkrainDesigns.Grids.Stats;
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

            var path = GridPathFinder<Tile>.FindPath(ourPosition, playerPosition, others);
            
            int radius = PositionInList(path, playerPosition);
            //Debug.Log($"{name} is within {radius} moves of player.");
            if (radius >= 0)
            {
                if (radius > Mover.MaxStepsPerTurn)
                {
                    path.Remove(playerPosition);
                    Mover.BeginMoveAction(path, TurnCompleted);
                    return;
                }

                Fighter.BeginAttackAction(player.GetComponent<Health>(), path, TurnCompleted);
                return;
            }

            TurnCompleted();
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
