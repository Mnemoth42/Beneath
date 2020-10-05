using UnityEngine;

namespace TkrainDesigns.ScriptableObjects
{
    /// <summary>
    /// Scriptable Object that can be used to keep a running tally... An example is collecting a certain number of keys... Count can be increased or decreased by
    /// any behavior that is using it and the value compared to determine if a goal has been reached.
    /// </summary>
    [CreateAssetMenu(fileName = "Key", menuName = "Shared/Key")]
    public class ScriptedKey : ScriptableBase
    {
        int count;

        [SerializeField] string keyName = "generic";

        public string KeyName => keyName;

        public int Count
        {
            get => count;
            set
            {
                if (count != value)
                {
                    Debug.LogWarning(keyName + " count = " + value);
                    count = value;
                    AnnounceEvent();
                }
            }
        }

        void OnEnable()
        {
            count = 0;
        }
    }
}