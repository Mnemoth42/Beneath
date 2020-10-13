using System.Collections.Generic;
using TkrainDesigns.ResourceRetriever;
using UnityEditor;

using UnityEngine;
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
        [SerializeField] string description="";
        [SerializeField] string extendedDescription="";
        [SerializeField] float minimum = 1.0f;
        [SerializeField] float maximum = 1000f;
        public List<StatSource> sources = new List<StatSource>();

        public string Description => description;
        public string ExtendedDescripton => extendedDescription;

        public float Minumum => minimum;
        public float Maximum => maximum;

        public List<StatSource> GetSources()
        {
            return sources;
        }

#if UNITY_EDITOR

        public void SetDescription(string newDescription)
        {
            if (description.Equals(newDescription)) return;
            Undo.RecordObject(this, "Change Description");
            description = newDescription;
            EditorUtility.SetDirty(this);
        }

        public void SetExtendedDescription(string newDescription)
        {
            if (extendedDescription.Equals(newDescription)) return;
            Undo.RecordObject(this, "Change Extended Description");
            extendedDescription = newDescription;
            EditorUtility.SetDirty(this);
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
    }



}
