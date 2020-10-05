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
                stats.OnLevelUpEvent += OnLevelUp;
            }
        }

        void OnDisable()
        {
            if (stats)
            {
                stats.OnLevelUpEvent -= OnLevelUp;
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
                if (stats!=null && healthStat!=null)
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
        public float TakeDamage(float damage, ScriptableStat defensiveStat, GameObject instigator)
        {
            
            if (IsAlive)
            {
                if (stats != null && defensiveStat != null)
                {
                    damage = (int)Mathf.Max(damage / (stats.GetStatValue(defensiveStat)/10f), damage / 2.0f);
                    if (damage < 1.0f) damage = 1.0f;
                }
                GetComponent<Animator>().SetTrigger("GetHit");
                currentHealth = Mathf.Clamp(currentHealth - damage, 0, MaxHealth);
                onTakeDamage?.Invoke();
                onTakeDamageFloat?.Invoke(-damage);
                //Debug.Log($"{name} has {currentHealth} remaining!");
                if ((int)currentHealth<1)
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
            return currentHealth;
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