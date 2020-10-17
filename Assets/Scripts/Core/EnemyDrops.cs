using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TkrainDesigns.Tiles.Core
{
    [System.Serializable]
    public class EnemyDropEntry
    {
        public GameObject character = null;
        public AnimationCurve probability = new AnimationCurve();
    }

    [CreateAssetMenu(fileName="New EnemyDropLibrary", menuName = "RPG/Enemy Drop Library")]
    public class EnemyDrops : ScriptableObject
    {
        [SerializeField]List<EnemyDropEntry> drops = new List<EnemyDropEntry>();

        public List<int> GetPotentialDrops(int level)
        {
            List<int> result = new List<int>();
            for(int d=0;d<drops.Count;d++)
            {
                EnemyDropEntry drop = drops[d];
                int probability = (int)drop.probability.Evaluate(level);
                if (drop.character == null) continue;
                //Debug.Log($"Adding {probability} {drop.character}");
                for (int i = 1; i < probability; i++)
                {
                    result.Add(d);
                }
            }
            return result;
        }

        public GameObject GetDrop(int level = 1)
        {
            var potentialDrops = GetPotentialDrops(level);
            if (potentialDrops.Count == 0) return null;
            return drops[potentialDrops[Random.Range(0, potentialDrops.Count)]].character;
        }

    }
}