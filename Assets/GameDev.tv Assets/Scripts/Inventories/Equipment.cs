using System;
using System.Collections.Generic;
using UnityEngine;
using TkrainDesigns.Saving;
using TkrainDesigns.ScriptableEnums;
using TkrainDesigns.ResourceRetriever;

namespace GameDevTV.Inventories
{
    /// <summary>
    /// Provides a store for the items equipped to a player. Items are stored by
    /// their equip locations.
    /// 
    /// This component should be placed on the GameObject tagged "Player".
    /// </summary>
    public class Equipment : MonoBehaviour, ISaveable
    {
        // STATE
        
        Dictionary<ScriptableEquipSlot, EquipableItem> equippedItems = new Dictionary<ScriptableEquipSlot, EquipableItem>();
        // PUBLIC

        /// <summary>
        /// Broadcasts when the items in the slots are added/removed.
        /// </summary>
        public event Action EquipmentUpdated;

        /// <summary>
        /// Return the item in the given equip location.
        /// </summary>
        public EquipableItem GetItemInSlot(ScriptableEquipSlot equipLocation)
        {
            if (!equippedItems.ContainsKey(equipLocation))
            {
                return null;
            }

            return equippedItems[equipLocation];
        }

        /// <summary>
        /// Add an item to the given equip location. Do not attempt to equip to
        /// an incompatible slot.
        /// </summary>
        public void AddItem(ScriptableEquipSlot slot , EquipableItem item)
        {
            if (item == null) return;
            Debug.Assert(item.GetAllowedEquipLocation() == slot);

            equippedItems[slot] = item;

            if (EquipmentUpdated != null)
            {
                EquipmentUpdated();
            }
        }

        /// <summary>
        /// Remove the item for the given slot.
        /// </summary>
        public void RemoveItem(ScriptableEquipSlot slot)
        {
            equippedItems.Remove(slot);
            if (EquipmentUpdated != null)
            {
                EquipmentUpdated();
            }
        }

        // PRIVATE

        public IEnumerable<ScriptableEquipSlot> GetAllPopulatedSlots()
        {

            return equippedItems.Keys;
        }

        [System.Serializable]
        struct EquipmentBundle
        {
            public string itemID;
            public SaveBundle bundle;
        }

        public SaveBundle CaptureState()
        {
            var equippedItemsForSerialization = new Dictionary<string, EquipmentBundle>();
            foreach (var pair in equippedItems)
            {
                EquipmentBundle equipmentBundle;
                equipmentBundle.itemID = pair.Value.GetItemID();
                equipmentBundle.bundle = pair.Value.Decorator.CaptureState();
                equippedItemsForSerialization[pair.Key.GetItemID()] = equipmentBundle;
            }
            SaveBundle bundle = new SaveBundle();
            bundle.PutObject("Equipment", equippedItemsForSerialization);
            return bundle;
        }

         public void RestoreState(SaveBundle bundle)
        {
            if (bundle == null) return;
            object state = bundle.GetObject("Equipment");
            equippedItems = new Dictionary<ScriptableEquipSlot, EquipableItem>();

            var equippedItemsForSerialization = (Dictionary<string, EquipmentBundle>)state;

            foreach (var pair in equippedItemsForSerialization)
            {
                ScriptableEquipSlot slot = ResourceRetriever<ScriptableEquipSlot>.GetFromID(pair.Key);
                if (slot == null)
                {
                    continue;
                }
                var item = (EquipableItem)InventoryItem.GetFromId(pair.Value.itemID);
                if (item != null)
                {
                    var instance = Instantiate(item);
                    instance.Decorator.RestoreState(pair.Value.bundle);
                    equippedItems[slot] = instance;
                }
            }
            EquipmentUpdated?.Invoke();
        }
    }
}