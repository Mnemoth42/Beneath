using UnityEngine;
using TMPro;
using GameDevTV.Inventories;

namespace GameDevTV.Core.UI
{
    public class UICoinDisplay : MonoBehaviour
    {
        Inventory inventory;
        TextMeshProUGUI text;
        // Start is called before the first frame update
        void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            inventory = player.GetComponent<Inventory>();
        }

        // Update is called once per frame
        void Update()
        {
            text.text = inventory.Coins.ToString();
        }
    }
}