using UnityEngine;

namespace TkrainDesigns.Tiles.Grids
{
    public class Gridlike : MonoBehaviour
    {
        const float XFactor = .834f;
        const float YFactor = .956f;

    

        float GridSize => TileUtilities.TileSize;

        bool Hexagon
        {
            get { return true; }
        }

        float HexGridSizeX => GridSize * XFactor;

        float HexGridSizeY => GridSize * YFactor;

        public Vector2Int GridPosition => new Vector2Int(GridX, GridY);

        public Vector3 IdealWorldPosition
        {
            get
            {
                if (Hexagon)
                {
                    float offset = HexGridSizeY * Xeven * .5f;
                    return new Vector3(GridX * HexGridSizeX, 0, GridY * HexGridSizeY + offset);
                }

                return new Vector3(GridX * GridSize, 0, GridY * GridSize);
            }
        }

        // Use this for initialization
        public int GridX
        {
            get
            {
                if (Hexagon)
                {
                    return Mathf.RoundToInt(transform.position.x / HexGridSizeX);
                }

                return Mathf.RoundToInt(transform.position.x / GridSize);
            }
        }

        int Xeven => Mathf.Abs(GridX % 2);


        public int GridY
        {
            get
            {
                if (Hexagon)
                {
                    float offset = HexGridSizeY * Xeven * .45f;
                    float zed = transform.position.z - offset;
                    return Mathf.RoundToInt(zed / HexGridSizeY);
                }

                return Mathf.RoundToInt(transform.position.z / GridSize);
            }
        }

        public float DistanceFrom(Vector3 location)
        {
            return Mathf.Abs(Vector3.Distance(location, transform.position));
        }

        public float DistanceFrom(Gridlike location)
        {
            return Mathf.Abs(Vector3.Distance(location.transform.position, transform.position));
        }
    }
}