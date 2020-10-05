using UnityEngine;
using GameDevTV.Inventories;
using GameDevTV.Core.UI.Dragging;

namespace GameDevTV.UI.Inventories
{
    public class InventorySlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        // CONFIG DATA
        [SerializeField] InventoryItemIcon icon = null;

        // STATE
        int index;
        //InventoryItem item;
        Inventory inventory;

        // PUBLIC

        public void Setup(Inventory inventoryToSet, int indexToSet)
        {
            inventory = inventoryToSet;
            index = indexToSet;
            icon.SetItem(inventoryToSet.GetItemInSlot(indexToSet), inventoryToSet.GetNumberInSlot(indexToSet));
        }

        public int MaxAcceptable(InventoryItem item)
        {
            if (inventory.HasSpaceFor(item))
            {
                return int.MaxValue;
            }
            return 0;
        }

        public void AddItems(InventoryItem item, int number)
        {
            if(number>0)
            {
                inventory.AddItemToSlot(index, item, number);
            }
        }

        public InventoryItem GetItem()
        {
            return inventory.GetItemInSlot(index);
        }

        public int GetNumber()
        {
            return inventory.GetNumberInSlot(index);
        }

        public void RemoveItems(int number)
        {
            inventory.RemoveFromSlot(index, number);
        }
    }
}