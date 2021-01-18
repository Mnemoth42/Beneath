using UnityEngine;

namespace GameDevTV.Inventories
{
    [CreateAssetMenu(fileName="CoinDrop", menuName = "Inventory/Coins")]
    public class CoinDrop : InventoryItem
    {
        public int CoinAmount
        {
            get => Random.Range(Level, Level * Mathf.Max(Level/4, 2));
        }
    }
}