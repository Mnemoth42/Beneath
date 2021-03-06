using TkrainDesigns.Stats;
using UnityEditor;
using UnityEngine;

namespace GameDevTV.Inventories
{
    /// <summary>
    /// An inventory item that can be placed in the action bar and "Used".
    /// </summary>
    /// <remarks>
    /// This class should be used as a base. Subclasses must implement the `Use`
    /// method.
    /// </remarks>
    
    public class ActionItem : InventoryItem
    {
        // CONFIG DATA
        [Tooltip("Does an instance of this item get consumed every time it's used.")]
        [SerializeField] bool consumable = false;
        [Range(0,300)]
        [SerializeField] float cooldown = 5;
        [SerializeField] bool useOnPickup = false;

        public virtual string TimerToken()
        {
            return "Global";
        }

        protected override string LevelNotMetString()
        {
            return $"\n<color=#ff8888>Level {Level} required to use</color>";
        }

        public override string PriceString()
        {
            return consumable ?  base.PriceString():"";
        }

        public override int Level
        {
            get { return base.Level; }
            set
            {
#if UNITY_EDITOR
                if (Application.isPlaying) return;
                Undo.RecordObject(this, "Set Item Level");
                base.Level = value;
                Dirty();
#endif
            }
        }

        public override bool CanHaveStatBoosts()
        {
            return false;
        }

        public bool UseOnPickup { get => useOnPickup; protected set => useOnPickup = value; }

        public virtual bool ShouldPerform()
        {
            return false;
        }

        public float Cooldown => cooldown;

        public void ActivateCooldown(GameObject user)
        {
            user.GetComponent<CooldownManager>().SetTimer(TimerToken(), (int)cooldown);
        }

        // PUBLIC

        /// <summary>
        /// Trigger the use of this item. Override to provide functionality.
        /// </summary>
        /// <param name="user">The character that is using this action.</param>
        public virtual bool Use(GameObject user)
        {
            if (!CanUse(user))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Override this to tell the AI this is a Healing spell
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>

        public virtual bool CanUse(GameObject user)
        {
            if (!user) return false;
            if (!user.TryGetComponent(out PersonalStats stats) || (stats.Level < Level))
            {
                if(!stats) Debug.Log("No stats?"); else Debug.Log($"{stats.Level} < {Level}");
                return false;
            }

            if (user.TryGetComponent<CooldownManager>(out CooldownManager cooldownManager))
            {
                return cooldownManager.TurnsRemaining(TimerToken()) == 0;
            }
            return true;
        }

        public virtual bool AIHealingSpell()
        {
            return false;
        }
        /// <summary>
        /// Override this to tell the AI this is an Attack Spell
        /// </summary>
        /// <returns></returns>
        public virtual bool AIRangedAttackSpell()
        {
            return false;
        }

        /// <summary>
        /// Override this to tell the AI the Range of the spell.
        /// </summary>
        /// <returns></returns>
        public virtual int Range(GameObject user)
        {
            return 1;
        }

        public bool IsConsumable()
        {
            return consumable;
        }

        public override string GetDescription()
        {
            string result = base.GetDescription();
            result += $"\nCooldown {cooldown} turns.";
            if (consumable) result += "\n<color=#4444ff>Consumable</color>";
            if (useOnPickup) result += "\n<color=#4444ff>Uses when picked up</color>";
            return result;
        }

#if UNITY_EDITOR
        void SetConsumable(bool value)
        {
            if (consumable == value) return;
            Undo.RecordObject(this, value?"Is Consumable":"Not Consumable");
            consumable = value;
            Dirty();
        }

        void SetUseOnPickup(bool value)
        {
            if (useOnPickup == value) return;
            Undo.RecordObject(this, value?"Use On Pickup": "Don't Use On Pickup");
            useOnPickup = value;
            Dirty();
        }

        void SetCooldown(float value)
        {
            if (cooldown == value) return;
            Undo.RecordObject(this, "Set Cooldown");
            cooldown = value;
            Dirty();
        }

        bool drawActionItem = true;
        public override void DrawCustomInspector(float width, GUIStyle style)
        {
            base.DrawCustomInspector(width, style);
            drawActionItem = EditorGUILayout.Foldout(drawActionItem, "ActionItem Data");
            if (!drawActionItem) return;
            Level = EditorGUILayout.IntSlider("Level", Level, 1, 100);
            SetConsumable(EditorGUILayout.Toggle("Is Consumable", consumable));
            SetUseOnPickup(EditorGUILayout.Toggle("Use On Pickup", useOnPickup));
            SetCooldown(EditorGUILayout.IntSlider("Cooldown", (int)cooldown, 1,30));
            
        }

#endif
    }
}