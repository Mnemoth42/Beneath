using TkrainDesigns.ScriptableEnums;
using UnityEngine;

namespace TkrainDesigns.Stats
{
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
            int successes = 0;
            for (int i = 0; i < (int) amount; i++)
            {
                float attackRoll = Random.Range(0.0f, attackValue);
                float defenseRoll = Mathf.Max(Random.Range(0.0f, defenseValue),1.0f);
                if (attackRoll > defenseRoll) successes++;
                float damageThisRoll = Mathf.Clamp(attackRoll / defenseRoll, .33f, 3.0f);
                result += damageThisRoll;
            }
            Debug.Log($"Brokering {attacker.name}'s attack on {defender.name}.  Attack Value: {amount}, attackValue={attackValue}, defenseValue={defenseValue}");
            Debug.Log($"Successes = {successes}.  Total Damage: {result}");
            return result;
        }
    }
}