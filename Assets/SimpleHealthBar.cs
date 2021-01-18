using TkrainDesigns.Attributes;
using TkrainDesigns.Tiles.Visible;
using UnityEngine;
using UnityEngine.UI;

public class SimpleHealthBar : MonoBehaviour
{
    Health health;
   
    EnemyVisibility visiblity;
    [SerializeField] Image image;
    bool hasBeenHit = false;
    void Awake()
    {
        
        health = GetComponentInParent<Health>();
        health.onTakeDamage.AddListener(UpdateUI);
        health.onDeath.AddListener(OnDeath);
        if (health.TryGetComponent(out visiblity))
        {
            visiblity.onChangeVisibility += UpdateVisibility;
        }
    }
    
    void UpdateVisibility(bool visible)
    {
        foreach (Image i in GetComponentsInChildren<Image>())
        {
            i.enabled =  visible;
        }
    }

    void UpdateUI()
    {
        hasBeenHit = true;
        UpdateVisibility(true);
        image.fillAmount = health.HealthAsPercentage;
        
    }

    void OnDeath()
    {
        if (visiblity)
        {
            visiblity.onChangeVisibility -= UpdateVisibility;
        }

        health.onTakeDamage.RemoveListener(UpdateUI);
        Destroy(gameObject);
    }
}
