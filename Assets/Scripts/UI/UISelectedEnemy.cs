using System;
using System.Collections;
using System.Collections.Generic;
using TkrainDesigns.Grids.Stats;
using TkrainDesigns.Stats;
using TkrainDesigns.Tiles.Control;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISelectedEnemy : MonoBehaviour
{
    [SerializeField] GameObject enemyPanel;   
    [Header("Enemy Description")]
    [SerializeField] Image playerIcon;
    [SerializeField] TextMeshProUGUI enemyName;
    [SerializeField] TextMeshProUGUI enemyDescription;
    [Header("Health")]
    [SerializeField] Image healthBar;
    [SerializeField] TextMeshProUGUI healthText;

    [Header("Level")] [SerializeField] TextMeshProUGUI LevelText;

    
    PlayerController player;
    Health health;
    PersonalStats stats;


    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        player.onTargetChanged += UpdateDisplay;
        if (enemyPanel)
        {
            enemyPanel.SetActive(false);
        }
        
    }

    void UpdateDisplay(BaseController target)
    {
        if (target == null)
        {
            enemyPanel.SetActive(false);
            if (health)
            {
                health.onDeath.RemoveListener(CancelDisplay);
                health.onTakeDamage.RemoveListener(UpdateHealth);
            }
        }
        else
        {
            enemyPanel?.SetActive(true);
            stats = target.GetComponent<PersonalStats>();
            LevelText.text = stats.Level.ToString();
            health = target.GetComponent<Health>();
            health.onDeath.AddListener(CancelDisplay); 
            health.onTakeDamage.AddListener(UpdateHealth);
            UpdateHealth();
            playerIcon.sprite = stats.Avatar;
            enemyDescription.text = stats.GetDescription();
            enemyName.text = stats.GetCharacterName();
        }
    }

    void UpdateHealth()
    {
        healthBar.fillAmount = health.HealthAsPercentage;
        healthText.text = $"{(int) health.CurrentHealth}/{(int) health.MaxHealth}";
    }

    void CancelDisplay()
    {
        enemyPanel.SetActive(false);
    }

}
