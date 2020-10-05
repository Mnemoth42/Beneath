using TkrainDesigns.ResourceRetriever;
using UnityEngine;
#pragma warning disable CS0649
namespace TkrainDesigns.ScriptableEnums
{

    [CreateAssetMenu(fileName ="EquipmentSlot", menuName = "TkrainDesigns/ScriptableEnums/New Equipment Slot")]
    public class ScriptableEquipSlot : RetrievableScriptableObject
    {
        
        [SerializeField] string description;
        [PreviewSprite]
        [SerializeField] Sprite slotSprite;

        public string Description { get => description;  }
        public Sprite SlotSprite { get => slotSprite; }

        

        

        
    }
}