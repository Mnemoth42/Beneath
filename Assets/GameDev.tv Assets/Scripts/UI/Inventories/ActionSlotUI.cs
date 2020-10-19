using GameDevTV.Core.UI.Dragging;
using GameDevTV.Inventories;
using TkrainDesigns.Tiles.Actions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameDevTV.UI.Inventories
{
    /// <summary>
    /// The UI slot for the player action bar.
    /// </summary>
    public class ActionSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>, IPointerClickHandler
    {



        // CONFIG DATA
        [SerializeField] InventoryItemIcon icon = null;
        [SerializeField] int index = 0;
        [SerializeField] CooldownUI cooldownTimer;
        [SerializeField] Image HighlightImage = null;

        // CACHE
        ActionStore store;
        GameObject player;
        CooldownManager cooldownManager;


        int turnsToWait = 0;
        bool isPlayerTurn = false;

        // LIFECYCLE METHODS
        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            cooldownManager = player.GetComponent<CooldownManager>();
            cooldownManager.onCooldownChanged += SetTimer;
            store = player.GetComponent<ActionStore>();
            store.StoreUpdated += UpdateIcon;
            store.OnBeginTurn += OnBeginTurn;
            store.OnEndTurn += OnEndTurn;
        }

        void SetTimer()
        {
            if (!cooldownTimer) return;
            ActionItem item = store.GetAction(index);
            if (item)
            {

                turnsToWait = (cooldownManager.TurnsRemaining(item.TimerToken()));

            }
            else turnsToWait = 0;
            cooldownTimer.SetCooldownTimer(turnsToWait);

        }


        // PUBLIC

        public void AddItems(InventoryItem item, int number)
        {
            store.AddAction(item, index, number);
        }

        public InventoryItem GetItem()
        {
            return store.GetAction(index);
        }

        public int GetNumber()
        {
            return store.GetNumber(index);
        }

        public int MaxAcceptable(InventoryItem item)
        {
            return store.MaxAcceptable(item, index);
        }

        public void RemoveItems(int number)
        {
            store.RemoveItems(index, number);
        }

        // PRIVATE

        void UpdateIcon()
        {
            icon.SetItem(GetItem(), GetNumber());
            SetTimer();
        }

        void OnBeginTurn()
        {
            ActionItem item = store.GetAction(index);
            isPlayerTurn = true;
            SetTimer();
        }

        void OnEndTurn()
        {
            Highlight(Color.white);
            isPlayerTurn = false;
        }

        void Highlight(Color color)
        {
            if (HighlightImage)
            {
                HighlightImage.color = color;
            }
        }

        public static void ClearHighlights()
        {
            foreach (ActionSlotUI ui in FindObjectsOfType<ActionSlotUI>())
            {
                ui.Highlight(Color.white);
            }
        }

        

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!isPlayerTurn) return;
            if (eventData.clickCount < 2)
            {
                return;
            }
            var item = store.GetAction(index);
            if (item && item.CanUse(player))
            {
                if (store.Use(index, player))
                {
                    ClearHighlights();
                    Highlight(Color.blue);
                }
                else
                {
                    Highlight(Color.white);
                }
            }
        }
    }
}
