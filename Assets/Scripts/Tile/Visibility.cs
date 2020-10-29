﻿using TkrainDesigns.Tiles.Grids;
using TkrainDesigns.Tiles.Pathfinding;
using UnityEngine;

namespace TkrainDesigns.Tiles.Core
{
    public class Visibility : MonoBehaviour
    {
        public float distanceToBeSeen = 10;
       

        void Start()
        {
            foreach (MeshRenderer rend in GetComponentsInChildren<MeshRenderer>())
            {
                rend.enabled = false;
            }
        }

        

        public void TestVisibility(Vector2Int tile)
        {
            
                if (Vector2Int.Distance(tile, TileUtilities.GridPosition(transform.position)) < distanceToBeSeen)
                {
                    Vector3 testPosition = TileUtilities.IdealWorldPosition(tile);
                    Vector3 position = transform.position;
                    Transform transformToCheck = transform;
                    if (ExtendedRayTrace(tile, testPosition, position, transformToCheck))
                    {
                        return;
                    }


                    foreach (MeshRenderer rend in GetComponentsInChildren<MeshRenderer>())
                    {
                        rend.enabled = true;
                        Destroy(this);
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