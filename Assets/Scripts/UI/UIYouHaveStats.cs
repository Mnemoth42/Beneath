using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TkrainDesigns.Tiles.Stats.UI
{
    public class UIYouHaveStats : MonoBehaviour
    {

        StatStore statStore;
        TextMeshProUGUI text;
        void Awake()
        {
            statStore = GameObject.FindGameObjectWithTag("Player").GetComponent<StatStore>();
            text = GetComponent<TextMeshProUGUI>();
        }

        void OnEnable()
        {
            statStore.onStatStoreUpdated += UpdateDisplay;
            UpdateDisplay();
        }

        void OnDisable()
        {
            statStore.onStatStoreUpdated -= UpdateDisplay;
        }

        void UpdateDisplay()
        {
            if (statStore.Dirty)
            {
                text.text = "You have uncommitted stat points!";
                return;
            }

            if (statStore.IncreasesToSpend() > 0)
            {
                text.text = "You have unspent stat points!";
                return;
            }

            text.text = "";
        }
    }
}