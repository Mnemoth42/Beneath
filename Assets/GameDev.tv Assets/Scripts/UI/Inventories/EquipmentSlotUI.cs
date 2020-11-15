using UnityEngine;
using UnityEngine.UI;
using GameDevTV.Core.UI.Dragging;
using GameDevTV.Inventories;
using RPG.Inventory;
using TkrainDesigns.ScriptableEnums;
using TkrainDesigns.Stats;

namespace GameDevTV.UI.Inventories
{
    /// <summary>
    /// An slot for the players equipment.
    /// </summary>
    public class EquipmentSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        // CONFIG DATA

        [Tooltip("The gameobject with the Image on it.  It should have an InventoryItemIcon() behavior attached.  " +
            "The system will put the actual icon of the inventory item here.")]
        [SerializeField] InventoryItemIcon icon = null;
        [Tooltip("The gameobject with the affordance image.  The actual sprite will be set at runtime from the EquipSlot icon.")]
        [SerializeField] Image AffordanceIcon = null;
        [Tooltip("The ScriptableEquipSlot that represents what can be placed in this location.  " +
            "If you wish to crate a new equippable location, create a new ScriptableEquipSlot" +
            " and fill in the proper parameters.")]
        [SerializeField] ScriptableEquipSlot equipSlot = null;
        [Tooltip("This is the item that will be displayed in the tooltip if there is nothing in the inspector.")]
        [SerializeField] StatsEquipableItem defaultItem; 

        // CACHE
        Equipment playerEquipment;
        PersonalStats stats;

        // LIFECYCLE METHODS
       
        private void Awake() 
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            stats = player.GetComponent<PersonalStats>();
            playerEquipment = player.GetComponent<Equipment>();
            playerEquipment.EquipmentUpdated += RedrawUI;
        }

        private void Start() 
        {
            if(equipSlot && AffordanceIcon)
            {
                AffordanceIcon.sprite = equipSlot.SlotSprite;
            }
            RedrawUI();
        }

        // PUBLIC

        public int MaxAcceptable(InventoryItem item)
        {
            EquipableItem equipableItem = item as EquipableItem;
            if (equipableItem == null) return 0;
            if (equipableItem.GetAllowedEquipLocation() != equipSlot) return 0;
            if (equipableItem.Level > stats.Level) return 0;
            if (GetItem() != null) return 0;

            return 1;
        }

        public void AddItems(InventoryItem item, int number)
        {
            playerEquipment.AddItem(equipSlot, (EquipableItem) item);
        }

        public InventoryItem GetItem()
        {
            return playerEquipment.GetItemInSlot(equipSlot);
        }

        public int GetNumber()
        {
            if (GetItem() != null)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public void RemoveItems(int number)
        {
            playerEquipment.RemoveItem(equipSlot);
        }

        public InventoryItem GetTooltipItem()
        {
            InventoryItem result = playerEquipment.GetItemInSlot(equipSlot);
            if (result == null) result = defaultItem;
            return result;
        }

        // PRIVATE

        void RedrawUI()
        {
            icon.SetItem(playerEquipment.GetItemInSlot(equipSlot));
            if (AffordanceIcon)
            {
                if (playerEquipment.GetItemInSlot(equipSlot) == null)
                {
                    AffordanceIcon.enabled = true;
                }
                else
                {
                    AffordanceIcon.enabled = false;
                } 
            }
        }
    }
}