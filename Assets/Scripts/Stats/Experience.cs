using System;
using JetBrains.Annotations;
using TkrainDesigns.Extensions;
using TkrainDesigns.Saving;
using TkrainDesigns.ScriptableEnums;
using TkrainDesigns.Stats;
using UnityEngine;

#pragma warning disable CS0649


namespace TkrainDesigns.Tiles.Stats
{
    [RequireComponent(typeof(PersonalStats))]
    public class Experience : MonoBehaviour, ISaveable
    {
        private const string TokenExperience = "Experience";
        [Header("Put ScriptableStat representing Experience needed per level here.")]
        [SerializeField]
        
        ScriptableStat experienceNeededStat;

        [NotNull] PersonalStats stats;
        int experience=0;

        void Awake()
        {
            stats=GetComponent<PersonalStats>();
        }

        /// <summary>
        /// Occurs when [experience gained].
        /// </summary>
        public event Action ExperienceGained;

        /// <summary>
        /// Adds ExperienceToAdd to the current experience.  ***Side Effect, invokes experienceGained event.
        /// </summary>
        /// <param name="experienceToAdd">The experience to add.</param>
        /// <returns>The new experience value.</returns>
        public int GainExperience(int experienceToAdd)
        {

            experience += experienceToAdd.Floor(0);
            TestLevelUp();
            ExperienceGained?.Invoke();
            return experience;
        }
        /// <summary>
        /// Should only be called by BaseStats when levelling up.  Spends amount of experience gained after gaining a level.  
        /// </summary>
        /// <param name="experienceToSpend">The experience to spend.</param>
        /// <returns>The new experience value.</returns>
        public int SpendExperience(int experienceToSpend)
        {
            experience -= experienceToSpend;
            experience = experience.Floor(0);
            return experience;
        }

        /// <summary>
        /// Checks experience against the experience needed for the next level.  If greater, reduces experience and increases level until experience is less than the experience needed.   **Side effect, calls OnLevelUpEvent()
        /// </summary>
        private void TestLevelUp()
        {
            int level = stats.Level;
            int experienceNeeded = (int)stats.GetStatValue(experienceNeededStat, 100000000);
            while (GetExperience >= experienceNeeded)
            {
                level += 1;
                SpendExperience(experienceNeeded);
                experienceNeeded = (int)stats.GetRawStatValue(experienceNeededStat, level: level, fallback: 1000000.0f);
            }

            if (level > stats.Level)
            {
                stats.SetLevel(level);
            }
        }

        /// <summary>
        /// Getter for the current experience value.  Read only.
        /// </summary>
        /// <value>
        /// experience
        /// </value>
        public int GetExperience => experience;


        public ScriptableStat ExperienceNeededStat => experienceNeededStat;

        public SaveBundle CaptureState()
        {
            SaveBundle save = new SaveBundle();
            save.PutInt(TokenExperience, experience);
            return save;
        }

        public void RestoreState(SaveBundle save)
        {
            experience = save.GetInt(TokenExperience);
            ExperienceGained?.Invoke();
        }
    } 
}
