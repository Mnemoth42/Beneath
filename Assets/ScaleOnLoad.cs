using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScaleOnLoad : MonoBehaviour
{
    [SerializeField] float Min = .8f;
    [SerializeField] float Max = 1.2f;

    void Awake()
    {
        transform.localScale *= Random.Range(Min, Max);
    }
}
