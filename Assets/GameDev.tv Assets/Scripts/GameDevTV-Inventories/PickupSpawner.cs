using UnityEngine;
using TkrainDesigns.Saving;

namespace GameDevTV.Inventories
{
    /// <summary>
    /// Spawns pickups that should exist on first load in a level. This
    /// automatically spawns the correct prefab for a given inventory item.
    /// </summary>
    public class PickupSpawner : MonoBehaviour, ISaveable
    {
        // CONFIG DATA
        [SerializeField] InventoryItem item = null;
        [SerializeField] int number = 1;
        [SerializeField] int level = 5;
        [SerializeField] bool spawnOnAwake = true;

        bool hasBeenSpawned = false;
        // LIFECYCLE METHODS
        public virtual void Awake()
        {
            // Spawn in Awake so can be destroyed by save system after.
            if(spawnOnAwake)
            SpawnPickup();
        }

        // PUBLIC

        /// <summary>
        /// Returns the pickup spawned by this class if it exists.
        /// </summary>
        /// <returns>Returns null if the pickup has been collected.</returns>
        public Pickup GetPickup() 
        { 
            return GetComponentInChildren<Pickup>();
        }

        /// <summary>
        /// True if the pickup was collected.
        /// </summary>
        public bool IsCollected() 
        { 
            return GetPickup() == null;
        }

        //PRIVATE

        public void SpawnPickup()
        {
            var spawnedPickup = item.SpawnPickup(transform.position, number, level);
            spawnedPickup.transform.SetParent(transform);
            hasBeenSpawned = true;
        }

        private void DestroyPickup()
        {
            if (GetPickup())
            {
                Destroy(GetPickup().gameObject);
            }
        }

        public SaveBundle CaptureState()
        {
            SaveBundle bundle = new SaveBundle();
            bundle.PutBool("IsCollected", IsCollected());
            bundle.PutBool("hasBeenSpawned", hasBeenSpawned);
            return bundle;
        }

        public void RestoreState(SaveBundle bundle)
        {
            if (bundle == null)
            {
                return;
            }

            hasBeenSpawned = bundle.GetBool("hasBeenSpawned");
            bool shouldBeCollected = bundle.GetBool("IsCollected");
            if (hasBeenSpawned && !spawnOnAwake)
            {
                SpawnPickup();
            }
            if (shouldBeCollected && !IsCollected())
            {
                DestroyPickup();
            }

            if (!shouldBeCollected && IsCollected())
            {
                SpawnPickup();
            }
        }
    }
}