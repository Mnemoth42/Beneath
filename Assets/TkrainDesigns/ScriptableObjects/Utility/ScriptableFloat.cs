using System;
using UnityEngine;

namespace TkrainDesigns.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ScriptedFloat", menuName = "Shared/Float")]
    public class ScriptableFloat : ScriptableBase
    {
        float backup;

        [SerializeField] float val;

        public float Value
        {
            get => val;
            set
            {
                if (Math.Abs(val - value) > .001f)
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