using TkrainDesigns.ScriptableEnums;
using UnityEngine;

namespace TkrainDesigns.Tiles.Stats
{
    [System.Serializable]
    public struct Modifier
    {
        public ScriptableStat stat;
        [Range(-100, 100)]
        public float value;

        public Modifier(ScriptableStat _stat, float _value)
        {
            stat = _stat;
            value = _value;
        }

        public bool CompareModifier(Modifier modToCompare)
        {
            if (stat != modToCompare.stat) return false;
            if (value != modToCompare.value) return false;
            return true;
        }
    }
}