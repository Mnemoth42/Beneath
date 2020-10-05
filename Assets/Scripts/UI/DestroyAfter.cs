using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    [SerializeField] float timeToLive = 5.0f;
    void Start()
    {
        Destroy(gameObject, timeToLive);
    }

    
}
