using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HitIndicator : MonoBehaviour
{
    Image image;
    [Header("How intense the effect.")]
    [Range(0,1)]
    [SerializeField] float depth=.5f;
    [Header("How quickly screen returns to normal")]
    [Range(.1f,5f)]
    [SerializeField] float recoverySpeed=2;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Color color = image.color;
        float alpha = color.a;
        alpha -= Time.deltaTime * recoverySpeed;
        if (alpha < 0.0f) alpha = 0.0f;
        color.a = alpha;
        image.color = color;
    }

    public void RegisterHit()
    {
        Color color = image.color;
        color.a = depth;
        image.color = color;
    }
}
