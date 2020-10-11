using System.Collections;
using System.Collections.Generic;
using TkrainDesigns.Grids.Stats;
using TkrainDesigns.Stats;
using TkrainDesigns.Tiles.Control;
using TkrainDesigns.Tiles.Stats;
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
    [SerializeField] Slider slider;

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
        if (slider)
        {
            slider.value = experience.GetExperience / stats.GetStatValue(experience.ExperienceNeededStat);
        }
        text.text = $"Exp: {experience.GetExperience}/{(int)stats.GetStatValue(experience.ExperienceNeededStat)}";
    }
}
