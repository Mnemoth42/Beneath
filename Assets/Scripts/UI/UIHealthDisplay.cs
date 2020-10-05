using System;
using System.Collections;
using System.Collections.Generic;
using TkrainDesigns.Grids.Stats;
using TkrainDesigns.Tiles.Control;
using TMPro;
using UnityEngine;

public class UIHealthDisplay : MonoBehaviour
{
    [SerializeField] bool isTarget = false;

    TextMeshProUGUI text;
    Health health;
    PlayerController player;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        player = FindObjectOfType<PlayerController>();
        health = player.GetComponent<Health>();
    }

    void Start()
    {
        
        UpdateDisplay();
    }

    void OnEnable()
    {
        if (!isTarget)
        
            health.onTakeDamage.AddListener(UpdateDisplay);
    }

    void OnDisable()
    {
        if (!isTarget)
        
            health.onTakeDamage.RemoveListener(UpdateDisplay);
    }

    public void UpdateTargetDisplay(BaseController target)
    {
        if (health)
        {
            health.onTakeDamage.RemoveListener(UpdateDisplay);
        }
        health = target.GetComponent<Health>();
        health.onTakeDamage.AddListener(UpdateDisplay);
        UpdateDisplay();
    }



    void UpdateDisplay()
    {
       
        text.text = $"Health: {(int)health.CurrentHealth}/{(int)health.MaxHealth}";
    }
}
