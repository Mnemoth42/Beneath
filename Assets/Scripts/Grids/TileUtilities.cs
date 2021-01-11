using UnityEngine;

namespace TkrainDesigns.Tiles.Grids
{
    public static class TileUtilities
    {
        public static float TileSize = 2.0f;
        const float XFactor = .834f;
        const float YFactor = .956f;

        static float HexGridSizeX => TileSize * XFactor;

        static float HexGridSizeY => TileSize * YFactor;

        static int GridX(Vector3 position)
        {
            return Mathf.RoundToInt(position.x / HexGridSizeX);
        }

        
        static int Xeven(Vector3 position)
        {
            return Mathf.Abs(GridX(position) % 2);
        }

        static int GridY(Vector3 position)
        {
            float offset = HexGridSizeY * Xeven(position) * .45f;
            float zed = position.z - offset;
            return Mathf.RoundToInt(zed / HexGridSizeY);
        }

        static Vector2Int GridPosition(Vector3 position)
        {
            return new Vector2Int(GridX(position), GridY(position));
        }
        /// <summary>
        /// The Position in Hex Tile space for Vector3(x,y,z)
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector2Int ToGridPosition(this Vector3 position)
        {
            return GridPosition(position);
        }

        static Vector3 IdealWorldPosition(Vector2Int location)
        {
            
            {
                
                {
                    float offset = HexGridSizeY * (location.x%2 * .5f);
                    return new Vector3(location.x * HexGridSizeX, 0, location.y * HexGridSizeY + offset);
                }

                
            }
        }
        /// <summary>
        /// The Position in 3D space for a tile located at Vector2(x,y)
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static Vector3 ToWorldPosition(this Vector2Int location)
        {
            return IdealWorldPosition(location);
        }
    }
}