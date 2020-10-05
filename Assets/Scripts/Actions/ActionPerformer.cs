using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPerformer : MonoBehaviour
{

    public void PerformActionStep(IEnumerator step)
    {
        StartCoroutine(step);
    }

    Action animatorCallback = null;

    public void SetAnimatorCallback(Action callback)
    {
        animatorCallback = callback;
    }

    public void Perform()
    {
        animatorCallback();
    }

}
