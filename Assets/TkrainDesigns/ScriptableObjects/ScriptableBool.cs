using UnityEngine;

namespace TkrainDesigns.ScriptableObjects
{

    [CreateAssetMenu(fileName = "ScriptedBool", menuName = "Shared/Bool")]

    public class ScriptableBool : ScriptableBase
    {

        [SerializeField] bool val;
        bool backup;

        public bool Value
        {
            get
            {
                return val;
            }
            set
            {
                if (val ^ value) //val XOR value will only be true if value is different from val, so changing true to true won't trigger event.
                {
                    val = value;
                    AnnounceEvent();
                }
            }
        }

        private void OnEnable()
        {
            backup = val;
        }

        public void Reset()
        {
            Value = backup;
        }
    } 
}
