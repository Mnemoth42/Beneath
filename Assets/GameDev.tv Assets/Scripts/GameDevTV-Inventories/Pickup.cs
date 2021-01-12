using TkrainDesigns.Saving;
using UnityEngine;

namespace GameDevTV.Inventories
{
    /// <summary>
    ///     To be placed at the root of a Pickup prefab. Contains the data about the
    ///     pickup such as the type of item and the number.
    /// </summary>
    public class Pickup : MonoBehaviour, ISaveable
    {
        // CACHED REFERENCE
        Inventory inventory;

        // STATE
        InventoryItem item;
        int number = 1;
        GameObject player;
        bool useOnPickup;


        //These exist to allow Any of the inventory saves to save associated data that's instantiated on a child of pickup... for example:  Runtime generated stats
        //If called on a plain pickup or not overridden, this interface does nothing.

        public virtual SaveBundle CaptureState() => new SaveBundle();

        public virtual void RestoreState(SaveBundle bundle)
        {
        }

        // LIFECYCLE METHODS

        void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            inventory = player.GetComponent<Inventory>();
        }

        // PUBLIC

        /// <summary>
        ///     Set the vital data after creating the prefab.
        /// </summary>
        /// <param name="itemToAdd">The type of item this prefab represents.</param>
        /// <param name="numberToAdd">The number of items represented.</param>
        public void Setup(InventoryItem itemToAdd, int numberToAdd)
        {
            if (itemToAdd == null)
            {
                return;
            }

            this.item = itemToAdd;
            if (!itemToAdd.IsStackable())
            {
                numberToAdd = 1;
            }

            this.number = numberToAdd;

            useOnPickup = itemToAdd.CanUseImmediate(player);
        }

        public InventoryItem GetItem()
        {
            return item;
        }

        public int GetNumber()
        {
            return number;
        }

        public void PickupItem()
        {
            if (useOnPickup)
            {
                item.UseImmediate(player, number);
                Destroy(gameObject);
                return;
            }

            bool foundSlot = inventory.AddToFirstEmptySlot(item, number);
            if (foundSlot)
            {
                
                Destroy(gameObject);
            }
        }

        public bool CanBePickedUp()
        {
            if (useOnPickup)
            {
                return item.CanUseImmediate(player);
            }

            return inventory.HasSpaceFor(item);
        }
    }
}