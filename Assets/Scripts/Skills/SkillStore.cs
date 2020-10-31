using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using TkrainDesigns.Stats;
using UnityEditor;
using UnityEngine;



namespace TkrainDesigns.Tiles.Skills
{
    [System.Serializable]
    public class LearnableSkill
    {
        [SerializeField]
        [Range(0,100)]
        int level=1;
        [SerializeField]
        [Range(0,5)]
        int slot = 0;
        [SerializeField]
        [Tooltip("This will replace any action already in the slot.")]
        ActionItem itemToLearn = null;

        public int GetLevel()
        {
            return level;
        }

        public int GetSlot()
        {
            return slot;
        }

        public ActionItem GetItem()
        {
            return itemToLearn;
        }

        #if UNITY_EDITOR
        public void SetLevel(int value)
        {
            level = value;
        }

        public void SetSlot(int value)
        {
            slot = value;
        }

        public void SetItem(ActionItem item)
        {
            itemToLearn = item;
        }

        #endif

    }
    [RequireComponent(typeof(ActionSpellStore))]
    public class SkillStore : MonoBehaviour
    {
        [SerializeField] List<LearnableSkill> skills = new List<LearnableSkill>();
        List<LearnableSkill> learnedSkills = new List<LearnableSkill>();

        PersonalStats stats;
        ActionSpellStore store;
        void Awake()
        {
            stats = GetComponent<PersonalStats>();
            store = GetComponent<ActionSpellStore>();
        }

        void OnEnable()
        {
            stats.onLevelUpEvent.AddListener(UpdateSkillStore);
            UpdateSkillStore();
        }

        void OnDisable()
        {
            stats.onLevelUpEvent.RemoveListener(UpdateSkillStore);
        }

        void UpdateSkillStore()
        {
            Debug.Log($"Testing Skills against Level {stats.Level}");
            foreach (LearnableSkill skill in skills)
            {
                if (learnedSkills.Contains(skill)) continue;
                if (skill.GetItem() == null) continue;
                if (skill.GetLevel() <= stats.Level)
                {
                    Debug.Log($"Adding {skill.GetItem()} to ActionStore");
                    store.AddAction(skill.GetItem(), skill.GetSlot(), 1);
                    learnedSkills.Add(skill);
                }
            }
        }
        #if UNITY_EDITOR
        public List<LearnableSkill> Skills => skills;

        public void RemoveSkill(LearnableSkill skill)
        {
            Undo.RecordObject(this, "Remove Skill");
            skills.Remove(skill);
            EditorUtility.SetDirty(this);
        }

        public void AddSkill(LearnableSkill skill)
        {
            Undo.RecordObject(this, "Add Skill");
            skills.Add(new LearnableSkill());
            EditorUtility.SetDirty(this);
        }

        #endif
    }
}