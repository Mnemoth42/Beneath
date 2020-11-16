using TkrainDesigns.Grids.Stats;
using TkrainDesigns.Stats;
using TkrainDesigns.Tiles.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewPlayerUIDisplay : MonoBehaviour
{
    [SerializeField] Image playerIcon;

    [Header("Health")]
    [SerializeField] Image healthBar;
    [SerializeField] TextMeshProUGUI healthText;

    [Header("Experience")]
    [SerializeField] Image experienceBar;
    [SerializeField] TextMeshProUGUI experienceText;

    [Header("Level")] [SerializeField] TextMeshProUGUI LevelText;

    Health health;
    Experience experience;
    PersonalStats stats;
    GameObject user;

     void Awake()
    {
        user = GameObject.FindGameObjectWithTag("Player");
        experience = user.GetComponent<Experience>();
        stats = user.GetComponentInParent<PersonalStats>();
        health = user.GetComponent<Health>();
    }

     void Start()
     {
         playerIcon.sprite = stats.Avatar;
     }

     void OnEnable()
     {
         experience.ExperienceGained += RedrawAll;
         health.onTakeDamage.AddListener(RedrawAll);
         stats.onLevelUpEvent.AddListener(RedrawAll);
     }

     void OnDisable()
     {
         experience.ExperienceGained -= RedrawAll;
         health.onTakeDamage.RemoveListener(RedrawAll);
         stats.onLevelUpEvent.RemoveListener(RedrawAll);
     }

     void RedrawAll()
     {
         healthBar.fillAmount = health.HealthAsPercentage;
         experienceBar.fillAmount = experience.GetExperience / stats.GetStatValue(experience.ExperienceNeededStat);
         healthText.text = $"Health: {(int) health.CurrentHealth}/{(int) health.MaxHealth}";
         experienceText.text =
             $"Exp: {experience.GetExperience}/{(int) stats.GetStatValue(experience.ExperienceNeededStat)}";
         LevelText.text = stats.Level.ToString();
     }
}
