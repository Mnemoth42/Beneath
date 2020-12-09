using System;
using System.Collections;
using System.Collections.Generic;
using TkrainDesigns.Tiles.Control;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    PlayerController player;

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        transform.position = player.transform.position + Vector3.up*3;
    }
}
