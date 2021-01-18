using UnityEngine;

namespace TkrainDesigns.ScriptableEnums
{
    [System.Serializable]
    public class StatFormula
    {
        [SerializeField]
        ScriptableStat stat;
        [Range(1,1000)]
        [SerializeField] float startValue = 1.0f;
        [Range(1,1000)]
        [SerializeField] float endValue = 100.0f;

        public bool showCurve = false;

        public AnimationCurve ValueCurve = new AnimationCurve();

        

        public float Calculate(int level=1)
        {
            if (level < 1) level = 1;
            return ValueCurve.Evaluate(level);
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