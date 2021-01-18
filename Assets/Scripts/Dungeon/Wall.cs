using TkrainDesigns.Tiles.Grids;
using UnityEngine;

namespace TkrainDesigns.Dungeons
{
    public class Wall : MonoBehaviour
    {
        [SerializeField] GameObject[] evenWalls;
        [SerializeField] GameObject[] oddWalls;

        public void ActivateWall(int face)
        {
            if(transform.position.ToGridPosition().x%1==0) 
                evenWalls[face].SetActive(true);
            else oddWalls[face].SetActive(true);
        }
    }
}