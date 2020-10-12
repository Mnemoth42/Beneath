using TkrainDesigns.Saving;
using System.Collections.Generic;
using TkrainDesigns.ResourceRetriever;
using TkrainDesigns.ScriptableEnums;
using UnityEngine;
#pragma warning disable CS0649
namespace TkrainDesigns.Stats
{

    public class RandomStatDecorator :  ISaveable
    {
        
        Dictionary<ScriptableStat, float> additiveModifiers= new Dictionary<ScriptableStat, float>();   
        Dictionary<ScriptableStat, float> percentageModifiers=new Dictionary<ScriptableStat, float>();
        bool isAlreadyInitialized = false;

        public bool IsAlreadyInitialized { get => isAlreadyInitialized; set => isAlreadyInitialized = value; }
        public Dictionary<ScriptableStat, float> AdditiveModifiers { get => additiveModifiers;  }
        public Dictionary<ScriptableStat, float> PercentageModifiers { get => percentageModifiers;  }

        public RandomStatDecorator(int level, ScriptableStat[] potentialStatBoosts)
        {
            if(potentialStatBoosts.Length==0)
            {
                //Debug.Log("No potential Stat Boosts");
                isAlreadyInitialized = true;
                return;
            }
            for (int i = 0; i < level; i++)
            {
                if (Random.Range(0, 1) < level)
                {
                    CreateRandomStatDecorator(level, potentialStatBoosts);
                }
            }
            IsAlreadyInitialized = true;
        }

        public RandomStatDecorator()
        {

        }

        private void CreateRandomStatDecorator(int level, ScriptableStat[] potentialStatBoosts)
        {
            ScriptableStat stat = potentialStatBoosts[Random.Range(0, potentialStatBoosts.Length)];
            int modifierAmount = Random.Range(-level / 4, level);
            if (modifierAmount == 0)
            {
                modifierAmount = 1;
            }

            if (additiveModifiers.ContainsKey(stat))
            {
                if (!percentageModifiers.ContainsKey(stat))
                {
                    percentageModifiers[stat] = modifierAmount;
                }
                return;
            }
            additiveModifiers[stat] = modifierAmount;
        }

        public float GetAdditiveModifier(ScriptableStat stat)
        {
            if (additiveModifiers.ContainsKey(stat))
            {
                return additiveModifiers[stat];
            }

            return 0;
        }

        public float GetPercentageModifier(ScriptableStat stat)
        {
            if (percentageModifiers.ContainsKey(stat))
            {
                return percentageModifiers[stat];
            }

            return 0;
        }

        
        public SaveBundle CaptureState()
        {
            Dictionary<string, float> additiveSaveRecord = new Dictionary<string, float>();
            Dictionary<string, float> percentageSaveRecord = new Dictionary<string, float>();
            SaveBundle bundle = new SaveBundle();
            foreach(ScriptableStat stat in additiveModifiers.Keys)
            {
                additiveSaveRecord[stat.GetItemID()] = additiveModifiers[stat];
            }
            foreach(ScriptableStat stat in percentageModifiers.Keys)
            {
                percentageSaveRecord[stat.GetItemID()] = percentageModifiers[stat];
            }
            bundle.PutObject("Additive", additiveSaveRecord);
            bundle.PutObject("Percentage", percentageSaveRecord);
            return bundle;
        }

        public void RestoreState(SaveBundle bundle)
        {
            IsAlreadyInitialized = true;
            Dictionary<string, float> additiveSaveRecord = (Dictionary<string, float>)bundle.GetObject("Additive");
            Dictionary<string, float> percentageSaveRecord = (Dictionary<string, float>)bundle.GetObject("Percentage");
            if (additiveSaveRecord != null)
            {
                additiveModifiers = new Dictionary<ScriptableStat, float>();
                foreach(string key in additiveSaveRecord.Keys)
                {
                    ScriptableStat stat = ResourceRetriever<ScriptableStat>.GetFromID(key);
                    if (stat)
                    {
                        additiveModifiers[stat] = additiveSaveRecord[key];
                    }
                }
            }
            if (percentageSaveRecord != null)
            {
                percentageModifiers = new Dictionary<ScriptableStat, float>();
                foreach (string key in percentageSaveRecord.Keys)
                {
                    ScriptableStat stat = ResourceRetriever<ScriptableStat>.GetFromID(key);
                    if (stat)
                    {
                        percentageModifiers[stat] = percentageSaveRecord[key];
                    }
                }
            }
        }
    }
}