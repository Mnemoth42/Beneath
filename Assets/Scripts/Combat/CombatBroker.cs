using System.Collections;
using System.Collections.Generic;
using TkrainDesigns.ScriptableEnums;
using TkrainDesigns.Stats;
using UnityEngine;

public class CombatBroker : MonoBehaviour
{
    public static float CalculateDamage(GameObject attacker, GameObject defender, float amount, ScriptableStat offense,
                                        ScriptableStat defense)
    {
        float result = 0f;
        PersonalStats attackerStats = attacker.GetComponent<PersonalStats>();
        PersonalStats defenderStats = defender.GetComponent<PersonalStats>();
        float attackValue = attackerStats.GetStatValue(offense);
        float defenseValue = defenderStats.GetStatValue(defense);
        
        for (int i = 0; i < (int) amount; i++)
        {
            float attackRoll = Random.Range(0.0f, attackValue);
            float defenseRoll = Mathf.Max(Random.Range(0.0f, defenseValue),1.0f);
            float damageThisRoll = Mathf.Clamp(attackRoll / defenseRoll, .33f, 3.0f);
            result += damageThisRoll;
        }

        result *= attackerStats.GetPercentageModifiers(offense);
        result /= attackerStats.GetPercentageModifiers(defense);
        return result;
    }
}
