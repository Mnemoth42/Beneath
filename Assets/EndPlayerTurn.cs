using System;
using System.Collections;
using System.Collections.Generic;
using TkrainDesigns.Tiles.Control;
using UnityEngine;

public class EndPlayerTurn : MonoBehaviour
{
    [SerializeField] GameObject visibleButton;
    PlayerController playerController;
    bool isPlayerTurn = false;
    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerController.onBeginTurnEvent.AddListener(ShowUI);
        playerController.onEndTurnEvent.AddListener(HideUI);
    }

    void ShowUI()
    {
        isPlayerTurn = true;
        visibleButton.SetActive(true);
    }

    void HideUI()
    {
        isPlayerTurn = false;
        visibleButton.SetActive(false);
    }

    public void TryEndTurn()
    {
        playerController.EndTurn();
    }

}
