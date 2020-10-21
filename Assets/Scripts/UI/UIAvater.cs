using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using GameDevTV.UI.Inventories;
using TkrainDesigns.Stats;
using TkrainDesigns.Tiles.Combat;
using TkrainDesigns.Tiles.Control;
using UnityEngine;
using UnityEngine.UI;

public class UIAvater : MonoBehaviour, IItemHolder
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

    public InventoryItem GetItem()
    {
        GridFighter fighter = stats.GetComponent<GridFighter>();
        return fighter.GetCurrentWeaponConfig();
    }

    public InventoryItem GetTooltipItem()
    {
        return GetItem();
    }
}
