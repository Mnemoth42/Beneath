using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TkrainDesigns.Tiles.Control
{
    public class RagdollMinder : MonoBehaviour
    {
        void Start()
        {
            FreezeRagDoll();
        }

        public void RagDollEnabled()
        {
            foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            {
                rb.isKinematic = false;
            }
            Invoke(nameof(FreezeRagDoll), 2.0f);
        }

        void FreezeRagDoll()
        {
            foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            {
                rb.isKinematic = true;
            }
        }
    } 
}
