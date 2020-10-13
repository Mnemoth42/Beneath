using UnityEngine;
using UnityEngine.Rendering;

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
        [Range(1,1000)]
        [SerializeField] float startValue = 1.0f;
        [Range(1,1000)]
        [SerializeField] float endValue = 100.0f;

        public bool showCurve = false;

        public AnimationCurve ValueCurve = new AnimationCurve();

        

        public float Calculate(int level=1)
        {
            //if (level <= 1)
            //    return startingValue;
            //float c = Calculate(level - 1);
            //return (c * percentageAdjusted()) + absoluteAdded;
            if (level < 1) level = 1;
            return ValueCurve.Evaluate(level);
        }

        float percentageAdjusted()
        {
            return 1.0f;
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
            startValue = 1;
            endValue = 200;
            CreateAnimationCurve();
        }
        public StatFormula(ScriptableStat _stat, float sv, float pa, float aa)
        {
            startValue = 1;
            endValue = 200;
            CreateAnimationCurve();
            Stat = _stat;
        //    startingValue = sv;
        //    percentageAdded = pa;
        //    absoluteAdded = aa;
        }

        void CreateAnimationCurve()
        {
            ValueCurve = new AnimationCurve();
            ValueCurve.AddKey(1, startValue);
            ValueCurve.AddKey(100, endValue);
        }

#if UNITY_EDITOR
        public float GetStartValue() => startValue;
        public float GetEndValue() => endValue;

        public void SetStartValue(float newStartValue, float newEndValue)
        {
            startValue = newStartValue;
            endValue = newEndValue;
            CreateAnimationCurve();
        }

        

        

#endif

    }
}