using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;

namespace RPG.Inventory
{
    [CreateAssetMenu(fileName="New DropLibrary", menuName="Inventory/DropLibrary")]
    public class DropLibrary : ScriptableObject
    {
    

        [SerializeField] List<DropEntry> entries = new List<DropEntry>();

        public Drop GetRandomDrop(int level)
        {
            List<Drop> result = GetPossibleEntries(level);
            if (result.Count == 0) return null;
            return result[Random.Range(0, result.Count)];
        }

        public List<Drop> GetPossibleEntries(int level)
        {
            List<Drop> result = new List<Drop>();
            foreach (DropEntry entry in entries)
            {
                int chance = (int)entry.chance.Evaluate((float) level / 100.0f);
                int amount = (int)entry.amount.Evaluate((float) level / 100.0f);
                if (amount <= 0) continue;
                for (int i = 0; i < chance; i++)
                {
                    result.Add(new Drop(entry.item, Random.Range(0,amount)+1));
                }
            }

            return result;
        }
    }

    [System.Serializable]
    public class DropEntry
    {
        public InventoryItem item = null;
        public AnimationCurve chance = new AnimationCurve();
        public AnimationCurve amount = new AnimationCurve();
    }

    [System.Serializable]
    public class Drop
    {
        public InventoryItem item = null;
        public int count = 1;

        public Drop()
        {

        }

        public Drop(InventoryItem newItem, int newCount)
        {
            item = newItem;
            count = newCount;
        }
    }
}