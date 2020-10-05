using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballMover : MonoBehaviour
{
    Transform target;
    Action callback;
    [SerializeField] float speed = 10.0f;

    public void SetTarget(Transform value, Action callbackAction)
    {
        callback = callbackAction;
        target = value;
    }

    void Update()
    {
        if (target)
        {
            transform.LookAt(target);
            transform.Translate(Vector3.forward * (Time.deltaTime * speed));
            if (Vector3.Distance(transform.position, target.position) < 1.0f)
            {
                callback();
                Destroy(gameObject);
            }
        }
    }
}
