using GameDevTV.Inventories;
using GameDevTV.UI.Inventories;
using TkrainDesigns.Stats;
using TkrainDesigns.Tiles.Combat;
using TkrainDesigns.Tiles.Control;
using UnityEngine;
using UnityEngine.UI;

public class UIAvater : MonoBehaviour, IItemHolder
{
    [SerializeField] bool isTarget = false;

    PlayerController player;
    PersonalStats stats;
    Image image;
    public Sprite maleSprite;
    public Sprite femaleSprite;

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        stats = player.GetComponent<PersonalStats>();
        image = GetComponent<Image>();
    }

    void Start()
    {
        if (!isTarget)
        {
            if (player.TryGetComponent(out CharacterGenerator cg))
            {
                image.sprite = cg.isMale ? maleSprite : femaleSprite;
            }
        }
    }

    public void UpdateTargetDisplay(BaseController target)
    {
        if (target)
        {
            stats = target.GetComponent<PersonalStats>();
            image.sprite = stats.Avatar;
        }
    }

    public InventoryItem GetItem()
    {
        GridFighter fighter = stats.GetComponent<GridFighter>();
        return fighter.GetCurrentWeaponConfig();
    }

    public InventoryItem GetTooltipItem()
    {
        return GetItem();
    }
}
