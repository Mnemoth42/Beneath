using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;

namespace TkrainDesigns.Tiles.Actions
{
    public abstract class PerformableActionItem : ActionItem
    {
        public virtual bool ShouldPerform()
        {
            return false;
        }

        public virtual void PerformAction(GameObject user, GameObject target = null, List<Vector2Int> path = null,
                                          System.Action callback = null)
        {
            
        }
    }
}