using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;

namespace TkrainDesigns.Tiles.Skills
{
    public class SkillStore : ActionStore
    {
        public List<ActionItem> startingActionItems;

        void Start()
        {
            ActionStore actionStore = GetComponent<ActionStore>();
            int i = 0;
            foreach (ActionItem item in startingActionItems)
            {
                actionStore.AddAction(item, i, 1);
                i++;
            }
        }
    }
}