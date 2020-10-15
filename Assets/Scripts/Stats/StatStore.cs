using System.Collections;
using System.Collections.Generic;
using TkrainDesigns.ResourceRetriever;
using TkrainDesigns.Saving;
using TkrainDesigns.ScriptableEnums;
using TkrainDesigns.Stats;
using TkrainDesigns.Tiles.Stats;
using UnityEngine;

public class StatStore : MonoBehaviour, IModifierProvider, ISaveable
{

    public event System.Action onStatStoreUpdated;

    Dictionary<ScriptableStat, int> storage = new Dictionary<ScriptableStat, int>();


    public void IncreaseStatModifier(ScriptableStat stat)
    {
        if (IncreasesToSpend() < 1) return;
        if (!storage.ContainsKey(stat))
        {
            storage.Add(stat, 1);
        }
        else
        {
            storage[stat] += 1;
        }
        onStatStoreUpdated?.Invoke();
    }

    public void DecreaseStatModifier(ScriptableStat stat)
    {
        if (storage.ContainsKey(stat))
        {
            storage[stat] = Mathf.Max(storage[stat] - 1, 0);
        }
        onStatStoreUpdated?.Invoke();
    }

    public bool HasPositiveModifier(ScriptableStat stat)
    {
        return storage.ContainsKey(stat) && storage[stat] > 0;
    }

    public int GetPositiveModifier(ScriptableStat stat)
    {
        return storage.ContainsKey(stat) ? storage[stat] : 0;
    }

    public int TotalStatIncreases()
    {
        return (GetComponent<PersonalStats>().Level) * 2;
    }

    public int IncreasesToSpend()
    {
        int result = TotalStatIncreases();
        foreach (var pair in storage)
        {
            result -= pair.Value;
        }
        return result;
    }

    public IEnumerable<float> GetAdditiveModifier(ScriptableStat stat)
    {
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
