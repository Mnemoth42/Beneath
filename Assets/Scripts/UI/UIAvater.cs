using System;
using System.Collections;
using System.Collections.Generic;

using TkrainDesigns.Stats;
using TkrainDesigns.Tiles.Control;
using UnityEngine;
using UnityEngine.UI;

public class UIAvater : MonoBehaviour
{
    [SerializeField] bool isTarget = false;

    PlayerController player;
    PersonalStats stats;
    Image image;

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        stats = player.GetComponent<PersonalStats>();
        image = GetComponent<Image>();
    }

    void Start()
    {
        if (!isTarget)
        {
            image.sprite = stats.Avatar;
        }
    }

    public void UpdateTargetDisplay(BaseController target)
    {
        if (target)
        {
            stats = target.GetComponent<PersonalStats>();
            image.sprite = stats.Avatar;
        }
    }
}
