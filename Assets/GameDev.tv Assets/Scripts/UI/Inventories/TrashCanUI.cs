using System.Collections;
using System.Collections.Generic;
using GameDevTV.Core.UI.Dragging;
using GameDevTV.Inventories;
using UnityEngine;

public class TrashCanUI : MonoBehaviour, IDragContainer<InventoryItem>
{
    public int MaxAcceptable(InventoryItem item)
    {
        return int.MaxValue;
    }

    public void AddItems(InventoryItem item, int number)
    {
        return;
    }

    public InventoryItem GetItem()
    {
        return null;
    }

    public int GetNumber()
    {
        return 0;
    }

    public void RemoveItems(int number)
    {
        
    }
}
