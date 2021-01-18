using TkrainDesigns.Stats;
using TkrainDesigns.Tiles.Control;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiExperienceDisplay : MonoBehaviour
{
    

    TextMeshProUGUI text;
    Experience experience;
    PersonalStats stats;
    BaseController target;
    PlayerController player;
    [SerializeField] Image image;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        player = FindObjectOfType<PlayerController>();
        stats = player.GetComponent<PersonalStats>();
        experience = player.GetComponent<Experience>();
    }

    void Start()
    {
        UpdateDisplay();
    }

    void OnEnable()
    {
        
        {
            experience.ExperienceGained += UpdateDisplay;
        }
        
    }

    void OnDisable()
    {
        
        {
            experience.ExperienceGained -= UpdateDisplay;
        }
    }

    

    void UpdateDisplay()
    {
        if (image)
        {
            image.fillAmount= experience.GetExperience / stats.GetStatValue(experience.ExperienceNeededStat);
        }
        text.text = $"Exp: {experience.GetExperience}/{(int)stats.GetStatValue(experience.ExperienceNeededStat)}";
    }
}
