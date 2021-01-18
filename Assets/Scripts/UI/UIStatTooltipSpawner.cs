using GameDevTV.Core.UI.Tooltips;
using UnityEngine;

namespace TkrainDesigns.ScriptableEnums
{
    public class UIStatTooltipSpawner : TooltipSpawner
    {
        public override void UpdateTooltip(GameObject tooltip)
        {
            if (TryGetComponent(out IStatHolder holder) && tooltip!=null && tooltip.TryGetComponent(out UIStatTooltip tip))
            {
                tip.Setup(holder.GetStat());
            }
        }

        public override bool CanCreateTooltip()
        {
            if (TryGetComponent(out IStatHolder holder))
            {
                return holder.GetStat() != null;
            }
            return false;
        }
    }

    public interface IStatHolder
    {
        ScriptableStat GetStat();
    }
}