using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeFreezer : MonoBehaviour
{
    void OnEnable()
    {
        Time.timeScale = 0.0f;
    }

    void OnDisable()
    {
        Time.timeScale = 1.0f;
    }
}
