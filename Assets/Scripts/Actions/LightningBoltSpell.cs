using System;
using System.Collections;
using System.Collections.Generic;
using DigitalRuby.LightningBolt;
using TkrainDesigns.Attributes;
using TkrainDesigns.Stats;
using TkrainDesigns.Tiles.Combat;
using UnityEditor;
using UnityEngine;


namespace TkrainDesigns.Tiles.Actions
{
    [CreateAssetMenu(fileName="LightningBolt", menuName = "Actions/New LightningBolt")]
    public class LightningBoltSpell : OffensiveSpellBase
    {
        [SerializeField] LightningBoltScript boltPrefab;

        public override void PerformAction(GameObject user, GameObject target = null, Action callback = null)
        {
            if (!target)
            {
                callback();
                return;
            }
            ActivateCooldown(user);
            CacheParameters(user, target, callback);
            currentUser.transform.LookAt(target.transform.position);
            InvokeCastAnimation(InvokeLightning);
        }

        void InvokeLightning()
        {
            LightningBoltScript bolt = Instantiate(boltPrefab, currentUser.transform.position, Quaternion.identity);
            bolt.AnimationMode = LightningBoltAnimationMode.Random;
            bolt.StartPosition = GetStartPosition();
            bolt.EndPosition = currentTarget.GetComponent<CombatTarget>().WorldAimPoint;
            float damageAmount =
                CombatBroker.CalculateDamage(currentUser, currentTarget, AdjustedDamage, damageStat, defenseStat);
            currentTarget.GetComponent<Health>().TakeDamage(damageAmount, currentUser);
            callbackAction?.Invoke();
        }

        

#if UNITY_EDITOR

        void SetLightningBolt(LightningBoltScript value)
        {
            if (boltPrefab == value) return;
            SetUndo("Set Bolt Prefab");
            boltPrefab = value;
            Dirty();
        }

        bool drawLightningBolt = true;
        public override void DrawCustomInspector(float width, GUIStyle style)
        {
            base.DrawCustomInspector(width, style);
            drawLightningBolt = EditorGUILayout.Foldout(drawLightningBolt, "LightningBolt Data", style );
            if (!drawLightningBolt) return;
            SetLightningBolt(DrawObjectList("Bolt Prefab", boltPrefab));

        }

#endif

    }
}