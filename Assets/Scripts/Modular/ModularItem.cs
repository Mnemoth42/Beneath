using System.Collections.Generic;
using PsychoticLab;
using TkrainDesigns.ResourceRetriever;
using UnityEngine;

namespace TkrainDesigns.Tiles.Modular
{
    [CreateAssetMenu(fileName="New Equipable", menuName = "Inventory/Equipables")]
    public class ModularItem : RetrievableScriptableObject
    {
        [Header("The name of the object in the Modular Characters Prefab representing this item.")]
        public List<string> objectsToActivate = new List<string>();
        [Header("Slot Categories to deactivate when this item is activated.")]
        public List<ModularCategories> slotsToDeactivate = new List<ModularCategories>();

        

        public void ActivateItem(CharacterRandomizer character)
        {
            if (!character)
            {
                Debug.LogError($"No Character to activate {name} on.");
                return;
            }
            character.ActivateItems(objectsToActivate, slotsToDeactivate);
        }

        public override string GetDisplayName()
        {
            return "TODO";
        }

        public override string GetDescription()
        {
            return "TODO";
        }
    }
}
