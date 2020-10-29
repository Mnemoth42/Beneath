using System.Collections;
using System.Collections.Generic;
using TkrainDesigns.Tiles.Grids;
using UnityEngine;

namespace TkrainDesigns.Tiles.Pathfinding
{
    public class PathBase
    {
        public static readonly Vector2Int[] DirectionsHexEven =
        {
            Vector2Int.left,
            Vector2Int.right,
            Vector2Int.up,
            Vector2Int.down,
            new Vector2Int(-1,+1),
            new Vector2Int(+1,+1)
        };

        public static readonly Vector2Int[] DirectionsHexOdd =
        {
            Vector2Int.left,
            Vector2Int.right,
            Vector2Int.up,
            Vector2Int.down,
            new Vector2Int(-1,-1),
            new Vector2Int(+1,-1)
        };
        const float XFactor = .417f;
        const float YFactor = .478f;
        public static Vector3 AdjacentOdd(int corner)
        {
            return TileUtilities.IdealWorldPosition(DirectionsHexOdd[corner]);
        }

        public static Vector3 AdjacentEven(int corner)
        {
            return TileUtilities.IdealWorldPosition(DirectionsHexEven[corner]);
        }
    } 
}
