using TMPro;
using UnityEngine;

namespace RPG.UI
{
    //[RequireComponent(typeof(TextMeshPro))]
    public class HealthChangeDisplay : MonoBehaviour
    {
        TextMeshPro text;


        [SerializeField] float speed = 10.0f;
        [Range(1,10)]
        [SerializeField] float duration = 1.0f;
        [SerializeField] Color damageColor = Color.red;
        [SerializeField] Color healedColor = Color.green;


        float lifeTime = -1;

        private void Awake()
        {
            text = GetComponent<TextMeshPro>();
            if (!text)
            {
                text = GetComponentInChildren<TextMeshPro>();
            }
            
        }

        

        



        // Update is called once per frame
        void Update()
        {
            if (!text)
            {
                Destroy(gameObject);
                return;
            }
            transform.localPosition += Vector3.up * speed * Time.deltaTime;
            text.alpha = 1 - (lifeTime / duration);
            lifeTime += Time.deltaTime;
            if (lifeTime > duration)
            {
                Destroy(gameObject);
            }
        }

        public void Initialize(float delta)
        {
            if (!text)
            {
                Destroy(gameObject);
                return;
            }
            text.color= delta>0? healedColor: damageColor;
            text.text = string.Format("{0:F0}", Mathf.Abs(delta));
            lifeTime = 0;
        }
    } 
}
