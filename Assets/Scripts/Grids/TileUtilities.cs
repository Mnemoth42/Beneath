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

        public static Vector2Int GridPosition(Vector3 position)
        {
            return new Vector2Int(GridX(position), GridY(position));
        }

        public static Vector3 IdealWorldPosition(Vector2Int location)
        {
            
            {
                
                {
                    float offset = HexGridSizeY * (location.x%2 * .5f);
                    return new Vector3(location.x * HexGridSizeX, 0, location.y * HexGridSizeY + offset);
                }

                
            }
        }

        

        ///// <summary>
        ///// Converts a worldspace float dimension to the nearest tile.
        ///// </summary>
        ///// <param name="dimension"></param>
        ///// <returns></returns>
        //public static int CalcFloatDimToTileDimension(float dimension)
        //{
        //    return Mathf.RoundToInt(dimension / TileSize);
        //}

        //public static float CalcTileDimToFloatDimension(int dimension)
        //{
        //    return (float) dimension * TileSize;
        //}

        //public static Vector2Int CalcTileLocation(Vector3 location)
        //{
        //    return new Vector2Int(CalcFloatDimToTileDimension(location.x), CalcFloatDimToTileDimension(location.z));
        //}

        //public static Vector3 CalcTilePhysicalLocation(Vector2Int location)
        //{
        //    return new Vector3(CalcTileDimToFloatDimension(location.x), 0, CalcTileDimToFloatDimension(location.y));
        //}

        //public static Vector3 CalculateCorrectTileLocation(Vector3 location)
        //{
        //    return CalcTilePhysicalLocation(CalcTileLocation(location));
        //}
    }
}