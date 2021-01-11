using System;
using System.Collections;
using System.Collections.Generic;
using TkrainDesigns.Attributes;
using TkrainDesigns.ScriptableEnums;
using TkrainDesigns.Stats;
using TkrainDesigns.Tiles.Actions;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName="SlimeAttack", menuName="Actions/New SlimeAttack")]
public class Slimer : PerformableActionItem
{
    [SerializeField] int baseDamage = 10;
    [SerializeField] ScriptableStat attackStat;
    [SerializeField] ScriptableStat defenseStat;

    

    public override bool CanUse(GameObject user)
    {
        if (base.CanUse(user))
        {
            return defenseStat != null && attackStat != null;
        }

        return false;
    }

    public override int Range(GameObject user)
    {
        return 0;
    }

    public override void PerformAction(GameObject user, GameObject target = null, Action callback = null)
    {
        CacheParameters(user, target, callback);
        ActivateCooldown(currentUser);
        InvokeCastAnimation(Damage);
    }

    

    void Damage()
    {
        float actualDamage =
            CombatBroker.CalculateDamage(currentUser, currentTarget, baseDamage, attackStat, defenseStat);
        currentTarget.GetComponent<Health>().TakeDamage(actualDamage, currentUser);
        callbackAction.Invoke();
    }

    public override bool AIRangedAttackSpell()
    {
        return true;
    }

    public override string GetDescription()
    {
        string result = base.GetDescription();
        result += $"\n\n Base Damage {baseDamage}";
        return result;
    }

#if UNITY_EDITOR
    bool drawSlimer;
    public override void DrawCustomInspector(float width, GUIStyle style)
    {
        base.DrawCustomInspector(width, style);
        drawSlimer = EditorGUILayout.Foldout(drawSlimer, "Draw Slimer Data");
        if (!drawSlimer) return;
        SetItem(ref baseDamage, EditorGUILayout.IntSlider("Base Damage", baseDamage, 1,100),"Base Damage");
        DrawStat(ref attackStat, "Attack Stat", "Stat used to determine attacker's success in attacking");
        DrawStat(ref defenseStat, "Defense Stat", "Stat used to determine defender's success against attack");
    }




#endif

}
