using UnityEngine;

namespace TkrainDesigns.Core
{
    public class ScaleOnLoad : MonoBehaviour
    {
        [SerializeField] float Min = .8f;
        [SerializeField] float Max = 1.2f;

        void Start()
        {
            transform.localScale *= Random.Range(Min, Max);
        }
    }
}
