using System.Collections;
using System.Collections.Generic;
using TkrainDesigns.Tiles.Grids;
using UnityEngine;

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
