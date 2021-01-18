using System.Collections.Generic;
using TkrainDesigns.ResourceRetriever;
using UnityEditor;
using UnityEngine;

namespace TkrainDesigns.ScriptableEnums
{
    [CreateAssetMenu(fileName="NewElement", menuName = "TkrainDesigns/ScriptableEnums")]
    public class ScriptableElement : RetrievableScriptableObject
    {
        [SerializeField] string displayName = "";
        [SerializeField] string description = "";
        [SerializeField] List<ScriptableElement> strengths;
        

        public override string GetDescription()
        {
            return description;
        }

        public override string GetDisplayName()
        {
            return displayName;
        }

        public bool IsStrongAgainst(ScriptableElement element)
        {
            return strengths.Contains(element);
        }

#if UNITY_EDITOR

        public void SetDescription(string newDescription)
        {
            if (description.Equals(newDescription)) return;
            Undo.RecordObject(this, "Change Description");
            description = newDescription;
            EditorUtility.SetDirty(this);
        }

        public void SetDisplayname(string newDisplayName)
        {
            if (displayName.Equals(newDisplayName)) return;
            Undo.RecordObject(this, "Change Display Name");
            displayName = newDisplayName;
            EditorUtility.SetDirty(this);

        }

        public void AddStrength()
        {
            Undo.RecordObject(this, "Add New Strength");
            strengths.Add(null);
        }

        public void RemoveStrength(int i)
        {
            string str = "null";
            if (strengths[i] != null) str = strengths[i].displayName;
            Undo.RecordObject(this, $"Remove Strength: {str}");
            strengths.RemoveAt(i);
        }

#endif

    }
}