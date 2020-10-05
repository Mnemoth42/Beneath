using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TkrainDesigns.Tiles.Combat
{
    public class GridWeapon : MonoBehaviour
    {
        public  UnityEvent onHit;

        public void OnHit()
        {
            onHit?.Invoke();
        }

    }

}