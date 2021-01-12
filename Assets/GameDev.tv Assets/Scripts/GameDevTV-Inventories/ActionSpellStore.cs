using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevTV.Inventories
{
    public class ActionSpellStore : ActionStore
    {
        public override int MaxAcceptable(InventoryItem item, int index)
        {
            ActionItem actionItem = item as ActionItem;
            if (!actionItem) return 0;
            if (actionItem.IsStackable()) return 0;
            return base.MaxAcceptable(item, index);
        }

        
    }
}