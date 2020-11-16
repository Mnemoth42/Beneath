using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TkrainDesigns.Tiles.Stats.UI
{
    public class UIYouHaveStats : MonoBehaviour
    {

        StatStore statStore;
        TextMeshProUGUI text;
        Button button;
        void Awake()
        {
            statStore = GameObject.FindGameObjectWithTag("Player").GetComponent<StatStore>();
            button = GetComponent<Button>();
            text = GetComponentInChildren<TextMeshProUGUI>();
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
                button.image.enabled = true;
                text.text = "You have uncommitted stat points!";
                return;
            }

            if (statStore.IncreasesToSpend() > 0)
            {
                button.image.enabled = true;
                text.text = "You have unspent stat points!";
                return;
            }

            button.image.enabled = false;
            text.text = "";
        }
    }
}