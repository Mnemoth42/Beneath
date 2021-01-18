using TMPro;
using UnityEngine;

public class PickedUPItemDisplay : MonoBehaviour
{
    TextMeshPro text;


    [SerializeField] float speed = 10.0f;
    [Range(1, 10)]
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

    public void Initialize(string title)
    {
        if (!text)
        {
            Destroy(gameObject);
            return;
        }

        text.text = title;
        lifeTime = 0;
    }
}
