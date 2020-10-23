using System;
using System.Collections;
using System.Collections.Generic;
using TkrainDesigns.Tiles.Stats;
using TMPro;
using UnityEngine;

public class UIStatsToSpend : MonoBehaviour
{
    TextMeshProUGUI text;
    StatStore store;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        store = GameObject.FindGameObjectWithTag("Player").GetComponent<StatStore>();
    }

    void OnEnable()
    {
        store.onStatStoreUpdated += UpdateDisplay;
        UpdateDisplay();
    }

    void OnDisable()
    {
        store.onStatStoreUpdated -= UpdateDisplay;
    }

    void UpdateDisplay()
    {
        text.text = $"Stats to Spend: {store.IncreasesToSpend()}";
    }
}
