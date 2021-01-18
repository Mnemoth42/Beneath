using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CooldownUI : MonoBehaviour
{
    TextMeshProUGUI cooldownTimer;
    Image image;
    float countdown;
    void Awake()
    {
        image = GetComponent<Image>();
        cooldownTimer = GetComponentInChildren<TextMeshProUGUI>();
        image.enabled = false;
        if (cooldownTimer)
        {
            cooldownTimer.enabled = false;
        }
    }

    public void SetCooldownTimer(int cooldownTime)
    {
        if (!cooldownTimer)
        {
            return;
        }
        if (cooldownTime>0)
        {
            
            image.enabled = true;
            cooldownTimer.enabled = true;
            cooldownTimer.text = $"{cooldownTime}";
        }
        else
        {
            image.enabled = false;
            cooldownTimer.enabled = false;
        }
    }

    
}
