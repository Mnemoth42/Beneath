using TkrainDesigns.Tiles.Grids;
using UnityEngine;

namespace TkrainDesigns.Tiles.Core
{
    public class Visibility : MonoBehaviour
    {
        public float distanceToBeSeen = 10;
        bool hasBeenSeen = false;

        void Start()
        {
            foreach (MeshRenderer rend in GetComponentsInChildren<MeshRenderer>())
            {
                rend.enabled = false;
            }
        }

        public void TestVisibility(Vector2Int tile)
        {
            if (!hasBeenSeen)
            {
                if (Vector2Int.Distance(tile, TileUtilities.GridPosition(transform.position)) < distanceToBeSeen)
                {
                    hasBeenSeen = true;
                    foreach (MeshRenderer rend in GetComponentsInChildren<MeshRenderer>())
                    {
                        rend.enabled = true;
                    }
                }
            }
        }
    }
}