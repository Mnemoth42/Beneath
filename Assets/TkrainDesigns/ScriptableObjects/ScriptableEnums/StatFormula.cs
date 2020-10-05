using UnityEngine;

namespace TkrainDesigns.ScriptableEnums
{
    [System.Serializable]
    public class StatFormula
    {
        [SerializeField]
        ScriptableStat stat;
        [Range(1, 1000)]
        [SerializeField] public float startingValue = 100;
        [Range(0, 1)]
        [SerializeField] public float percentageAdded = 0.0f;
        [Range(0, 1000)]
        [SerializeField] public float absoluteAdded = 10;


        public float Calculate(int level=1)
        {
            if (level <= 1)
                return startingValue;
            float c = Calculate(level - 1);
            return (c * percentageAdjusted()) + absoluteAdded;
        }

        float percentageAdjusted()
        {
            return (percentageAdded+100.0f)/100.0f;
        }

        public float L1
        {
            get
            {
                return Calculate(1);
            }
        }

        public ScriptableStat Stat { get => stat; set => stat = value; }

        public StatFormula()
        {

        }
        public StatFormula(ScriptableStat _stat, float sv, float pa, float aa)
        {
            Stat = _stat;
            startingValue = sv;
            percentageAdded = pa;
            absoluteAdded = aa;
        }
    }
}