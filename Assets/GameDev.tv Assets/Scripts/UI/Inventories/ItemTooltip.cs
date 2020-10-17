using UnityEngine;
using TMPro;
using GameDevTV.Inventories;
using UnityEngine.UI;

namespace GameDevTV.UI.Inventories
{
    /// <summary>
    /// Root of the tooltip prefab to expose properties to other classes.
    /// </summary>
    public class ItemTooltip : MonoBehaviour
    {
        // CONFIG DATA
        [SerializeField] TextMeshProUGUI titleText = null;
        [SerializeField] TextMeshProUGUI bodyText = null;
        [SerializeField] Image icon = null;

        // PUBLIC

        public void Setup(InventoryItem item)
        {
            if (item == null) return;
            icon.sprite = item.GetIcon();
            titleText.text = item.GetDisplayName();
            bodyText.text = item.GetDescription();
        }
    }
}
