﻿using System;
using GameDevTV.Core.UI.Dragging;
using GameDevTV.Inventories;
using TkrainDesigns.Tiles.Actions;
using TkrainDesigns.Tiles.Control;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameDevTV.UI.Inventories
{
    /// <summary>
    /// The UI slot for the player action bar.
    /// </summary>
    public class ActionSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {



        // CONFIG DATA
        [SerializeField] InventoryItemIcon icon = null;
        [SerializeField] int index = 0;
        [SerializeField] bool consumables = false;
        [SerializeField] CooldownUI cooldownTimer;
        [SerializeField] Image HighlightImage = null;
        

        // CACHE
        ActionStore store;
        GameObject player;
        CooldownManager cooldownManager;
        PlayerController controller;


        int turnsToWait = 0;
        bool isPlayerTurn = false;

        void Start()
        {
            UpdateIcon();
        }

        // LIFECYCLE METHODS
        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            cooldownManager = player.GetComponent<CooldownManager>();
            cooldownManager.onCooldownChanged += SetTimer;
            if (consumables)
            {
                store = player.GetComponent<ActionPotionStore>();
            }
            else
            {
                store = player.GetComponent<ActionSpellStore>();
            }

            controller = player.GetComponent<PlayerController>();
            controller.onBeginTurnEvent.AddListener(OnBeginTurn);
            controller.onEndTurnEvent.AddListener(OnEndTurn);
            store.StoreUpdated += UpdateIcon;
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
            Debug.Log("OnBeginTurn()");
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

        float lastClick = 0.0f;

        void Update()
        {
            lastClick -= Time.deltaTime;
        }

        public void HandleClick()
        {
            Debug.Log("Click");
            if (!isPlayerTurn) return;
            player.GetComponent<PlayerController>().CancelClicks();
            if (lastClick <=0.0f)
            {
                lastClick = .5f;
                return;
            }
            Debug.Log("Double Click");
            lastClick = 0.0f;
            var item = store.GetAction(index);
            if (item && item.CanUse(player))
            {
                Debug.Log($"Using {item}");
                if (store.Use(index, player))
                {
                    ClearHighlights();
                    Highlight(Color.blue);
                }
                else
                {
                    Highlight(Color.white);
                }
            } else Debug.Log("Can't Use Item");
        }

        //Alternate way of getting item
        public InventoryItem GetTooltipItem()
        {
            return store.GetAction(index);
        }
    }
}
