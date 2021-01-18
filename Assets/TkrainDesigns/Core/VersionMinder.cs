using TMPro;
using UnityEngine;

namespace TkrainDesigns.Core
{
    public class VersionMinder : MonoBehaviour
    {
        TextMeshProUGUI text;

        void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
#if UNITY_ANDROID
        text.text = $"Android Version {Application.version}beta";
#else
            text.text = $"PC Version {Application.version}beta";
#endif
        }
    }
}
