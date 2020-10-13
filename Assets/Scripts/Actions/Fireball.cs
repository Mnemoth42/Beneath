using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using TkrainDesigns.Grids.Stats;
using TkrainDesigns.ScriptableEnums;
using TkrainDesigns.Stats;
using TkrainDesigns.Tiles.Combat;
using TkrainDesigns.Tiles.Control;
using UnityEditor;
using UnityEngine;


namespace TkrainDesigns.Tiles.Actions
{
    [CreateAssetMenu(fileName = "Fire Spell", menuName = "Actions/New Fire Spell")]
    public class Fireball : PerformableActionItem
    {
        [SerializeField] float baseDamage = 5.0f;
        [SerializeField] GameObject fireball;

        [SerializeField] ScriptableStat damageStat;
        [SerializeField] ScriptableStat defenseStat;
        [SerializeField] ScriptableStat intelligenceStat;
        [SerializeField] int range = 3;

        public override string TimerToken()
        {
            return "Fireball";
        }

        public override bool ShouldPerform()
        {
            return true;
        }

        public override bool AIRangedAttackSpell()
        {
            return true;
        }

        public override int Range(GameObject user)
        {
            if (user == null) return 0;
            PersonalStats stats = user.GetComponent<PersonalStats>();
            if (stats && intelligenceStat)
            {
                return (int)(range + (stats.GetStatValue(intelligenceStat) * .1f));
            }
            return range;
        }

        Action finished;
        GameObject currentUser;
        GameObject currentTarget;
        Action callbackAction;

        public override void PerformAction(GameObject user, GameObject target = null, Action callback = null)
        {
            if (!target)
            {
                callback();
                return;
            }
            ActivateCooldown(user);
            finished = callback;
            currentUser = user;
            currentTarget = target;
            callbackAction = callback;
            user.transform.LookAt(target.transform.position);
            user.GetComponent<ActionPerformer>().SetAnimatorCallback(ReleaseFireball);
            user.GetComponent<Animator>().SetTrigger("Cast");

        }


        void ReleaseFireball()
        {
            Vector3 startPosition = currentUser.transform.position + Vector3.up;
            GridFighter fighter = currentUser.GetComponent<GridFighter>();
            if (fighter.GetRightHandTransform() != null)
            {
                startPosition = fighter.GetRightHandTransform().position;
            }

            GameObject go = Instantiate(fireball, startPosition, Quaternion.identity);
            FireballMover mover = go.GetComponent<FireballMover>();
            mover.SetTarget(currentTarget.transform, Damage);

        }


        void Damage()
        {
            currentTarget.GetComponent<Health>()
                         .TakeDamage(CombatBroker.CalculateDamage(currentUser, currentTarget,
                                                                  baseDamage, damageStat, defenseStat), currentUser);
            callbackAction();
        }

        public override bool Use(GameObject user)
        {
            if (user.TryGetComponent(out PlayerController controller))
            {
                return controller.SetCurrentAction(this);

            }

            return false;
        }

#if UNITY_EDITOR

        void SetBaseDamage(float value)
        {
            if (baseDamage == value) return;
            Undo.RecordObject(this, "Change Base Damage");
            baseDamage = value;
            Dirty();
        }

        void SetDamageStat(ScriptableStat stat)
        {
            if (damageStat == stat) return;
            Undo.RecordObject(this, "Set Damage Stat");
            damageStat = stat;
            Dirty();
        }

        void SetDefenseStat(ScriptableStat stat)
        {
            if (defenseStat == stat) return;
            Undo.RecordObject(this, "Set Defense Stat");
            defenseStat = stat;
            Dirty();
        }

        void SetIntelligenceStat(ScriptableStat stat)
        {
            Undo.RecordObject(this, "Set Intelligence Stat");
            intelligenceStat = stat;
            Dirty();
        }


        void SetFireball(GameObject go)
        {
            if (fireball == go) return;
            Undo.RecordObject(this, "Change Projectile");
            fireball = go;
            Dirty();
        }

        

        void SetRange(int newRange)
        {
            Undo.RecordObject(this, "Set Range");
            range = newRange;
            Dirty();
        }

        public override void DrawCustomInspector(float width, GUIStyle style)
        {
            base.DrawCustomInspector(width, style);
            SetFireball((GameObject)EditorGUILayout.ObjectField("Projectile", fireball, typeof(GameObject), true));
            SetBaseDamage((float)EditorGUILayout.IntSlider("Base Damage", (int)baseDamage, 1, 100));
            SetRange(EditorGUILayout.IntSlider("Range", range, 0, 8));
            SetDamageStat((ScriptableStat)EditorGUILayout.ObjectField("Damage Stat", damageStat,
                                                                      typeof(ScriptableStat), false));
            SetDefenseStat((ScriptableStat)EditorGUILayout.ObjectField("Defense Stat", defenseStat, typeof(ScriptableStat), false));
            SetIntelligenceStat((ScriptableStat)EditorGUILayout.ObjectField("Intelligence Stat", intelligenceStat, typeof(ScriptableStat), false));
        }

#endif

    }
}