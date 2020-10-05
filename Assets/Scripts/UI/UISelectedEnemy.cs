using System;
using System.Collections;
using System.Collections.Generic;
using TkrainDesigns.Grids.Stats;
using TkrainDesigns.Tiles.Control;
using UnityEngine;

public class UISelectedEnemy : MonoBehaviour
{
    [SerializeField] GameObject enemyPanel = null;
    [SerializeField] UINameDisplay nameDisplay;
    [SerializeField] UIHealthDisplay healthDisplay;
    [SerializeField] UILevelDisplay levelDisplay;
    [SerializeField] UIDescription description;
    [SerializeField] UIAvater avatarDisplay;
    PlayerController player;
    Health health;

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
            }
            
        }
        else
        {
            enemyPanel?.SetActive(true);
            healthDisplay?.UpdateTargetDisplay(target);
            levelDisplay?.UpdateTargetDisplay(target);
            avatarDisplay?.UpdateTargetDisplay(target);
            description?.UpdateTargetDisplay(target);
            nameDisplay?.UpdateTargetDisplay(target);
            health = target.GetComponent<Health>();
            if (health)
            {
                health.onDeath.AddListener(CancelDisplay); 
            }
        }
    }

    void CancelDisplay()
    {
        enemyPanel.SetActive(false);
    }

}
