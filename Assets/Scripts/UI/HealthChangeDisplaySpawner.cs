using System.Collections;
using System.Collections.Generic;
using RPG.UI;
using UnityEngine;

public class HealthChangeDisplaySpawner : MonoBehaviour
{
    public HealthChangeDisplay healthChangeDisplay;

    public void SpawnHealthChangeDisplay(float delta)
    {
        if (healthChangeDisplay)
        {
            HealthChangeDisplay h = Instantiate(healthChangeDisplay, transform.position, Quaternion.identity);
            h.Initialize(delta);
        }
    }
}
