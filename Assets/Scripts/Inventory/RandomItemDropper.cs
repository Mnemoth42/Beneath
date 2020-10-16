using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using TkrainDesigns.Grids.Stats;
using TkrainDesigns.Stats;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.Inventory
{
    public class RandomItemDropper : ItemDropper
    {
        [SerializeField] DropLibrary library;
        Health health;
        void Awake()
        {
            Health health = GetComponent<Health>();
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
            if (!library)
            {
                Debug.LogError($"{name} does not have a DropLibrary attached.");
            }

            if (!TryGetComponent<PersonalStats>(out PersonalStats stats))
            {
                Debug.Log($"{name} does not have a personalStats attached.");
            }
            int level = stats.Level;
            int numberOfDrops = Random.Range(0, level+1);
            //Debug.Log($"{name} is dropping {numberOfDrops} items.");

            for (int i = 0; i < numberOfDrops; i++)
            {
                Drop drop = library.GetRandomDrop(level);
                if (drop.item != null)
                {
                    //Debug.Log($"{name} has dropped {drop.item}");
                    DropItem(drop.item, drop.count);
                }
            }
        }
    }
}