using System;
using System.Linq;
using System.Reflection.Emit;
using TkrainDesigns.ScriptableEnums;
using UnityEditor;
using UnityEngine;

namespace TkrainDesigns.ResourceRetriever
{
    public abstract class RetrievableScriptableObject : ScriptableObject, IHasItemID, ISerializationCallbackReceiver
    {
        [Header("Unique UUID for saving references to this Scriptable Object", order = 0)]
        [SerializeField] string itemId;
        
        public string GetItemID()
        {
            return itemId;
        }

        public void ReIssueItemID()
        {
            itemId = Guid.NewGuid().ToString();
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            // Generate and save a new UUID if this is blank.
            if (string.IsNullOrWhiteSpace(itemId))
            {
                itemId = Guid.NewGuid().ToString();
            }
            // Test for multiple objects with the same UUID
            //var items = Resources.LoadAll<RetrievableScriptableObject>("").Where(p => p.GetItemID() == itemId).ToList();
            //if (items.Count > 1)
            //{
            //    itemId = Guid.NewGuid().ToString();
            //}
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            // Require by the ISerializationCallbackReceiver but we don't need
            // to do anything with it.
        }

#if UNITY_EDITOR

        public void Dirty()
        {
            EditorUtility.SetDirty(this);
        }

        public void SetUndo(string caption="Whatever You Just Did")
        {
            Undo.RecordObject(this, caption);
        }

        

        protected void SetItem<T>(ref T itemToChange, T value, string caption = "item") where T: IComparable<T>
        {
            if (itemToChange.Equals(value)) return;
            SetUndo($"Set {caption}");
            itemToChange = value;
            Dirty();
        }

        protected void SetItemNoCheck<T>(T itemToChange, T value, string caption = "item")
        {
            SetUndo($"Set {caption}");
            itemToChange = value;
            Dirty();
        }

        protected void DrawFloat(ref float itemToChange, string title = "Float Value", string tooltip = "")
        {
            GUIContent label = new GUIContent(title, tooltip);
            SetItem(ref itemToChange,EditorGUILayout.FloatField(label, itemToChange), title);
        }

        protected void DrawFloatAsIntSlider(ref float itemToChange, int minValue = 1, int maxValue = 100,
                                            string title = "Float Value", string tooltip = "")
        {
            GUIContent label = new GUIContent(title, tooltip);
            SetItem(ref itemToChange, EditorGUILayout.IntSlider(label, (int)itemToChange, minValue, maxValue), title);
        }

        protected void DrawIntSlider(ref int itemToChange, int minValue = 1, int maxValue = 100,
                                     string title = "Int Value", string tooltip="")
        {
            GUIContent label = new GUIContent(title, tooltip);
            SetItem(ref itemToChange, EditorGUILayout.IntSlider(label, itemToChange, minValue, maxValue), title);
        }

        protected void DrawBoolSlider(ref bool itemToChange, string title = "Boolean Value", string tooltip = "")
        {
            GUIContent label = new GUIContent(title, tooltip);
            SetItem(ref itemToChange, EditorGUILayout.Toggle(label, itemToChange));
        }

        protected void DrawTextField(ref string itemToChange, string title = "String Value", string tooltip = "")
        {
            GUIContent label = new GUIContent(title, tooltip);
            SetItem(ref itemToChange, EditorGUILayout.TextField(label, itemToChange));
        }

        protected void DrawTextArea(ref string itemToChange, string title = "String Value", string tooltip = "")
        {
            GUIContent label = new GUIContent(title, tooltip);
            GUIStyle longStyle = new GUIStyle(GUI.skin.textArea) { wordWrap = true };
            EditorGUILayout.LabelField(label);
            SetItem(ref itemToChange, EditorGUILayout.TextArea(itemToChange, longStyle));
        }

        

#endif

    }
}
