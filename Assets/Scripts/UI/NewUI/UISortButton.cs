using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;

public class UISortButton : MonoBehaviour
{
    Inventory inventory;

    void Awake()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    public void SortItems()
    {
        inventory.SortInventory();
    }
}
