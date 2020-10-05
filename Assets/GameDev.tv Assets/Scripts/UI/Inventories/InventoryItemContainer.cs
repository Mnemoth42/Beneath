using TkrainDesigns.Stats;

namespace GameDevTV.Inventories
{
    public class InventoryItemContainer
    {
        public InventoryItem item = null;
        public RandomStatDecorator decorator = null;

        public InventoryItemContainer(InventoryItem item, RandomStatDecorator decorator)
        {
            this.item = item;
            this.decorator = decorator;
        }
    }
}