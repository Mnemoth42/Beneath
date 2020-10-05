using System.Collections;
using System.Collections.Generic;
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
    } 
}
