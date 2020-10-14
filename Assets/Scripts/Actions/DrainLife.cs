using System;
using System.Collections;
using System.Collections.Generic;
using TkrainDesigns.Grids.Stats;
using TkrainDesigns.ScriptableEnums;
using TkrainDesigns.Stats;
using TkrainDesigns.Tiles.Actions;
using UnityEditor;
using UnityEngine;

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

    

    public override bool CanUse(GameObject user)
    {
        if(!base.CanUse(user)) return false;
        if (user.GetComponent<Health>().HealthAsPercentage >= 1.0f) return false;
        return true;
    }

    GameObject currentUser;
    GameObject currentTarget;
    Action callbackAction;

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
        float damageToDo = CombatBroker.CalculateDamage(currentUser, currentTarget, baseDamage, attackStat, defenseStat);
        Debug.Log($"{currentUser.name} is attacking for {damageToDo} Drain points.");
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
        Debug.Log($"Raw heal amount = {amount}.  HealModifiers = {HealModifiers()}.  HealPercent = {HealPercent()}.  Amount Healed = {amount*HealModifiers()*HealPercent()}");
        currentUser.GetComponent<Health>().Heal(amount*HealModifiers()*HealPercent());
        callbackAction?.Invoke();
    }

    public override string GetDescription()
    {
        string result = base.GetDescription();
        result += $"\nBase Damage {baseDamage}";
        if (attackStat)
        {
            result += $"\nMultiplies by {attackStat.Description}";
        }
        else
        {
            result += "\n<color=#ff8888>No Attack Stat Set!</color>";
        }

        if (defenseStat)
        {
            result += $"\nDefended against by {defenseStat.Description}";
        }
        else
        {
            result += "\n<color=#ff8888>No Defense Stat Set!</color>";
        }

        return result;
    }

#if UNITY_EDITOR

    void SetDamage(float amount)
    {
        if (baseDamage.Equals(amount)) return;
        Undo.RecordObject(this, "Set Damage");
        baseDamage = amount;
        EditorUtility.SetDirty(this);
    }

    void SetHealPercent(float amount)
    {
        if (healPercent.Equals(amount)) return;
        Undo.RecordObject(this, "Set Heal Percentage");
        healPercent = amount;
        EditorUtility.SetDirty(this);
    }

    void SetAttackStat(ScriptableStat stat)
    {
        if (attackStat == stat) return;
        Undo.RecordObject(this, "Set Attack Stat");
        attackStat = stat;
        EditorUtility.SetDirty(this);
    }

    void SetDefenseStat(ScriptableStat stat)
    {
        if (defenseStat == stat) return;
        Undo.RecordObject(this, "Set Defense Stat");
        defenseStat = stat;
        EditorUtility.SetDirty(this);
    }

    bool showDrainLife = true;
    public override void DrawCustomInspector(float width, GUIStyle style)
    {
        base.DrawCustomInspector(width, style);
        showDrainLife = EditorGUILayout.Foldout(showDrainLife, "Drain Life Data", style);
        if (!showDrainLife) return;
        GUIContent damageLabel = new GUIContent("Base Damage", "This will be multiplied by the user's damage stat to determine raw hit value");
        SetDamage(EditorGUILayout.IntSlider(damageLabel, (int)baseDamage, 1,100));
        GUIContent heallabel = new GUIContent("Heal Percent", "Percentage of damage dealt that the user will heal.");
        SetHealPercent(EditorGUILayout.IntSlider(heallabel, (int) healPercent, 1, 100));
        GUIContent attackLabel = new GUIContent("Attack Stat", "The stat that baseDamage multiplies with to calculate raw hit value.");
        SetAttackStat((ScriptableStat)EditorGUILayout.ObjectField(attackLabel, attackStat, typeof(ScriptableStat), false));
        GUIContent defenseLabel = new GUIContent("Defense Stat", "The stat that the target will use to defend against this attack.");
        SetDefenseStat((ScriptableStat)EditorGUILayout.ObjectField(defenseLabel, defenseStat, typeof(ScriptableStat), false));
    }

#endif

}
