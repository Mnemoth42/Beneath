
using TkrainDesigns.Saving;
using TkrainDesigns.ScriptableEnums;
using TkrainDesigns.Tiles.Stats;
using UnityEngine;
using UnityEngine.Events;


namespace TkrainDesigns.Stats
{
    public class PersonalStats : MonoBehaviour, ISaveable
    {
        [Header("Character class (Create/TkrainDesigns/ScriptableEnums/New Class)")]
        [Tooltip("The ScriptableClass object contains the stat modifiers required for PersonalStats to work.  " +
            "For a new class, create one with Create/TkrainDesigns/ScriptableEnums/New Class.  " +
            "Add the stats to the Formula array. ")]
        [SerializeField] ScriptableClass personalClass = null;
        [Range(1, 99)]
        [Tooltip("Level for this entity.  Ignored if entity's level is saved in save file.")]
        [SerializeField] int level = 1;
        [Tooltip("If checked, stat system will search entity for possible modifiers.  Generally this should only be on the player.")]
        //[SerializeField] bool shouldUseModifiers = false;

        [SerializeField] Sprite avatar = null;

        [Header("Put stat representing experience value here")] [SerializeField]
        ScriptableStat experienceGainedStat = null;

        public UnityEvent onLevelUpEvent;
        public UnityEvent<Vector3, string> onLevelUpTextEvent;

        public string GetCharacterName()
        {
            if (!personalClass) return "Unknown";
            return personalClass.name;
        }

        public string GetDescription()
        {
            if (!personalClass) return "";
            return personalClass.Description;
        }

        public int Level { get => level; protected set => level = value; }

        public Sprite Avatar => avatar;

        public void SetLevel(int newLevel)
        {
            int oldLevel = Level;
            Level = newLevel;
            if (Level>oldLevel)
            {
                onLevelUpTextEvent?.Invoke(transform.position, $"You have reached Level {level}");
                onLevelUpEvent?.Invoke(); 
                
            }
        }

        public int GetExperienceForKilling()
        {
            if (experienceGainedStat == null) return level*5;
            return (int)GetStatValue(experienceGainedStat);
        }

        

        

        /// <summary>  Returns the stat value specfied.  Will return fallback if specified stat does not exist for this entities class.</summary>
        /// <param name="stat"></param>
        /// <param name="fallback">  default value if stat fails.</param>
        /// <returns>float</returns>
        public float GetStatValue(ScriptableStat stat, float fallback = 1)
        {
            if (!personalClass)
            {
                Debug.LogErrorFormat("Error: {0} does not have a personalClass SO attached.", name);
                return fallback;
            }
            float rawstat = personalClass.GetStat(stat, Level, fallback);
            return  ( rawstat * GetPercentageModifiers(stat)) + GetAdditiveModifiers(stat);
        }

        

        public float GetRawStatValue(ScriptableStat stat, int level=1, float fallback = 1)
        {
            if (!personalClass || !stat) return fallback;
            return personalClass.GetStat(stat, level, fallback);
        }

        public float GetAdditiveModifiers(ScriptableStat stat, float fallback = 0)
        {
            //if (!shouldUseModifiers || !stat) return fallback;
            float accumulator = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifier(stat))
                {
                    accumulator += modifier;
                }
            }

            foreach (var source in stat.GetSources())
            {
                accumulator += GetAdditiveModifiers(source.stat) * source.effectPerLevel;
                accumulator += GetRawStatValue(source.stat) * source.effectPerLevel;
            }
            
            return accumulator;
        }


        public float GetPercentageModifiers(ScriptableStat stat, float fallback = 1)
        {
            //if (!shouldUseModifiers || !stat) return fallback;
            float accumulator = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifier(stat))
                {
                    accumulator += modifier;
                }
            }
            return (accumulator + 100.0f) / 100.0f;
        }

        public SaveBundle CaptureState()
        {
            SaveBundle bundle = new SaveBundle();
            bundle.PutInt("Level", Level);
            return bundle;
        }

        public void RestoreState(SaveBundle bundle)
        {
            Level = bundle.GetInt("Level", Level);
        }
    }
}