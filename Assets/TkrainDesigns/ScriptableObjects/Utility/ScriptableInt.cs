using UnityEngine;

namespace TkrainDesigns.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ScriptedInt", menuName = "Shared/Int")]
    public class ScriptableInt : ScriptableBase
    {
        int backup;

        [SerializeField] int val;

        public int Value
        {
            get => val;
            set
            {
                if (val != value)
                {
                    val = value;
                    AnnounceEvent();
                }
            }
        }

        void OnEnable()
        {
            backup = val;
        }

        public void Reset()
        {
            Value = backup;
        }
    }
}