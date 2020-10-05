using System.Collections.Generic;
using TkrainDesigns.ScriptableEnums;

namespace TkrainDesigns.Tiles.Stats
{
    public interface IModifierProvider
    {
        //IEnumerable<float> GetAdditiveModifier(EStat stat);
        //IEnumerable<float> GetPercentageModifier(EStat stat);
        IEnumerable<float> GetAdditiveModifier(ScriptableStat stat);
        IEnumerable<float> GetPercentageModifier(ScriptableStat stat);
    }
}