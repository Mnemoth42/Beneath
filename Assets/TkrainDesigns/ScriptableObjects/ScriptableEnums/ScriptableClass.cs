using System.Collections.Generic;
using TkrainDesigns.ResourceRetriever;
using UnityEditor;
using UnityEngine;
#pragma warning disable CS0649
namespace TkrainDesigns.ScriptableEnums
{
    [CreateAssetMenu(fileName = "newClass", menuName = "TkrainDesigns/ScriptableEnums/New Class")]
    public class ScriptableClass : RetrievableScriptableObject
    {
        #region Fields

        [SerializeField] string displayName = "";
        [SerializeField] string description="";
        [Header("Put Stats here.  Set size to the number of stats", order =0), Space(-15,order =2), Header("Select a ScriptableStat, a Level 1 value (S), and amount added per level (A).",order =3)]
        //[Header("Select a ScriptableStat, a Level 1 value (S), and amount added per level (A).")]
        [SerializeField] List<StatFormula> formula;

        #endregion
        #region Properies
        public string Description { get => description;
            set => description=value;
        }

        public string GetDisplayName()
        {
            return displayName;
        }

        public List<StatFormula> Formula { get => formula;  }
        #endregion

        #region Global Variables
        Dictionary<ScriptableStat, StatFormula> statFormulas;

        #endregion

# region Setup
        public void InitDictionary()
        {
            if (statFormulas == null)
            {
                statFormulas = new Dictionary<ScriptableStat, StatFormula>();
                foreach(StatFormula frm in formula)
                {
                    statFormulas.Add(frm.Stat, frm);
                }
            }
        }
# endregion

        public float GetStat(ScriptableStat stat, int level=1, float fallback = 1.0f)
        {
            InitDictionary();
            if (statFormulas.ContainsKey(stat))
            {
                return statFormulas[stat].Calculate(level);
            }
            return fallback;
        }

        public void AddFormula(ScriptableStat newStat)
        {
            StatFormula frm = new StatFormula();
            frm.Stat = newStat;
            formula.Add(frm);
        }

#if UNITY_EDITOR
        public void SetDisplayName(string newName)
        {
            if (displayName == newName) return;
            Undo.RecordObject(this, "Change DisplayName");
            displayName = newName;
            EditorUtility.SetDirty(this);
        }
        public void SetDescription(string newDescription)
        {
            if (description == newDescription) return;
            Undo.RecordObject(this, "Change Description");
            description = newDescription;
            EditorUtility.SetDirty(this);
        }

#endif

    }

}
