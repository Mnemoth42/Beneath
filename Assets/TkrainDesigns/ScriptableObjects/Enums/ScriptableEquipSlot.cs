using TkrainDesigns.ResourceRetriever;
using UnityEngine;
#pragma warning disable CS0649
namespace TkrainDesigns.ScriptableEnums
{

    [CreateAssetMenu(fileName ="EquipmentSlot", menuName = "TkrainDesigns/ScriptableEnums/New Equipment Slot")]
    public class ScriptableEquipSlot : RetrievableScriptableObject
    {
        
        [SerializeField] string description;
        
        [SerializeField] Sprite slotSprite;

        [SerializeField] int sortOrder;

        public string Description { get => description;  }
        public Sprite SlotSprite { get => slotSprite; }

        public override string GetDescription()
        {
            return description;
        }

        public override string GetDisplayName()
        {
            return description;
        }

        public int SortOrder()
        {
            return sortOrder;
        }
    }
}