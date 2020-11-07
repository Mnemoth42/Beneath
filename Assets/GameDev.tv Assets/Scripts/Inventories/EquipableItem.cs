using System;
using System.Collections.Generic;
using TkrainDesigns.ScriptableEnums;
using UnityEditor;
using UnityEngine;

namespace GameDevTV.Inventories
{

    [System.Serializable]
    public class ItemPair
    {
        public string category="";
        public int index=0;
    }

    /// <summary>
    /// An inventory item that can be equipped to the player. Weapons could be a
    /// subclass of this.
    /// </summary>
    [CreateAssetMenu(menuName = ("GameDevTV/GameDevTV.UI.InventorySystem/Equipable Item"))]
    public class EquipableItem : InventoryItem
    {
        // CONFIG DATA
        [Tooltip("Where are we allowed to put this item.")]
        [SerializeField] ScriptableEquipSlot allowedEquipLocation = null;
        [Header("The name of the object in the Modular Characters Prefab representing this item.")]
        [SerializeField]List<ItemPair> objectsToActivate = new List<ItemPair>();
        [Header("Slot Categories to deactivate when this item is activated.")]
        [SerializeField] List<string> slotsToDeactivate = new List<string>();

        public IEnumerable<ItemPair> ObjectsToActivate => objectsToActivate;
        public IEnumerable<string> SlotsToDeactivate => slotsToDeactivate;
        // PUBLIC

        public ScriptableEquipSlot GetAllowedEquipLocation()
        {
            return allowedEquipLocation;
        }
            public static readonly List<string> Categories = new List<string>
                                                         {
                                                             "HeadCoverings_Base_Hair",
                                                             "HeadCoverings_No_FacialHair",
                                                             "HeadCoverings_No_Hair",
                                                             "All_01_Hair",
                                                             "Helmet",
                                                             "All_04_Back_Attachment",
                                                             "All_05_Shoulder_Attachment_Right",
                                                             "All_06_Shoulder_Attachment_Left",
                                                             "All_07_Elbow_Attachment_Right",
                                                             "All_08_Elbow_Attachment_Left",
                                                             "All_09_Hips_Attachment",
                                                             "All_10_Knee_Attachement_Right",
                                                             "All_11_Knee_Attachement_Left",
                                                             "Elf_Ear",
                                                             "Female_Head_All_Elements",
                                                             "Female_Head_No_Elements",
                                                             "Female_01_Eyebrows",
                                                             "Male_Head_All_Elements",
                                                             "Male_Head_No_Elements",
                                                             "Male_01_Eyebrows",
                                                             "Male_02_FacialHair",
                                                             "Torso",
                                                             "UpperArm",
                                                             "LowerArm",
                                                             "Hand",
                                                             "Hips",
                                                             "Leg"
                                                         };


#if UNITY_EDITOR

        public void SetAllowedEquipLocation(ScriptableEquipSlot slot)
        {
            Undo.RecordObject(this, "Change Equip Slot");
            allowedEquipLocation = slot;
            Dirty();
        }

        void AddCategoryToRemove()
        {
            SetUndo("Add Category To Remove");
            slotsToDeactivate.Add("");
            Dirty();
        }

        void RemoveCategoryToRemove(int i)
        {
            SetUndo("Remove Category To Remove");
            slotsToDeactivate.RemoveAt(i);
            Dirty();
        }

        void SetCategoryToRemove(int i, string category)
        {
            if (slotsToDeactivate[i] == category) return;
            SetUndo("Change Category To Remove");
            slotsToDeactivate[i] = category;
            Dirty();
        }

        void AddObjectToActivate()
        {
            SetUndo("Add Item To Activate");
            objectsToActivate.Add(new ItemPair());
            Dirty();
        }

        void RemoveObjectToActivate(int i)
        {
            SetUndo("Remove Item To Activate");
            objectsToActivate.RemoveAt(i);
            Dirty();
        }

        void SetObjectToActivate(int i, string category, int index)
        {
            if (index == objectsToActivate[i].index && category == objectsToActivate[i].category) return;
            SetUndo("Change Item To Activate");
            objectsToActivate[i].index=index;
            objectsToActivate[i].category = category;
            Dirty();
        }

        bool displayEquipableItem = true;
        public override void DrawCustomInspector(float width, GUIStyle style)
        {
            base.DrawCustomInspector(width, style);
            displayEquipableItem = EditorGUILayout.Foldout(displayEquipableItem, "EquipableItem Data", style);
            if (!displayEquipableItem) return;
            SetAllowedEquipLocation(DrawScriptableObjectList("Allowed Equip Location", allowedEquipLocation));
            int itemToRemove = -1;
            for (int i = 0; i < objectsToActivate.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                int cat = Categories.IndexOf(objectsToActivate[i].category);
                if (cat == -1) cat = 0;
                cat = EditorGUILayout.Popup(cat, Categories.ToArray());
                int index = EditorGUILayout.IntSlider(objectsToActivate[i].index, 0, 30);
                if (GUILayout.Button("-")) itemToRemove = i;
                SetObjectToActivate(i, Categories[cat], index);
                EditorGUILayout.EndHorizontal();
            }

            if (itemToRemove>-1)
            {
                RemoveObjectToActivate(itemToRemove);
            }
            if(GUILayout.Button("Add Item To Activate")) AddObjectToActivate();
            itemToRemove = -1;
            for (int i = 0; i < slotsToDeactivate.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                int cat = Categories.IndexOf(slotsToDeactivate[i]);
                if (cat < 0) cat = 0;
                cat = EditorGUILayout.Popup(cat, Categories.ToArray());
                SetCategoryToRemove(i, Categories[cat]);
                if (GUILayout.Button("-")) itemToRemove = i;
                EditorGUILayout.EndHorizontal();
            }

            if (itemToRemove >-1)
            {
                RemoveCategoryToRemove(itemToRemove);
            }

            if (GUILayout.Button("Add Category to Remove"))
            {
                AddCategoryToRemove();
            }
        }

#endif
    }
}