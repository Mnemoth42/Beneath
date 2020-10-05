using GameDevTV.Inventories;
using System.Collections.Generic;
using TkrainDesigns.ScriptableEnums;
using TkrainDesigns.Tiles.Stats;

namespace RPG.Inventory
{
    public class StatsEquipment : Equipment, IModifierProvider
    {
        public IEnumerable<float> GetAdditiveModifier(ScriptableStat stat)
        {
            foreach (ScriptableEquipSlot slot in GetAllPopulatedSlots())
            {
                if (GetItemInSlot(slot) is IModifierProvider item)
                {
                    foreach (float modifier in item.GetAdditiveModifier(stat))
                    {
                        yield return modifier;
                    }
                }
            }
        }

        public IEnumerable<float> GetPercentageModifier(ScriptableStat stat)
        {
            foreach (ScriptableEquipSlot slot in GetAllPopulatedSlots())
            {
                if (GetItemInSlot(slot) is IModifierProvider item)
                {
                    foreach (float modifier in item.GetPercentageModifier(stat))
                    {
                        yield return modifier;
                    }
                }
            }
        }
    }
}