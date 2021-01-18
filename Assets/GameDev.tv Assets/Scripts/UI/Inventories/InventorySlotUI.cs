using GameDevTV.Core.UI.Dragging;
using GameDevTV.Inventories;
using TkrainDesigns.Core.Interfaces;
using TkrainDesigns.Stats;
using UnityEngine;

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
        PersonalStats stats;
        Equipment equipment;

        // PUBLIC

        public void Setup(Inventory inventoryToSet, int indexToSet)
        {
            inventory = inventoryToSet;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            stats = player.GetComponent<PersonalStats>();
            equipment = player.GetComponent<Equipment>();
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

        public void AttemptUse()
        {
            if (inventory.GetItemInSlot(index) == null) return;
            if (inventory.GetItemInSlot(index).Level > stats.Level) return;
            if (inventory.GetItemInSlot(index) is EquipableItem equipable)
            {
                EquipableItem swapItem = equipment.GetItemInSlot(equipable.GetAllowedEquipLocation());
                equipment.AddItem(equipable.GetAllowedEquipLocation(), equipable);
                inventory.RemoveFromSlot(index,1);
                if(swapItem)
                {
                    inventory.AddItemToSlot(index,swapItem,1);
                }
            }

            //if (inventory.GetItemInSlot(index) is ActionItem actionItem)
            //{
            //    if (!actionItem.CanUse(inventory.gameObject)) return;
            //    actionItem.Use(inventory.gameObject);
            //    if(actionItem.IsConsumable())
            //    {
            //        inventory.RemoveFromSlot(index, 1);
            //    }
            //}
        }

        float lastClicked = 0.0f;

        void Update()
        {
            lastClicked -= Time.deltaTime;
        }

        public void HandleClick()
        {
            inventory.GetComponent<ICancelClicks>().CancelClicks();
            if (lastClicked<0.0f)
            {
                lastClicked =.5f;
                return;
            }
            AttemptUse();
            lastClicked = 0;
        }

        public InventoryItem GetTooltipItem()
        {
            return inventory.GetItemInSlot(index);
        }
    }
}