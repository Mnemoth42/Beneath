﻿using System;
using TkrainDesigns.Attributes;
using TkrainDesigns.ScriptableEnums;
using TkrainDesigns.Stats;
using UnityEditor;
using UnityEngine;

namespace TkrainDesigns.Tiles.Actions
{
    [CreateAssetMenu(fileName = "New Drain", menuName = "Actions/New DrainLife Spell")]
    public class DrainLife : PerformableActionItem
    {
        [SerializeField] ScriptableStat attackStat;
        [SerializeField] ScriptableStat defenseStat;
        [SerializeField] float baseDamage = 3.0f;
        [SerializeField] float healPercent = 50.0f;


        public override bool AIRangedAttackSpell()
        {
            return true;
        }
        protected int AdjustedDamage
        {
            get
            {
                if (currentUser.TryGetComponent(out PersonalStats stats))
                {
                    return (int) (baseDamage + (stats.Level / 2.0f));
                }
                return (int)baseDamage;
            }
        }
    

        public override bool CanUse(GameObject user)
        {
            if(!base.CanUse(user)) return false;
            if (user.GetComponent<Health>().HealthAsPercentage >= 1.0f) return false;
            return true;
        }

    

        public override void PerformAction(GameObject user, GameObject target = null, Action callback = null)
        {
            if (!user || !target)
            {
                callback?.Invoke();
                return;
            }
            ActivateCooldown(user);
            currentUser = user;
            currentTarget = target;
            callbackAction = callback;
            user.GetComponent<Animator>().SetTrigger("Cast");
            user.GetComponent<ActionPerformer>().SetAnimatorCallback(Leech);
        }

        public override int Range(GameObject user)
        {
            return 0;
        }

        void Leech()
        {
            float damageToDo = CombatBroker.CalculateDamage(currentUser, currentTarget, AdjustedDamage, attackStat, defenseStat);
            //Debug.Log($"{currentUser.name} is attacking for {damageToDo} Drain points.");
            Heal(currentTarget.GetComponent<Health>().TakeDamage(damageToDo,  currentUser));
        }

        float HealPercent()
        {
            return healPercent / 100.0f;
        }

        float HealModifiers()
        {
            return (currentUser.GetComponent<PersonalStats>().GetPercentageModifiers(attackStat) / 100.0f)+1.0f;
        }

        void Heal(float amount)
        {
            // Debug.Log($"Raw heal amount = {amount}.  HealModifiers = {HealModifiers()}.  HealPercent = {HealPercent()}.  Amount Healed = {amount*HealModifiers()*HealPercent()}");
            currentUser.GetComponent<Health>().Heal(amount*HealModifiers()*HealPercent());
            callbackAction?.Invoke();
        }

        public override string GetDescription()
        {
            string result = base.GetDescription();
            result += $"\nBase Damage {baseDamage}";
            if (attackStat)
            {
                result += $"\nAttacks calculated using {GoodString(attackStat.DisplayName)}";
            }
            else
            {
                result += $"\n{BadString("Attack Stat Not Set!")} ";
            }

            if (defenseStat)
            {
                result += $"\nDefended against by {GoodString(defenseStat.DisplayName)}";
            }
            else
            {
                result += $"\n{BadString("Defense Stat Not Set!")}";
            }

            return result;
        }

#if UNITY_EDITOR

   

        bool showDrainLife = true;
        public override void DrawCustomInspector(float width, GUIStyle style)
        {
            base.DrawCustomInspector(width, style);
            showDrainLife = EditorGUILayout.Foldout(showDrainLife, "Drain Life Data", style);
            if (!showDrainLife) return;
            DrawFloatAsIntSlider(ref baseDamage,1,100,"Base Damage", "This will be multiplied by the user's damage to determine the raw hit value");
            DrawFloatAsIntSlider(ref healPercent, 1,200, "Healing Percent", "Amount of damage applied that will be absorbed by the user");
            DrawStat(ref attackStat, "Attack Stat", "The stat that the target will use to calculate raw hit value.");
            DrawStat(ref defenseStat, "Defense Stat", "The stat that the target will use to defend against this attack.");
        }

#endif

    }
}