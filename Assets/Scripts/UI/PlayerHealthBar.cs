using System;
using System.Collections;
using System.Collections.Generic;
using TkrainDesigns.Grids.Stats;
using TkrainDesigns.Tiles.Control;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class PlayerHealthBar : MonoBehaviour
{
    Slider slider;
    Health health;
    void Awake()
    {
        slider = GetComponent<Slider>();
        PlayerController player = FindObjectOfType<PlayerController>();
        health = player.GetComponent<Health>();
        
    }

    void OnEnable()
    {
        health.onTakeDamage.AddListener(UpdateHealthBar);
        health.onDeath.AddListener(OnDeath);
    }

    void OnDisable()
    {
        health.onDeath.RemoveListener(OnDeath);
        health.onTakeDamage.RemoveListener(UpdateHealthBar);
    }

    public void UpdateHealthBar()
    {
        slider.value = health.HealthAsPercentage;
    }

    public void OnDeath()
    {
        slider.gameObject.SetActive(false);
    }

}
