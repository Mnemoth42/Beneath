using System.Collections.Generic;
using UnityEngine;
using TkrainDesigns.Saving;

namespace GameDevTV.Inventories
{
    /// <summary>
    /// To be placed on anything that wishes to drop pickups into the world.
    /// Tracks the drops for saving and restoring.
    /// </summary>
    public class ItemDropper : MonoBehaviour, ISaveable
    {
        // STATE
        public List<Pickup> droppedItems = new List<Pickup>();


        // PUBLIC

        /// <summary>
        /// Create a pickup at the current position.
        /// </summary>
        /// <param name="item">The item type for the pickup.</param>
        /// <param name="number">
        /// The number of items contained in the pickup. Only used if the item
        /// is stackable.
        /// </param>
        public void DropItem(InventoryItem item, int number, int level = 1)
        {
            SpawnPickup(item, GetDropLocation(), number, level);
        }

        /// <summary>
        /// Create a pickup at the current position.
        /// </summary>
        /// <param name="item">The item type for the pickup.</param>
        public void DropItem(InventoryItem item)
        {
            SpawnPickup(item, GetDropLocation(), 1);
        }

        // PROTECTED

        /// <summary>
        /// Override to set a custom method for locating a drop.
        /// </summary>
        /// <returns>The location the drop should be spawned.</returns>
        protected virtual Vector3 GetDropLocation()
        {
            return transform.position;
        }

        // PRIVATE

        public void SpawnPickup(InventoryItem item, Vector3 spawnLocation, int number, int level = 1)
        {
            if (item == null) return;
            var pickup = item.SpawnPickup(spawnLocation, number, level);
            droppedItems.Add(pickup);
        }

        [System.Serializable]
        private struct DropRecord
        {
            public string itemID;
            public SerializableVector3 position;
            public int number;
            public SaveBundle bundle;
        }

        public SaveBundle CaptureState()
        {
            RemoveDestroyedDrops();
            var droppedItemsList = new DropRecord[droppedItems.Count];
            for (int i = 0; i < droppedItems.Count; i++)
            {
                
                droppedItemsList[i].itemID = droppedItems[i].GetItem().GetItemID();
                droppedItemsList[i].position = new SerializableVector3(droppedItems[i].transform.position);
                droppedItemsList[i].number = droppedItems[i].GetNumber();
                droppedItemsList[i].bundle = droppedItems[i].GetItem().Decorator.CaptureState();
            }
            SaveBundle bundle = new SaveBundle();
            bundle.PutObject("ItemDropper", droppedItemsList);
            return bundle;
        }

        public void RestoreState(SaveBundle bundle)
        {
            DestroyOldDrops();
            if (bundle == null) return;
            object state = bundle.GetObject("ItemDropper");
            var droppedItemsList = (DropRecord[])state;
            foreach (var item in droppedItemsList)
            {
                var pickupItem = InventoryItem.GetFromId(item.itemID);
                if (!pickupItem.IsStackable())
                {
                    var statHoldingItem = Instantiate(pickupItem);
                    pickupItem = statHoldingItem;
                    pickupItem.Decorator.RestoreState(item.bundle);
                }
                
                Vector3 position = item.position.ToVector();
                int number = item.number;
                SpawnPickup(pickupItem, position, number);
            }
        }

        /// <summary>
        /// Remove any drops in the world that have subsequently been picked up.
        /// </summary>
        private void RemoveDestroyedDrops()
        {
            var newList = new List<Pickup>();
            foreach (var item in droppedItems)
            {
                if (item != null)
                {
                    newList.Add(item);
                }
            }
            droppedItems = newList;
        }

        private void DestroyOldDrops()
        {
            foreach (var item in droppedItems)
            {
                if (item != null)
                {
                    Destroy(item);
                }
            }
            droppedItems.Clear();

        }


    }
}