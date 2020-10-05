using UnityEngine;

namespace TkrainDesigns.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ScriptedString", menuName = "Shared/String")]

    public class ScriptableString : ScriptableBase
    {

        [SerializeField] string val;
        string backup;

        public string Value
        {
            get
            {
                return val;
            }
            set
            {
                Debug.LogWarning(value);
                val = value;
                AnnounceEvent();
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