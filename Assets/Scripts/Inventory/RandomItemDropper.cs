using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using TkrainDesigns.Attributes;
using TkrainDesigns.Stats;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.Inventory
{
    public class RandomItemDropper : ItemDropper
    {
        [SerializeField] DropLibrary library;
        CoinDrop coinDropPrefab;

        Health health;
        void Awake()
        {
            Health health = GetComponent<Health>();
            coinDropPrefab = Resources.Load<CoinDrop>("Coin");
        }


        void OnEnable()
        {
            if (health)
            {
                health.onDeath.AddListener(DropRandomItems);
            }
        }

        void OnDisable()
        {
            if (health)
            {
                health.onDeath.RemoveListener(DropRandomItems);
            }
        }

        public void DropRandomItems()
        {
            

            if (!TryGetComponent<PersonalStats>(out PersonalStats stats))
            {
                return;
            }
            
            int level = stats.Level;
            if (coinDropPrefab)
            {
                DropItem(coinDropPrefab, 1, level);
            }
            if (!library)
            {
                return;
            }
            int numberOfDrops = Mathf.Clamp(Random.Range(0, level+1),0,2);
            List<string> dropsEncountered=new List<string>();
            for (int i = 0; i < numberOfDrops; i++)
            {
                Drop drop = library.GetRandomDrop(level);
                if (drop.item != null)
                {
                    if (dropsEncountered.Contains(drop.item.GetItemID())) continue;
                    dropsEncountered.Add(drop.item.GetItemID());
                    DropItem(drop.item, drop.count, level);

                }
            }
        }
    }
}