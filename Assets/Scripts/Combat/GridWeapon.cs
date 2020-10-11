using System;
using System.Collections;
using System.Collections.Generic;
using TkrainDesigns.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace TkrainDesigns.Tiles.Combat
{
    public class GridWeapon : MonoBehaviour
    {
        public  UnityEvent onHit;
        public Vector3 EffectPoint = new Vector3(0,0,0);
        public Quaternion EffectRotation = Quaternion.identity;

        public void OnHit()
        {
            onHit?.Invoke();
        }

        public Vector3 GetEffectPosition()
        {
            return transform.TransformPoint(EffectPoint);
        }

        public Quaternion GetEffectRotation()
        {
            return transform.TransformDirection(EffectRotation.eulerAngles).ToQuaternion();
        }

        public void SetEffectRotation(Quaternion newRotation)
        {
            EffectRotation = transform.InverseTransformDirection(newRotation.eulerAngles).ToQuaternion();
        }

        public void SetEffectPoint(Vector3 newPoint)
        {
            EffectPoint = transform.InverseTransformPoint(newPoint);
        }

    }

}