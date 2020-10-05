using JetBrains.Annotations;
using System.Collections.Generic;
using TkrainDesigns.Tiles.Grids;
using UnityEngine;

namespace TkrainDesigns.Tiles.Pathfinding
{
    

    public  class GridPathFinder<T>:PathBase where T : MonoBehaviour
    {
        static Dictionary<Vector2Int, T> map;

        [NotNull]
        public static Dictionary<Vector2Int, T> Map
        {
            get
            {
                if (map == null)
                {
                    ConductInventory();
                }

                return map;
            }
        }


        /// <summary>
        /// Creates or refreshes the internal map the GridPathFinder uses to find paths or report objects at a given location.
        /// This is normally called internally in the event that the map has not been initialized, but it can be called at any
        /// time in cases where the landscape has been altered.  In the event of dynamic generation, calling this after
        /// each time the dynamic grid is created is mandatory.
        /// </summary>
        public static void ConductInventory()
        {
            //Debug.Log("Gethering inventory");
            map = new Dictionary<Vector2Int, T>();
            foreach (T t in Object.FindObjectsOfType<T>())
            {
                Vector2Int key = TileUtilities.GridPosition(t.transform.position);
                if (Map.ContainsKey(key))
                {
                    //Debug.LogWarning($"{t.name} appears more than once in the grid.  Discarding extra tile.");
                }
                else
                {
                    Map[key] = t;
                }
            }
        }

        public static List<T> GetItemsAt(Vector2Int location)
        {
            List<T> result = new List<T>();
            foreach (T t in Object.FindObjectsOfType<T>())
            {
                Vector2Int test = TileUtilities.GridPosition(t.transform.position);
                if (test == location)
                {
                    result.Add(t);
                }
            }

            return result;
        }

        public static T GetItemAt(Vector2Int location)
        {
            return Map.ContainsKey(location) ? Map[location] : null;
        }

        public static bool DoesItemExistat(Vector2Int location)
        {
            return Map.ContainsKey(location);
        }

        [NotNull]
        public static Dictionary<Vector2Int, bool> GetLocations()
        {
            Dictionary<Vector2Int, bool> result = new Dictionary<Vector2Int, bool>();
            ConductInventory();
            foreach (var pair in Map)
            {
                result[pair.Key] = true;
            }

            return result;
        }

        public static Dictionary<Vector2Int, T> GetLocationsWithT()
        {
            ConductInventory();
            return Map;
        }

        public static List<Vector2Int> GetLocationsList()
        {
            List<Vector2Int> list = new List<Vector2Int>();
            foreach (var pair in Map)
            {
                list.Add(pair.Key);
            }

            return list;
        }

        
        static Dictionary<Vector2Int, Vector2Int> GetBreadCrumbs(Vector2Int start, Vector2Int finish,
                                                                 Dictionary<Vector2Int, bool> obstacles = null)
        {
            Dictionary<Vector2Int, Vector2Int> crumbs = new Dictionary<Vector2Int, Vector2Int>();
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            Dictionary<Vector2Int, bool> tried=new Dictionary<Vector2Int, bool>();
            if (obstacles!=null)
            {
                foreach (var obstacle in obstacles)
                {
                    if (map.ContainsKey(obstacle.Key))
                    {
                        tried[obstacle.Key] = true;
                    }
                } 
            }
            if (Map.ContainsKey(start) && Map.ContainsKey(finish))
            {
                //Debug.Log("Both {start} and {finish} are on the map, so we will try to get a path.");
                Vector2Int currentTile = start;
                TryEnqueueNeighbors();
                bool finished = false;
                while (!finished)
                {
                    if (queue.Count > 0)
                    {
                        currentTile = queue.Dequeue();
                        //Debug.Log($"Testing {currentTile}");
                        if (currentTile == finish)
                        {
                            //Debug.Log($"{currentTile} = {finish}, success!");
                            finished = true;
                        }
                        else
                        {
                            //Debug.Log($"Enqueuing neighbors of {currentTile}");
                            TryEnqueueNeighbors();
                        }
                    }
                    else
                    {
                        finished = true;
                    }
                }


                void TryEnqueue(Vector2Int vector2)
                {
                    if (map.ContainsKey(vector2) && !queue.Contains(vector2) && !tried.ContainsKey(vector2))
                    {
                        //Debug.Log($"Adding {vector2} to Queue");
                        queue.Enqueue(vector2);
                        tried[vector2] = true;
                        crumbs[vector2] = currentTile;
                    }
                    else
                    {
                        //Debug.Log($"Not adding {vector2} to Queue, as it has already been tried or queued");
                    }
                }

                void TryEnqueueNeighbors()
                {
                    bool odd = currentTile.x % 2 == 1;
                    if (currentTile.x < 0)
                    {
                        odd = !odd;
                    }

                    for (int i = 0; i < 6; i++)
                    {
                        TryEnqueue(currentTile+ (odd? DirectionsHexEven[i]: DirectionsHexOdd[i]));
                    }
                }
            }

            return crumbs;
        }

        

        [NotNull]
        public static List<Vector2Int> FindPath(Vector2Int start, Vector2Int finish,
                                                Dictionary<Vector2Int, bool> obstacles = null)
        {
            //Debug.Log("Finding Path:");
            if (obstacles == null)
            {
                obstacles = new Dictionary<Vector2Int, bool>();
            }
            ConductInventory();
            Dictionary<Vector2Int, Vector2Int> crumbs = GetBreadCrumbs(start, finish, obstacles);
            List<Vector2Int> result = new List<Vector2Int>();
            if (crumbs.ContainsKey(finish))
            {
                //Debug.Log("Crumbs have finish in location");
                Vector2Int current = finish;
                result.Add(finish);
                while (current != start)
                {
                    current = crumbs[current];
                    //Debug.Log($"Adding step: {current}");
                    result.Add(current);
                }

                result.Reverse();
            }

            return result;
        }
    }
}