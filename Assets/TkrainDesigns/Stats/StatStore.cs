using System.Collections;
using System.Collections.Generic;
using TkrainDesigns.ResourceRetriever;
using TkrainDesigns.Saving;
using TkrainDesigns.ScriptableEnums;
using UnityEngine;

namespace TkrainDesigns.Stats
{
    public class StatStore : MonoBehaviour, IModifierProvider, ISaveable
    {

        public event System.Action onStatStoreUpdated;

        Dictionary<ScriptableStat, int> uncommittedStorage = new Dictionary<ScriptableStat, int>();
        Dictionary<ScriptableStat, int> storage = new Dictionary<ScriptableStat, int>();

        public bool Dirty => uncommittedStorage.Count > 0;

        public void IncreaseStatModifier(ScriptableStat stat)
        {
            if (IncreasesToSpend() < 1) return;
            if (!uncommittedStorage.ContainsKey(stat))
            {
                uncommittedStorage.Add(stat, 1);
            }
            else
            {
                uncommittedStorage[stat] += 1;
            }
            onStatStoreUpdated?.Invoke();
        }

        public void DecreaseStatModifier(ScriptableStat stat)
        {
            if (uncommittedStorage.ContainsKey(stat))
            {
                uncommittedStorage[stat] = Mathf.Max(uncommittedStorage[stat] - 1, 0);
            }
            onStatStoreUpdated?.Invoke();
        }

        public bool HasPositiveModifier(ScriptableStat stat)
        {
            return uncommittedStorage.ContainsKey(stat) && uncommittedStorage[stat] > 0;
        }

        public int GetPositiveModifier(ScriptableStat stat)
        {
            int result = 0;
            foreach(var value in GetAdditiveModifier(stat))
            {
                result += (int)value;
            }
            return result;

        }

        public void CommitChanges()
        {
            foreach (var pair in uncommittedStorage)
            {
                if (storage.ContainsKey(pair.Key))
                {
                    storage[pair.Key] += pair.Value;
                }
                else
                {
                    storage[pair.Key] = pair.Value;
                }
            }
            uncommittedStorage.Clear();
            onStatStoreUpdated?.Invoke();
        }

        public void RevertChanges()
        {
            uncommittedStorage.Clear();
            onStatStoreUpdated?.Invoke();
        }

        public int TotalStatIncreases()
        {
            return (GetComponent<PersonalStats>().Level) * 2;
        }

        public int IncreasesToSpend()
        {
            int result = TotalStatIncreases();
            foreach (var pair in uncommittedStorage)
            {
                result -= pair.Value;
            }

            foreach (var pair in storage)
            {
                result -= pair.Value;
            }
            return result;
        }

        public IEnumerable<float> GetAdditiveModifier(ScriptableStat stat)
        {
            yield return !uncommittedStorage.ContainsKey(stat) ? 0.0f : uncommittedStorage[stat];
            yield return !storage.ContainsKey(stat) ? 0.0f : storage[stat];
        }

        public IEnumerable<float> GetPercentageModifier(ScriptableStat stat)
        {
            yield return 0.0f;
        }

        [System.Serializable]
        struct statStoreStruct
        {
            public string statString;
            public int value;

            public statStoreStruct(string stat, int val)
            {
                statString = stat;
                value = val;
            }
        }

        public SaveBundle CaptureState()
        {
            List<statStoreStruct> state = new List<statStoreStruct>();
            foreach (var pair in storage)
            {
                state.Add(new statStoreStruct(pair.Key.GetItemID(), pair.Value));
            }

            SaveBundle result = new SaveBundle();
            result.PutObject("State",state);
            return result;
        }

        public void RestoreState(SaveBundle bundle)
        {
            List<statStoreStruct> state = bundle.GetObject("State",new List<statStoreStruct>()) as List<statStoreStruct>;
            storage=new Dictionary<ScriptableStat, int>();
            foreach (var pair in state)
            {
                ScriptableStat stat = ResourceRetriever<ScriptableStat>.GetFromID(pair.statString);
                if(stat)
                {
                    storage[stat] = pair.value;
                }
            }
            onStatStoreUpdated?.Invoke();
        }
    }
}