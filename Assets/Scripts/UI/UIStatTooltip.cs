using TkrainDesigns.ScriptableEnums;
using TMPro;
using UnityEngine;

public class UIStatTooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText = null;
    [SerializeField] TextMeshProUGUI bodyText = null;

    // PUBLIC

    public void Setup(ScriptableStat item)
    {
        if (item == null) return;
        titleText.text = item.DisplayName;
        bodyText.text = item.Description;
    }
}
