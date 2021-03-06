﻿using UnityEngine;

namespace TkrainDesigns.Core
{
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
}
