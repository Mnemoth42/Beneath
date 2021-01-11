using System.Collections.Generic;
using TkrainDesigns.ResourceRetriever;
using UnityEditor;

using UnityEngine;
using UnityEngine.Serialization;

#pragma warning disable CS0649
namespace TkrainDesigns.ScriptableEnums
{
    [System.Serializable]
    public class StatSource
    {
        public ScriptableStat stat;
        public float effectPerLevel = 1.0f;

        public StatSource()
        {

        }

        public StatSource(ScriptableStat _stat, float _effectPerLevel)
        {
            stat = _stat;
            effectPerLevel = _effectPerLevel;
        }
    }

    [CreateAssetMenu(fileName ="NewStat", menuName = "TkrainDesigns/ScriptableEnums/New Stat")]
    public class ScriptableStat : RetrievableScriptableObject
    {
        [SerializeField] string displayName="";
        [FormerlySerializedAs("extendedDescription")] [SerializeField] string description="";
        [SerializeField] string alias = "";
        [SerializeField] float minimum = 1.0f;
        [SerializeField] float maximum = 1000f;
        public List<StatSource> sources = new List<StatSource>();

        public string DisplayName => displayName;
        public string Description => description;
        public string Alias => alias;

        public float Minumum => minimum;
        public float Maximum => maximum;

        public List<StatSource> GetSources()
        {
            return sources;
        }

#if UNITY_EDITOR

        public void SetDisplayName(string value)
        {
            SetItem(ref displayName, value, "Change Display Name");
        }

        public void SetDescription(string value)
        {
            SetItem(ref description, value, "Change Description");
        }

        public void SetAlias(string value)
        {
            SetItem(ref alias, value, "Set Alias");
        }

        public void AddStatSource()
        {
            Undo.RecordObject(this, "Add Stat Source");
            sources.Add(new StatSource());
            EditorUtility.SetDirty(this);
        }

        public void RemoveStatSource(int index)
        {
            
                Undo.RecordObject(this,"Remove Stat Source");
                sources.RemoveAt(index);
                EditorUtility.SetDirty(this);
            
        }

        public void SetMinimum(float newMinimum)
        {
            if (minimum.Equals(newMinimum)) return;
            Undo.RecordObject(this, "Change Minimum Value");
            minimum = newMinimum;
            EditorUtility.SetDirty(this);
        }

        public void SetMaximum(float newMaximum)
        {
            if (maximum.Equals(newMaximum)) return;
            Undo.RecordObject(this, "Change Maximum Value");
            maximum = newMaximum;
            EditorUtility.SetDirty(this);
        }

        public void ChangeStatSource(int index, StatSource newStatSource)
        {
            if (sources[index].stat == newStatSource.stat &&
                sources[index].effectPerLevel == newStatSource.effectPerLevel) return;
            Undo.RecordObject(this, "Change Stat Source");
            sources[index] = newStatSource;
            EditorUtility.SetDirty(this);
        }
#endif
        public override string GetDisplayName()
        {
            return displayName;
        }

        public override string GetDescription()
        {
            return description;
        }
    }



}
