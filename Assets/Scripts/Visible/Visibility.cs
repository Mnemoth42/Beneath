using TkrainDesigns.Tiles.Grids;
using TkrainDesigns.Tiles.Pathfinding;
using UnityEngine;

namespace TkrainDesigns.Tiles.Visible
{
    public class Visibility : MonoBehaviour
    {
        public float distanceToBeSeen = 10;
        public bool setIntensity = false;
        public float minIntensity = .1f;
        public float maxIntensity = .75f;
        bool hasBeenSeen = false;
        Vector2Int location;

        void Start()
        {
            location = transform.position.ToGridPosition();
            foreach (MeshRenderer rend in GetComponentsInChildren<MeshRenderer>())
            {
                rend.enabled = false;
            }
        }

        float Distance(Vector2Int tile)
        {
            return Vector2Int.Distance(tile, location);
        }

        public void TestVisibility(Vector2Int tile)
        {
            
                if (!hasBeenSeen && Vector2Int.Distance(tile, transform.position.ToGridPosition()) < distanceToBeSeen)
                {
                    Vector3 testPosition = tile.ToWorldPosition();
                    Vector3 position = transform.position;
                    Transform transformToCheck = transform;
                    if (RayTrace( testPosition, position, transformToCheck))
                    {
                        return;
                    }

                    hasBeenSeen = true;
                    foreach (MeshRenderer rend in GetComponentsInChildren<MeshRenderer>())
                    {
                        rend.enabled = true;
                    }
                }

                if (hasBeenSeen && setIntensity)
                {
                    float intensity = Distance(tile) / distanceToBeSeen;
                    intensity = 1 - intensity;
                    intensity *= maxIntensity;
                    intensity = Mathf.Clamp(intensity, minIntensity, maxIntensity);
                    foreach (MeshRenderer rend in GetComponentsInChildren<MeshRenderer>())
                    {
                        rend.material.SetFloat("Glow", intensity);
                    }
                }
                
            
        }

        public static bool ExtendedRayTrace(Vector2Int tile, Vector3 testPosition, Vector3 position,
                                                  Transform transformToCheck)
        {
            bool even = tile.x % 2 == 0;
            bool visible = false;
            if (RayTrace(testPosition, position, transformToCheck))
            {
                for (int i = 0; i < 6; i++)
                {
                    Tile tileToCheck = GridPathFinder<Tile>.GetItemAt(tile + (even
                                                                                  ? PathBase.DirectionsHexEven[i]
                                                                                  : PathBase.DirectionsHexOdd[i]));
                    if (tileToCheck == null) continue;
                    if (!RayTrace(testPosition,
                                  position + (even ? PathBase.AdjacentEven(i) : PathBase.AdjacentOdd(i)),
                                  transformToCheck))
                    {
                        visible = true;
                        break;
                    }
                }

                if (!visible) return true;
            }

            return false;
        }

        public static bool RayTrace(Vector3 testPosition, Vector3 position, Transform testTransform)
        {
            Ray ray = new Ray(testPosition, position - testPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Vector3.Distance(testPosition, position), 1 << 10))
            {
                if (hit.transform != testTransform) return true;
            }

            return false;
        }
    }
}