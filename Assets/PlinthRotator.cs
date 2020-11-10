using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlinthRotator : MonoBehaviour
{
    [SerializeField] float speed = 0f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, speed*Time.deltaTime);
    }

    public void SetSpeed(float value)
    {
        speed = value;
    }
}
