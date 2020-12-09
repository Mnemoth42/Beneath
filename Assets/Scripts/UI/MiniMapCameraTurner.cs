using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCameraTurner : MonoBehaviour
{
    float targetAngle;
    float currentAngle;
    public float turnSpeed = 10.0f;

    // Update is called once per frame
    void Update()
    {
        currentAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * turnSpeed);
        transform.rotation = Quaternion.AngleAxis(currentAngle, new Vector3(0,1,0));
    }
    public void TurnCamera(bool left)
    {
        
        targetAngle+= left ? -60f : 60f;
        

    }
}
