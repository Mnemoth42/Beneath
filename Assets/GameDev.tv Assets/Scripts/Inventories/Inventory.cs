using System;
using System.Linq;
using UnityEngine;
using TkrainDesigns.Saving;

namespace GameDevTV.Inventories
{
    /// <summary>
    /// Provides storage for the player inventory. A configurable number of
    /// slots are available.
    ///
    /// This component should be placed on the GameObject tagged "Player".
    /// </summary>
    public class Inventory : MonoBehaviour, ISaveable
    {
        // CONFIG DATA
        [Tooltip("Allowed size")]
        [SerializeField] int inventorySize = 16;

        // STATE
        InventorySlot[] slots;

        struct InventorySlot
        {
            public InventoryItem item;
            public int number;
        }

        int coins = 0;

        // PUBLIC

        /// <summary>
        /// Broadcasts when the items in the slots are added/removed.
        /// </summary>
        public event Action InventoryUpdated;

        /// <summary>
        /// Convenience for getting the player's inventory.
        /// </summary>
        public static Inventory GetPlayerInventory()
        {
            var player = GameObject.FindWithTag("Player");
            return player.GetComponent<Inventory>();
        }

        /// <summary>
        /// Could this item fit anywhere in the inventory?
        /// </summary>
        public bool HasSpaceFor(InventoryItem item)
        {
            return FindSlot(item) >= 0;
        }

        /// <summary>
        /// How many slots are in the inventory?
        /// </summary>
        public int GetSize()
        {
            return slots.Length;
        }

        /// <summary>
        /// Attempt to add the items to the first available slot.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="number">The number to add.</param>
        /// <returns>Whether or not the item could be added.</returns>
        public bool AddToFirstEmptySlot(InventoryItem item, int number)
        {
            int i = FindSlot(item);

            if (i < 0)
            {
                return false;
            }
            if (!item.IsStackable() && slots[i].item != null) return false;
            slots[i].item = item;
            slots[i].number += 1;
            if (!item.IsStackable()) slots[i].number = 1;
            InventoryUpdated?.Invoke();
            return true;
        }

        /// <summary>
        /// Is there an instance of the item in the inventory?
        /// </summary>
        public bool HasItem(InventoryItem item)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (object.ReferenceEquals(slots[i].item, item))
                {
                    return true;
                }
            }
            return false;
        }

        public void RemoveItem(InventoryItem item)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (object.ReferenceEquals(slots[i].item, item))
                {
                    slots[i].item = null;
                    slots[i].number = 0;
                }
            }
            InventoryUpdated?.Invoke();
        }

        /// <summary>
        /// Return the item type in the given slot.
        /// </summary>
        public InventoryItem GetItemInSlot(int slot)
        {
            return slots[slot].item;
        }

        /// <summary>
        /// Get the number of items in the given slot.
        /// </summary>
        public int GetNumberInSlot(int slot)
        {
            return slots[slot].number;
        }

        /// <summary>
        /// Remove a number of items from the given slot. Will never remove more
        /// that there are.
        /// </summary>
        public void RemoveFromSlot(int slot, int number)
        {
            slots[slot].number -= number;
            if (slots[slot].number <= 0)
            {
                slots[slot].number = 0;
                slots[slot].item = null;
            }

            InventoryUpdated?.Invoke();
        }

        /// <summary>
        /// Will add an item to the given slot if possible. If there is already
        /// a stack of this type, it will add to the existing stack. Otherwise,
        /// it will be added to the first empty slot.
        /// </summary>
        /// <param name="slot">The slot to attempt to add to.</param>
        /// <param name="item">The item type to add.</param>
        /// <param name="number">The number of items to add.</param>
        /// <returns>True if the item was added anywhere in the inventory.</returns>
        public bool AddItemToSlot(int slot, InventoryItem item, int number)
        {
            if (item == null) return false;
            if (slots[slot].item != null)
            {
                return AddToFirstEmptySlot(item, number); ;
            }

            var i = FindStack(item);
            if (i >= 0)
            {
                slot = i;
            }

            slots[slot].item = item;
            slots[slot].number += number;
            InventoryUpdated?.Invoke();
            return true;
        }

        // PRIVATE

        private void Awake()
        {
            slots = new InventorySlot[inventorySize];
        }

        /// <summary>
        /// Find a slot that can accomodate the given item.
        /// </summary>
        /// <returns>-1 if no slot is found.</returns>
        private int FindSlot(InventoryItem item)
        {
            int i = FindStack(item);
            if (i < 0)
            {
                i = FindEmptySlot();
            }
            return i;
        }

        /// <summary>
        /// Find an empty slot.
        /// </summary>
        /// <returns>-1 if all slots are full.</returns>
        private int FindEmptySlot()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Find an existing stack of this item type.
        /// </summary>
        /// <returns>-1 if no stack exists or if the item is not stackable.</returns>
        private int FindStack(InventoryItem item)
        {
            if (!item.IsStackable())
            {
                return -1;
            }

            for (int i = 0; i < slots.Length; i++)
            {
                if (object.ReferenceEquals(slots[i].item, item))
                {
                    return i;
                }
            }
            return -1;
        }

        [System.Serializable]
        private struct InventorySlotRecord
        {
            public string itemID;
            public int number;
            public int level;
            public SaveBundle bundle;
        }
        

        public int AddCoins(int coinsToAdd)
        {
            coins += coinsToAdd;
            return coins;
        }

        public int RemoveCoins(int coinsToLose)
        {
            coins -= coinsToLose;
            coins = Mathf.Max(coinsToLose, 0);
            return coins;
        }

        public void SortInventory()
        {
            var sortables = slots.Where(i => i.item != null).OrderBy(j => j.item.SortOrder())
                                 .ThenByDescending(k => k.item.Level).ToList();
            slots = new InventorySlot[inventorySize];
            for (int i = 0; i < sortables.Count; i++)
            {
                slots[i] = sortables[i];
            }
            InventoryUpdated?.Invoke();
        }

        public int Coins { get => coins; protected set => coins = Mathf.Min(0,value); }

        public void RestoreState(SaveBundle bundle)
        {
            if (bundle == null) return;
            Coins = bundle.GetInt("Coins");
            object state = bundle.GetObject("Inventory");
            var slotStrings = (InventorySlotRecord[])state;
            for (int i = 0; i < slotStrings.Length; i++)
            {

                var item = InventoryItem.GetFromId(slotStrings[i].itemID);
                if (item)
                {
                    if (!item.IsStackable())
                    {
                        var newItem = Instantiate(item);
                        item = newItem;
                        item.Decorator.RestoreState(slotStrings[i].bundle);

                    }

                    item.Level = slotStrings[i].level;
                    slots[i].item = item;
                    slots[i].number = slotStrings[i].number; 
                }
            }

            InventoryUpdated?.Invoke();
        }

        public SaveBundle CaptureState()
        {
            var slotStrings = new InventorySlotRecord[inventorySize];
            for (int i = 0; i < inventorySize; i++)
            {
                if (slots[i].item != null)
                {
                    slotStrings[i].itemID = slots[i].item.GetItemID();
                    slotStrings[i].number = slots[i].number;
                    slotStrings[i].level = slots[i].item.Level;
                    slotStrings[i].bundle = slots[i].item.Decorator.CaptureState();
                }
            }
            SaveBundle bundle = new SaveBundle();
            bundle.PutObject("Inventory", slotStrings);
            bundle.PutInt("Coins", coins);
            return bundle;
        }

        
    }
}