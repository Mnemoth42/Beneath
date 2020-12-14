using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEditor;
using UnityEngine;

namespace RPG.Inventory
{
    [CreateAssetMenu(fileName="New DropLibrary", menuName="Inventory/DropLibrary")]
    public class DropLibrary : ScriptableObject
    {
    

        [SerializeField] List<DropEntry> entries = new List<DropEntry>();

        public Drop GetRandomDrop(int level)
        {
            List<Drop> result = GetPossibleEntries(level);
            if (result.Count == 0) return null;
            return result[Random.Range(0, result.Count)];
        }

        public List<Drop> GetPossibleEntries(int level)
        {
            List<Drop> result = new List<Drop>();
            foreach (DropEntry entry in entries)
            {
                int chance = (int)entry.chance.Evaluate((float) level);
                int amount = (int)entry.amount.Evaluate((float) level);
                if (amount <= 0) continue;
                for (int i = 0; i < chance; i++)
                {
                    result.Add(new Drop(entry.item, Random.Range(0,amount)+1));
                }
            }

            return result;
        }

#if UNITY_EDITOR
        Vector2 scrollpos = new Vector2();
        public void DrawCustomInspector()
        {
            GUIStyle style = new GUIStyle();
            style.padding = new RectOffset(0, 10,0,0);
            scrollpos = EditorGUILayout.BeginScrollView(scrollpos);
            EditorGUILayout.BeginVertical(style);
            int deleteDropEntry = -1;
            for (int i=0;i<entries.Count;i++)
            {
                DropEntry entry = entries[i];
                EditorGUILayout.BeginHorizontal();
                if (entry.DrawCustomInspector(this)) deleteDropEntry = i;
                EditorGUILayout.EndHorizontal();
            }
            if (deleteDropEntry > -1)
            {
                Undo.RecordObject(this, "Remove Entry");
                entries.RemoveAt(deleteDropEntry);
                EditorUtility.SetDirty(this);
            }
            if (GUILayout.Button("Add Drop Item"))
            {
                Undo.RecordObject(this, "Add Entry");
                entries.Add(new DropEntry());
                EditorUtility.SetDirty(this);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
        
#endif

    }

    [System.Serializable]
    public class DropEntry
    {
        public InventoryItem item = null;
        public AnimationCurve chance = new AnimationCurve();
        public AnimationCurve amount = new AnimationCurve();

        public DropEntry()
        {
            chance.AddKey(0, 1);
            chance.AddKey(100, 50);
            amount.AddKey(0, 1);
            amount.AddKey(100, 1);
        }

#if UNITY_EDITOR

        void SetInventoryItem(DropLibrary parent)
        {
            var itemList = Statics.GetScriptableObjectsOfType<InventoryItem>();
            var itemStrings = Statics.GetNamesFromScriptableObjectList(itemList);
            var index = Statics.FindNameInScriptableObjectList(itemStrings, item);
            InventoryItem newItem = itemList[EditorGUILayout.Popup(index, itemStrings.ToArray())];
            //InventoryItem newItem = (InventoryItem)EditorGUILayout.ObjectField(item!=null?item.GetDisplayName():"Item:", item, typeof(InventoryItem), false);
            if (item == newItem) return;
            Undo.RecordObject(parent, "Select Item To Drop");
            item = newItem;
            EditorUtility.SetDirty(parent);
        }

        void SetChance(DropLibrary parent)
        {
            AnimationCurve newCurve = EditorGUILayout.CurveField("Chance", chance);
            if(!chance.Equals(newCurve))
            {
                Undo.RecordObject(parent, "Change Curve");
                chance = newCurve;
                EditorUtility.SetDirty(parent);
            }
        }
        void SetAmount(DropLibrary parent)
        {
            EditorGUI.BeginChangeCheck();
            AnimationCurve newChance = EditorGUILayout.CurveField("Amount", amount);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(parent, "Change Amount");
                amount = newChance;
                EditorUtility.SetDirty(parent);
            }
        }

        public bool DrawCustomInspector(DropLibrary parent)
        {
            
            SetInventoryItem(parent);
            SetChance(parent);
            SetAmount(parent);
            return GUILayout.Button("-");
        }

#endif

    }

    [System.Serializable]
    public class Drop
    {
        public InventoryItem item = null;
        public int count = 1;

        public Drop()
        {

        }

        public Drop(InventoryItem newItem, int newCount)
        {
            item = newItem;
            count = newCount;
        }
    }
}