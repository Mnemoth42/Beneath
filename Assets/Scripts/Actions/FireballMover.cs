using System;
using System.Collections;
using System.Collections.Generic;
using TkrainDesigns.Tiles.Combat;
using UnityEngine;

public class FireballMover : MonoBehaviour
{
    CombatTarget target;
    Action callback;
    [SerializeField] float speed = 10.0f;
    [SerializeField] GameObject explosion;
    Vector3 targetPosition;

    public void SetTarget(Transform value, Action callbackAction)
    {
        callback = callbackAction;
        target = value.GetComponent<CombatTarget>();
        targetPosition = target.transform.TransformPoint(target.aimPoint);
    }

    void Update()
    {
        if (target)
        {
            transform.LookAt(targetPosition);
            transform.Translate(Vector3.forward * (Time.deltaTime * speed));
            if (Vector3.Distance(transform.position, targetPosition) < 1.0f)
            {
                if (explosion)
                {
                    Instantiate(explosion, transform.position, Quaternion.identity);
                }
                callback();
                Destroy(gameObject);
            }
        }
    }
}
