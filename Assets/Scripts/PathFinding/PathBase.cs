using TkrainDesigns.Tiles.Grids;
using UnityEngine;

namespace TkrainDesigns.Tiles.Pathfinding
{
    public  static class PathBase
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

        public static Vector3 AdjacentOdd(int corner)
        {
            return DirectionsHexOdd[corner].ToWorldPosition();
        }

        public static Vector3 AdjacentEven(int corner)
        {
            return DirectionsHexEven[corner].ToWorldPosition();
        }

        public static Vector2Int Adjacent(int x, int y, int corner)
        {
            if (x % 2 == 0) return new Vector2Int(x + DirectionsHexEven[corner].x, y + DirectionsHexEven[corner].y);
            return new Vector2Int(x + DirectionsHexOdd[corner].x, y + DirectionsHexOdd[corner].y);
        }

        public static Vector2Int Adjacent(Vector2Int location, int corner)
        {
            return Adjacent(location.x, location.y, corner);
        }
    }

    public static class PathBaseExtensions
    {
        public static Vector2Int Adjacent(this Vector2Int location, int face)
        {
            return PathBase.Adjacent(location, face);
        }
    }

}
