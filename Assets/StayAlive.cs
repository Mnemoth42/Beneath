using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayAlive : MonoBehaviour
{
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }



}
