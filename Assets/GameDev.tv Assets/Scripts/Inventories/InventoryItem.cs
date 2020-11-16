using System;
using System.Collections.Generic;
using TkrainDesigns.ResourceRetriever;
using TkrainDesigns.ScriptableEnums;
using TkrainDesigns.Stats;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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
        [SerializeField] int level = 1;
        
        [SerializeField]  List<ScriptableStat> potentialStatBoosts = new List<ScriptableStat>();

        RandomStatDecorator decorator = new RandomStatDecorator();


        public virtual bool CanHaveStatBoosts()
        {
            return !stackable;
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

        public virtual int Price()
        {
            return level * level * 10;
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
                //Debug.LogErrorFormat("InventoryItem {0} does not have its pickup assigned.", name);
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

                instance.level = level;
                pickupToSpawn.Setup(instance, number);
                return pickupToSpawn;
            }

            pickupToSpawn.Setup(this, number);
            return pickupToSpawn;
            
        }

        public void InitDecorator(int level)
        {
            Decorator = new RandomStatDecorator(level, potentialStatBoosts.ToArray());
        }

        public Sprite GetIcon()
        {
            return icon;
        }

        public int Level
        {
            get => level;
            set => level = value;
        }

        public bool IsStackable()
        {
            return stackable;
        }
        
        public override string GetDisplayName()
        {
            if (decorator != null) return $"{displayName} {decorator.Alias}";
            return displayName;
        }

        public virtual string PriceString()
        {
            return $"\nSelling Price: {Price()}";
        }

        public override string GetDescription()
        {
            string result = description;
            if(!stackable)
            {
                if (Application.isPlaying)
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    PersonalStats stats = player.GetComponent<PersonalStats>();
                    if (level > stats.Level)
                    {
                        result += $"\n<color=#ff8888>Level {level} required to equip</color>";
                    } 
                    else
                    {
                        result += $"\nLevel {level}";
                    }
                }
                else
                {
                    result += $"\nLevel {level}";
                }
            }

            result += PriceString();
#if UNITY_EDITOR
            if (pickup==null)
            {
                result += "\n" + BadString("No Pickup Selected!"); 
            }
#endif
            return result;
        }

        public Pickup GetPickup()
        {
            return pickup;
        }

        public virtual int SortOrder()
        {
            return 0;
        }
        
        public virtual bool UseImmediate(GameObject user, int qty=1)
        {
            return false;
        }

        public virtual bool CanUseImmediate(GameObject user)
        {
            return false;
        }

        public static string GoodString(string text)
        {
            return $"<color=#8888ff>{text}</color>";
        }

        public static string BadString(string text)
        {
            return $"<color=#ff8888>{text}</color>";
        }

        public static string BoldString(string text)
        {
            return $"<b>{text}</b>";
        }

        public static string ItalicString(string text)
        {
            return $"<i>{text}</i>";
        }


        public static string StatString(string stat, float value, string modificationtype)
        {
            string bonus = BoldString(value < 0 ? BadString("penalty") : GoodString("bonus"));
            
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
                potential = $"Remove Potential Boost {potentialStatBoosts[index].DisplayName}";
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

        protected GUIStyle indent;
        protected void BeginIndent()
        {
            EditorGUILayout.BeginVertical(indent);
        }

        protected void EndIndent()
        {
            EditorGUILayout.EndVertical();
        }

        public virtual void DrawCustomInspector(float width, GUIStyle style)
        {
            if (GUILayout.Button($"UUID={GetItemID()}  Press to change"))
            {
                ReIssueItemID();
                Dirty();
            }
            GUIStyle indent = new GUIStyle();
            indent.padding= new RectOffset(10,10,0,0);
            drawInventoryItem = EditorGUILayout.Foldout(drawInventoryItem, "InventoryItem Data", style);
            if (!drawInventoryItem) return;
            EditorGUILayout.BeginVertical(indent);
                SetItem(ref displayName, EditorGUILayout.TextField("Display Name", displayName), "Display Name");
                GUIStyle longStyle = new GUIStyle(GUI.skin.textArea) {wordWrap = true};
            //longStyle.fixedHeight = longStyle.CalcHeight(new GUIContent(GetRawDescription()), textWidth);
                SetDescription(EditorGUILayout.TextArea(description, longStyle));
                SetIcon((Sprite) EditorGUILayout.ObjectField("Icon", icon, typeof(Sprite), false));
                SetPickup(DrawObjectList("Pickup", pickup));
                SetIsStackable(EditorGUILayout.Toggle("isStackable", IsStackable()));
                if (!CanHaveStatBoosts())
                {
                    EditorGUILayout.EndVertical();
                    return;
                };
                int itemtoDelete = -1;
                if (potentialStatBoosts.Count > 0)
            {
                EditorGUILayout.LabelField("Potential Random Stat Boosts");
            }
            for (int i = 0; i < potentialStatBoosts.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                ScriptableStat stat = potentialStatBoosts[i];
                SetPotentialStatModifier(i,DrawScriptableObjectList(stat==null?"Select Stat":stat.DisplayName , stat));
                //SetPotentialStatModifier(i,(ScriptableStat)EditorGUILayout.ObjectField(stat==null?"Select Stat": stat.DisplayName,stat, typeof(ScriptableStat),false));
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
            EditorGUILayout.EndVertical();
        }



#endif

    }
}
