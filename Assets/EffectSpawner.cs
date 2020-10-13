using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    public GameObject effectToSpawn;
    public bool parentToCaller = false;

    public void SpawnEffect()
    {
        if (!effectToSpawn) return;
        if (parentToCaller)
        {
            Instantiate(effectToSpawn, transform);
        }
        else
        {
            Instantiate(effectToSpawn, transform.position, Quaternion.identity);
        }
    }
}
