using System;
using System.Collections;
using System.Collections.Generic;
using TkrainDesigns.Stats;
using TkrainDesigns.Tiles.Control;
using TMPro;
using UnityEngine;

public class UINameDisplay : MonoBehaviour
{
    TextMeshProUGUI text;
    BaseController target;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateTargetDisplay(BaseController newTarget)
    {
        if (newTarget == null)
        {
            text.text= "";
        }

        target = newTarget;
        var stats = target.GetComponent<PersonalStats>();
        if (stats)
        {
            text.text = stats.GetCharacterName();
        }
    }


}
