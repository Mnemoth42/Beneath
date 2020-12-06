using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TkrainDesigns.Tiles.Combat
{
    public enum EAffiliation
    {
        Player,
        Npc,
        Bandits,
        Beasts,
        Other
    }

    public class CombatTarget : MonoBehaviour
    {
        public EAffiliation affiliation = EAffiliation.Bandits;
        public Vector3 aimPoint = new Vector3(0,1,0);

        public Vector3 WorldAimPoint => transform.TransformPoint(aimPoint);

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position+aimPoint, .25f);
        }
    }
}