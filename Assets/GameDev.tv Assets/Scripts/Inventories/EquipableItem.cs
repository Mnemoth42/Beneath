using TkrainDesigns.ScriptableEnums;
using UnityEditor;
using UnityEngine;

namespace GameDevTV.Inventories
{
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

        // PUBLIC

        public ScriptableEquipSlot GetAllowedEquipLocation()
        {
            return allowedEquipLocation;
        }

#if UNITY_EDITOR

        public void SetAllowedEquipLocation(ScriptableEquipSlot slot)
        {
            Undo.RecordObject(this, "Change Equip Slot");
            allowedEquipLocation = slot;
            Dirty();
        }

        bool displayEquipableItem = true;
        public override void DrawCustomInspector(float width, GUIStyle style)
        {
            base.DrawCustomInspector(width, style);
            displayEquipableItem = EditorGUILayout.Foldout(displayEquipableItem, "EquipableItem Data", style);
            if (!displayEquipableItem) return;
            SetAllowedEquipLocation((ScriptableEquipSlot)EditorGUILayout.ObjectField("Allowed Equip Location", allowedEquipLocation, typeof(ScriptableEquipSlot), false));
        }

#endif
    }
}