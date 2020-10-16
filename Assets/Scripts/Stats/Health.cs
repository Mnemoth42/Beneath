using System;
using System.Collections;
using System.Collections.Generic;
using TkrainDesigns.ScriptableEnums;
using TkrainDesigns.Stats;
using TkrainDesigns.Tiles.Stats;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace TkrainDesigns.Grids.Stats
{
    public class Health : MonoBehaviour
    {
        [System.Serializable]
        public class HealthChangeEvent : UnityEvent<float>
        {

        }

        [SerializeField] float startHealth;
        [Header("Place Health Scriptable Stat Here")]
        [SerializeField] ScriptableStat healthStat = null;
        public UnityEvent onDeath;
        public UnityEvent onTakeDamage;
        public HealthChangeEvent onTakeDamageFloat;
        float currentHealth;
        PersonalStats stats;
        bool isAlive = true;
        void Awake()
        {
            currentHealth = startHealth;
            stats = GetComponent<PersonalStats>();
            if (stats) currentHealth = -1;
        }

        void OnEnable()
        {
            if (stats)
            {
                stats.onLevelUpEvent.AddListener(OnLevelUp);
            }
        }

        void OnDisable()
        {
            if (stats)
            {
                stats.onLevelUpEvent.RemoveListener(OnLevelUp);
            }
        }

        void OnLevelUp()
        {
            float healthChange = MaxHealth - currentHealth;
            currentHealth = MaxHealth;
            onTakeDamageFloat?.Invoke(healthChange);
            onTakeDamage?.Invoke();
        }

        void Start()
        {
            currentHealth = MaxHealth;
        }

        public float MaxHealth
        {
            get
            {
                if (stats != null && healthStat != null)
                {
                    return stats.GetStatValue(healthStat);
                }

                return startHealth;
            }
        }

        public float CurrentHealth
        {
            get
            {
                if (currentHealth == -1)
                {
                    currentHealth = MaxHealth;
                }
                return currentHealth;
            }
        }
        

        public float TakeDamage(float amount, GameObject instigator)
        {
            float actualDamage = 0.0f;
            if (IsAlive)
            {
                float healthBeforeAttack = currentHealth;
                GetComponent<Animator>().SetTrigger("GetHit");
                currentHealth = Mathf.Floor(Mathf.Clamp(currentHealth - Mathf.Max(amount, 1.0f), 0, MaxHealth));
                actualDamage = (int)(healthBeforeAttack - currentHealth);
                //Debug.Log($"{name} takes {actualDamage} damage of the {amount} that was sent to TakeDamage.  {healthBeforeAttack}-{currentHealth}");
                onTakeDamage?.Invoke();
                onTakeDamageFloat?.Invoke(-actualDamage);
                if ((int)currentHealth < 1)
                {
                    isAlive = false;
                    if (instigator)
                    {
                        if (stats && instigator.TryGetComponent<Experience>(out Experience experience))
                        {
                            experience.GainExperience(stats.GetExperienceForKilling());
                        }
                    }
                    onDeath?.Invoke();
                }
            }
            return actualDamage;
        }

        public float Heal(float amountToHeal)
        {
            if (IsAlive)
            {
                float amountReallyHealed = Mathf.Min(amountToHeal, MaxHealth - currentHealth);
                GetComponent<Animator>().SetTrigger("Victory");
                currentHealth = Mathf.Clamp(currentHealth + amountReallyHealed, 0, MaxHealth);
                onTakeDamage?.Invoke();
                onTakeDamageFloat.Invoke(amountReallyHealed);

            }

            return currentHealth;
        }

        public bool IsAlive => isAlive;

        public bool IsDead => !IsAlive;

        public float HealthAsPercentage => currentHealth / MaxHealth;
    }
}