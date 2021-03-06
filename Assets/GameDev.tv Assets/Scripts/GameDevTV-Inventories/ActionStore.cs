﻿using System;
using System.Collections.Generic;
using TkrainDesigns.Saving;
using UnityEngine;
using UnityEngine.Events;

#pragma warning disable CS0649
namespace GameDevTV.Inventories
{
    /// <summary>
    /// Provides the storage for an action bar. The bar has a finite number of
    /// slots that can be filled and actions in the slots can be "used".
    /// 
    /// This component should be placed on the GameObject tagged "Player".
    /// </summary>
    public class ActionStore : MonoBehaviour, ISaveable
    {
        // STATE
        Dictionary<int, DockedItemSlot> dockedItems = new Dictionary<int, DockedItemSlot>();


        private class DockedItemSlot
        {
            public ActionItem item;
            public int number;
        }

        [SerializeField] List<ActionItem> defaultItems = new List<ActionItem>();

        void Awake()
        {
            for (int i = 0; i < defaultItems.Count; i++)
            {
                if (defaultItems[i] != null)
                {
                    ActionItem item = Instantiate(defaultItems[i]) as ActionItem;
                    AddAction(item, i, 1);
                }

            }
        }

        // PUBLIC

        /// <summary>
        /// Broadcasts when the items in the slots are added/removed.
        /// </summary>
        public event Action StoreUpdated;

        public UnityEvent<Vector3, string> StoreUpdatedAdvertiser;

        public event Action OnBeginTurn;
        public event Action OnEndTurn;

        public void AnnounceUpdate()
        {
            StoreUpdated?.Invoke();
        }

        public void BeginTurn()
        {
            //Debug.Log($"BeginTurn");
            OnBeginTurn?.Invoke();
        }

        public void EndTurn()
        {
            //Debug.Log("EndTurn");
            OnEndTurn?.Invoke();
        }

        /// <summary>
        /// Get the action at the given index.
        /// </summary>
        public ActionItem GetAction(int index)
        {
            if (dockedItems.ContainsKey(index))
            {
                return dockedItems[index].item;
            }
            return null;
        }

        /// <summary>
        /// Get the number of items left at the given index.
        /// </summary>
        /// <returns>
        /// Will return 0 if no item is in the index or the item has
        /// been fully consumed.
        /// </returns>
        public int GetNumber(int index)
        {
            if (dockedItems.ContainsKey(index))
            {
                return dockedItems[index].number;
            }
            return 0;
        }

        /// <summary>
        /// Add an item to the given index.
        /// </summary>
        /// <param name="item">What item should be added.</param>
        /// <param name="index">Where should the item be added.</param>
        /// <param name="number">How many items to add.</param>
        public void AddAction(InventoryItem item, int index, int number)
        {
            if (dockedItems.ContainsKey(index))
            {
                if (object.ReferenceEquals(item, dockedItems[index].item) && item.IsStackable())
                {
                    dockedItems[index].number += number;
                    StoreUpdated?.Invoke();
                    return;
                }
            }
            {
                var slot = new DockedItemSlot { item = item as ActionItem, number = number };
                dockedItems[index] = slot;
            }
            StoreUpdatedAdvertiser?.Invoke(transform.position, $"Learned {item.GetDisplayName()}");
            StoreUpdated?.Invoke();
        }

        /// <summary>
        /// Use the item at the given slot. If the item is consumable one
        /// instance will be destroyed until the item is removed completely.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="user">The character that wants to use this action.</param>
        /// <returns>False if the action could not be executed.</returns>
        public bool Use(int index, GameObject user)
        {
            if (dockedItems.ContainsKey(index))
            {
                if (dockedItems[index].item.Use(user))
                {
                    if (dockedItems[index].item.IsConsumable())
                    {
                        RemoveItems(index, 1);
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Remove a given number of items from the given slot.
        /// </summary>
        public void RemoveItems(int index, int number)
        {
            if (dockedItems.ContainsKey(index))
            {
                dockedItems[index].number -= number;
                if (dockedItems[index].number <= 0)
                {
                    dockedItems.Remove(index);
                }
                StoreUpdated?.Invoke();
            }

        }

        /// <summary>
        /// What is the maximum number of items allowed in this slot.
        /// 
        /// This takes into account whether the slot already contains an item
        /// and whether it is the same type. Will only accept multiple if the
        /// item is consumable.
        /// </summary>
        /// <returns>Will return int.MaxValue when there is not effective bound.</returns>
        public virtual int  MaxAcceptable(InventoryItem item, int index)
        {
            var actionItem = item as ActionItem;
            if (actionItem == null)
            {
                return 0;
            }

            int existingSlot = FindItem(item);
            if (existingSlot >= 0 && existingSlot != index) return 0; //No duplicate actions, so there.
            if (actionItem.IsConsumable())
            {
                if (existingSlot < 0) return 5; //int.maxvalue;
                return 5 - dockedItems[index].number;
            }
            if (dockedItems.ContainsKey(index) && !object.ReferenceEquals(item, dockedItems[index].item))
            {
                return 0;
            }

            if (dockedItems.ContainsKey(index))
            {
                return 0;
            }

            return 1;
        }

        public int FindItem(InventoryItem item)
        {
            foreach (var pair in dockedItems)
            {
                if (pair.Value.item == item) return pair.Key;
            }

            return -1;
        }

        /// PRIVATE

        [System.Serializable]
        private struct DockedItemRecord
        {
            public string itemID;
            public int number;
        }

        public SaveBundle CaptureState()
        {
            var state = new Dictionary<int, DockedItemRecord>();
            foreach (var pair in dockedItems)
            {
                DockedItemRecord record = new DockedItemRecord { itemID = pair.Value.item.GetItemID(), number = pair.Value.number };
                state[pair.Key] = record;
            }
            SaveBundle bundle = new SaveBundle();
            bundle.PutObject("ActionStore", state);
            return bundle;
        }

        public void RestoreState(SaveBundle bundle)
        {
            if (bundle != null)
            {
                object state = bundle.GetObject("ActionStore");
                Dictionary<int, DockedItemRecord> stateDict = (Dictionary<int, DockedItemRecord>)state;
                foreach (KeyValuePair<int, DockedItemRecord> pair in stateDict)
                {
                    AddAction(InventoryItem.GetFromId(pair.Value.itemID), pair.Key, pair.Value.number);
                }
            }
        }
    }
}