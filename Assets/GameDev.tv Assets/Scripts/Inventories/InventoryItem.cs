using System.Collections.Generic;
using TkrainDesigns.ResourceRetriever;
using TkrainDesigns.ScriptableEnums;
using TkrainDesigns.Stats;
using UnityEditor;
using UnityEngine;
#pragma warning disable CS0649
namespace GameDevTV.Inventories
{
    /// <summary>
    /// A ScriptableObject that represents any item that can be put in an
    /// inventory.
    /// </summary>
    /// <remarks>
    /// In practice, you are likely to use a subclass such as `ActionItem` or
    /// `EquipableItem`.
    /// </remarks>
    public abstract class InventoryItem : RetrievableScriptableObject
    {
        public const string CENTERTAG = "<align=\"center\">";

        // CONFIG DATA

        [Tooltip("Item name to be displayed in UI.")]
        public string displayName = "";
        [Tooltip("Item description to be displayed in UI.")]
        [SerializeField][TextArea] public string description = "";
        [Tooltip("The UI icon to represent this item in the inventory.")]
        [SerializeField]  Sprite icon = null;
        [Tooltip("The prefab that should be spawned when this item is dropped.")]
        [SerializeField]  Pickup pickup = null;
        [Tooltip("If true, multiple items of this type can be stacked in the same inventory slot.")]
        [SerializeField]  bool stackable = false;
        
        [SerializeField]  List<ScriptableStat> potentialStatBoosts = new List<ScriptableStat>();

        RandomStatDecorator decorator = new RandomStatDecorator();


        public virtual bool CanHaveStatBoosts()
        {
            return true;
        }

        // STATE
        

        public RandomStatDecorator Decorator { get => decorator; set => decorator = value; }

        // PUBLIC

        /// <summary>
        /// Get the inventory item instance from its UUID.
        /// </summary>
        /// <param name="itemId">
        /// String UUID that persists between game instances.
        /// </param>
        /// <returns>
        /// Inventory item instance corresponding to the ID.
        /// </returns>
        public static InventoryItem GetFromId(string itemId)
        { 
            return ResourceRetriever<InventoryItem>.GetFromID(itemId);
        }


        /// <summary>
        /// Spawn the pickup gameobject into the world.
        /// </summary>
        /// <param name="position">Where to spawn the pickup.</param>
        /// <param name="number">How many instances of the item does the pickup represent.</param>
        /// <param name="level">Used to calculate the values of potential stat moifiers</param>
        /// <returns>Reference to the pickup object spawned.</returns>
        public Pickup SpawnPickup(Vector3 position, int number, int level)
        {
            if (pickup == null)
            {
                Debug.LogErrorFormat("InventoryItem {0} does not have its pickup assigned.", name);
                return null;
            }
            var pickupToSpawn = Instantiate(pickup);
            pickupToSpawn.transform.position = position;
            if (!stackable)
            {
                var instance = Instantiate(this);
                if (!instance.Decorator.IsAlreadyInitialized)
                {
                    if (!stackable)
                    {
                        instance.Decorator = new RandomStatDecorator(level, potentialStatBoosts.ToArray());
                    }
                }
                pickupToSpawn.Setup(instance, number);
                return pickupToSpawn;
            }

            pickupToSpawn.Setup(this, number);
            return pickupToSpawn;
            
        }

        public void InitDecorator(int level)
        {
            Debug.LogFormat("{0} - InitDecorator - Level {1}, {2}", displayName, level, potentialStatBoosts.Count);
            Decorator = new RandomStatDecorator(level, potentialStatBoosts.ToArray());
        }

        public Sprite GetIcon()
        {
            return icon;
        }



        public bool IsStackable()
        {
            return stackable;
        }
        
        public string GetDisplayName()
        {
            return displayName;
        }

        public virtual string GetDescription()
        {
            return description;
        }

        public Pickup GetPickup()
        {
            return pickup;
        }


        
        public virtual bool UseImmediate(GameObject user, int qty=1)
        {
            return false;
        }

        public virtual bool CanUseImmediate(GameObject user)
        {
            return false;
        }

        public static string StatString(string stat, float value, string modificationtype)
        {
#pragma warning disable CA1305 // Specify IFormatProvider
            return string.Format("<b><color={2}>{0:f0} {4} {1} to {3}.</b><br>", value, value < 0 ? "penalty" : "bonus", value < 0 ? "#FF1111" : "#1111FF", stat, modificationtype);
#pragma warning restore CA1305 // Specify IFormatProvider
        }

        public static string EditorStatString(string stat, float value, string modificationtype)
        {
            string bonus = "<color=#8888ff>bonus</color>";
            
            if (value < 0) bonus = "<color=#ff8888>penalty</color>";
            return $"\n{Mathf.Abs(value)} {modificationtype} {bonus} to {stat}. ";
        }


        public string GetRawDescription()
        {
            return description;
        }

#if UNITY_EDITOR

        

        public void SetDisplayName(string newDisplayName)
        {
            if (displayName.Equals(newDisplayName)) return;
            Undo.RecordObject(this, "Change Display Name");
            displayName = newDisplayName;
            Dirty();
        }

        public void SetDescription(string newDescription)
        {
            if (description.Equals(newDescription)) return;
            Undo.RecordObject(this, "Change Description");
            description = newDescription;
            Dirty();
        }

        public void SetIcon(Sprite newIcon)
        {
            if (icon == newIcon) return;
            Undo.RecordObject(this, "Change Icon");
            icon = newIcon;
            Dirty();
        }

        public void SetPickup(Pickup newPickup)
        {
            if (pickup == newPickup) return;
            Undo.RecordObject(this, "Change Pickup");
            pickup = newPickup;
            Dirty();
        }

        public void AddPotentialStatModifier()
        {
            Undo.RecordObject(this, "Add Potential StatModifier");
            potentialStatBoosts.Add(null);
            Dirty();
        }

        public void RemovePotentialStatModifier(int index)
        {
            string potential = "Remove Blank Potential Boost";
            if (potentialStatBoosts[index] != null)
            {
                potential = $"Remove Potential Boost {potentialStatBoosts[index].Description}";
            }
            Undo.RecordObject(this, potential);
            potentialStatBoosts.RemoveAt(index);
            Dirty();
        }

        public void SetPotentialStatModifier(int index, ScriptableStat statToSet)
        {
            if (statToSet == potentialStatBoosts[index]) return;
            Undo.RecordObject(this, "Change Potential Stat");
            potentialStatBoosts[index] = statToSet;
            Dirty();
        }

        public ScriptableStat GetPotentialStat(int index)
        {
            return potentialStatBoosts[index];
        }
        public int PotentialStatCount()
        {
            return potentialStatBoosts.Count;
        }

        public void SetIsStackable(bool value)
        {
            if (IsStackable() == value) return;
            Undo.RecordObject(this, value?"Set Stackable":"Set Not Stackable");
            stackable = value;
            Dirty();
        }

        bool drawInventoryItem = true;
        public virtual void DrawCustomInspector(float width, GUIStyle style)
        {
            drawInventoryItem = EditorGUILayout.Foldout(drawInventoryItem, "InventoryItem Data", style);
            if (!drawInventoryItem) return;
            //SetDisplayName(EditorGUILayout.TextField("Display Name", displayName));
            SetItem(ref displayName, EditorGUILayout.TextField("Display Name", displayName),"Display Name");
            float textWidth = width * .66f - 40;
            GUIStyle longStyle = new GUIStyle(GUI.skin.textArea) { wordWrap = true };
            //longStyle.fixedHeight = longStyle.CalcHeight(new GUIContent(GetRawDescription()), textWidth);
            SetDescription(EditorGUILayout.TextArea(description, longStyle));
            SetIcon((Sprite)EditorGUILayout.ObjectField("Icon", icon, typeof(Sprite), false));
            SetPickup((Pickup)EditorGUILayout.ObjectField("Pickup", pickup, typeof(Pickup), false));
            SetIsStackable(EditorGUILayout.Toggle("isStackable", IsStackable()));
            if (!CanHaveStatBoosts()) return;
            int itemtoDelete = -1;
            if (potentialStatBoosts.Count > 0)
            {
                EditorGUILayout.LabelField("Potential Random Stat Boosts");
            }
            for (int i = 0; i < potentialStatBoosts.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                ScriptableStat stat = potentialStatBoosts[i];
                SetPotentialStatModifier(i,(ScriptableStat)EditorGUILayout.ObjectField(stat==null?"Select Stat": stat.Description,stat, typeof(ScriptableStat),false));
                if (GUILayout.Button("-"))
                {
                    itemtoDelete = i;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (itemtoDelete > -1)
            {
                RemovePotentialStatModifier(itemtoDelete);
            }

            if (GUILayout.Button("+ Potential Random Stat Boost"))
            {
                AddPotentialStatModifier();
            }

        }

#endif

    }
}
