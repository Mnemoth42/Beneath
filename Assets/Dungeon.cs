
using System;
using System.Collections.Generic;
using System.Linq;
using TkrainDesigns.Stats;
using TkrainDesigns.Tiles.Control;
using TkrainDesigns.Tiles.Grids;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

namespace TkrainDesigns.Tiles.Dungeons
{
    public class Dungeon : MonoBehaviour
    {
        [Header("Floor Tile")]
        [SerializeField] Tile tile;
        [Header("Wall tile")]
        [SerializeField] GameObject wall;
        [Header("Exit Tile")] [SerializeField] GameObject exit;
        [Header("Initial map extent.")]
        [SerializeField] [Range(5,100)] int width = 25;
        [SerializeField] [Range(5, 100)] int height = 25;
        [Header("Probabilities")]
        [SerializeField] [Range(1,10)]int branchProbability = 8;
        [SerializeField] [Range(1,10)]int roomProbability = 3;
        [SerializeField] [Range(1, 10)] int roomNoise;

        [Header("Dropped Items/Enemies")] [SerializeField]
        GameObject[] drops;

        int level = 0;

        public int Level
        {
            get { return level; }
        }
        

        Dictionary<Vector2Int, int> map;
        Vector2Int start;
        Vector2Int finish;

        public Vector2Int Finish => finish;


        void Start()
        {
            level += 1;
            GenerateNewDungeon();
        }

        public void GenerateNewDungeon()
        {
            CreateDungeon();
            CreateTiles();
            CreateWalls();

            MovePlayer();
            ControllerCoordinator.BeginNextControllerTurn();
        }


        static bool TestLocationBounds(Vector2Int loc)
        {
            return (loc.x>=0 && loc.y>=0);
        }

        void MovePlayer()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player)
            {
                PlayerController control = player.GetComponent<PlayerController>();
                control.InitializePlayer( TileUtilities.IdealWorldPosition(start));
                control.ResetTurn();
                control.SetFinish(finish, GenerateNewDungeon);
            }
        }

        void CreateDungeon()
        {
            ClearPreviousMap();
            map = new Dictionary<Vector2Int, int>();
            BuildEssentialPath();
            AddRandomPaths();
        }

        void AddRandomPaths()
        {
            List<Vector2Int> pathToExpand = new List<Vector2Int>();
            foreach (var keyPair in map)
            {
                pathToExpand.Add(keyPair.Key);
            }
            foreach (var item in pathToExpand)
            {
                if (Random.Range(0, 10) < 8)
                {
                    Vector2Int tileToTry = NextRandomTile(item);
                    while (Chance(branchProbability))
                    {
                        tileToTry = NextRandomTile(tileToTry);
                        
                    }
                }
            }
        }

        Vector2Int NextRandomTile(Vector2Int item)
        {
            Vector2Int direction = Vector2Int.zero;
            switch (Random.Range(0, 4))
            {
                case 0:
                    direction = Vector2Int.up;
                    break;
                case 1:
                    direction = Vector2Int.down;
                    break;
                case 2:
                    direction = Vector2Int.left;
                    break;
                case 3:
                    direction = Vector2Int.right;
                    break;
            }

            Vector2Int tileToTry = item+direction;
            if (tileToTry.x >= 0 && tileToTry.y >= 0)
            {
                if (TestLocationBounds(tileToTry))
                {
                    map[tileToTry] = 1;
                }

                bool extended = false;
                while (Chance(roomProbability))
                {
                    tileToTry += direction;
                    if (tileToTry.x < 0 || tileToTry.y < 0)
                    {
                        break;
                    }

                    map[tileToTry] = 1;
                    extended = true;
                }


                if (!extended)
                {
                    return tileToTry;
                }

                int size = Random.Range(2, 5);
                {
                    for (int x = -size*2; x < size*2; x++)
                    {
                        for (int y = -size*2; y < size*2; y++)
                        {
                            Vector2Int t = new Vector2Int(x, y) + tileToTry;
                            if (t.x < 0 || t.y < 0)
                            {
                                continue;
                            }

                            if (Vector2Int.Distance(t, tileToTry) < size && !Chance(roomNoise))
                            {
                                map[t] = 1;
                            }
                        }
                    }

                    map[tileToTry] = 2;
                }

                return tileToTry;
            }

            return item;
        }

        static bool Chance(int probability)
        {
            return Random.Range(0, 10) < probability;
        }

        void ClearPreviousMap()
        {
            foreach (var child in GetComponentsInChildren<Transform>())
            {
                if (child.transform != this.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        void BuildEssentialPath()
        {
            int currentY = height / 2;
            int currentX = width/2;

            Vector2Int currentPosition = start = new Vector2Int(currentX, currentY);
            map[currentPosition] = 1;
            //Create guaranteed path to finish;
            while (currentPosition.x < width + width/2)
            {
                int toss = Random.Range(0, 3);
                currentPosition += toss == 0 ? Vector2Int.right : (toss == 1 ? Vector2Int.up : Vector2Int.down);
                if (currentPosition.y < 0)
                {
                    currentPosition.y = 0;
                }

                map[currentPosition] = 1;
                TestLocationBounds(currentPosition);
            }

            finish = currentPosition;
        }

        

        void CreateTiles()
        {
            Quaternion rotation = tile.transform.rotation;
            foreach (var pair in map)
            {
                if (pair.Key.x >= 0 && pair.Key.y >= 0)
                {
                    Tile inst = Instantiate(tile, TileUtilities.IdealWorldPosition(pair.Key), rotation);
                    inst.transform.SetParent(transform);
                    if (pair.Value > 1 && pair.Key!=start && pair.Key!=finish)
                    {
                        GameObject go = Instantiate(drops[Random.Range(0, drops.Length)],
                                                    TileUtilities.IdealWorldPosition(pair.Key), Quaternion.identity);
                        go.transform.SetParent(transform);
                        if (go.TryGetComponent<PersonalStats>(out PersonalStats stats))
                        {
                            stats.SetLevel(Mathf.Max(Random.Range(level, level+2),1));
                        }
                    }

                    
                }


            }

            GameObject exitObject = Instantiate(exit, TileUtilities.IdealWorldPosition(finish), Quaternion.identity);
            exitObject.transform.SetParent(transform);
        }
        //TODO: Calculate bounds of actual dungeon 
        void CreateWalls()
        {
            List<Vector2Int> actualTiles=new List<Vector2Int>();
        
            int startX = width;
            int startY = height;
            int endx = 0;
            int endy = 0;
            foreach (var pair in map)
            {
                actualTiles.Add(pair.Key);
                endx = Mathf.Max(endx, pair.Key.x);
                endy = Mathf.Max(endy, pair.Key.y);
                startX = Mathf.Min(startX, pair.Key.x);
                startY = Mathf.Min(startY, pair.Key.y);
            }
            //Debug.Log($"({startX},{startY} - {endx}, {endy}");
            for(int x=startX-10;x<endx+10;x++)
            {
                for (int y = startY-10; y < endy + 10; y++)
                {
                    Vector2Int testLocation = new Vector2Int(x,y);
                    var nearestTile = actualTiles.OrderBy(t => Vector2Int.Distance(t, testLocation)).First();
                    bool closeEnough = (Vector2Int.Distance(testLocation, nearestTile) < 10);
                    if (closeEnough)
                    {
                        if (!map.ContainsKey(new Vector2Int(x, y)) || x < 0 || y < 0)
                        {
                            GameObject inst = Instantiate(wall, TileUtilities.IdealWorldPosition(new Vector2Int(x, y)),
                                                          Quaternion.identity);
                            inst.transform.SetParent(transform);
                        }
                    }
                }
            }
        }

    
    }
}