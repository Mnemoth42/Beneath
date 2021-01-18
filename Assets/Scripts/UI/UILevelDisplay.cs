using TkrainDesigns.Stats;
using TkrainDesigns.Tiles.Control;
using TMPro;
using UnityEngine;

public class UILevelDisplay : MonoBehaviour
{
    [SerializeField] bool isTarget = false;
    TextMeshProUGUI text;
    PlayerController player;
    PersonalStats stats;
    BaseController target;
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        player = FindObjectOfType<PlayerController>();
        stats = player.GetComponent<PersonalStats>();
        
    }

    void Start()
    {
        UpdateDisplay();
    }

    void OnEnable()
    {
        if (!isTarget)
        
        {
            stats.onLevelUpEvent.AddListener(UpdateDisplay);
            UpdateDisplay();
        }
    }

    void OnDisable()
    {
        if (!isTarget)
        
        {
            stats.onLevelUpEvent.RemoveListener(UpdateDisplay);
        }
    }

    public void UpdateTargetDisplay(BaseController newTarget)
    {
        target = newTarget;
        stats = newTarget.GetComponent<PersonalStats>();
        UpdateDisplay();
    }
    void UpdateDisplay()
    {

        text.text = $"Level: {stats.Level}";
    }
}
