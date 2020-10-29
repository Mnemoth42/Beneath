﻿using System;
using System.Collections;
using System.Collections.Generic;
using TkrainDesigns.Grids.Stats;
using TkrainDesigns.Tiles.Control;
using TkrainDesigns.Tiles.Core;
using TkrainDesigns.Tiles.Grids;
using TkrainDesigns.Tiles.Movement;
using UnityEngine;

namespace Tkraindesigns.Tiles.Core
{
    public class EnemyVisibility : MonoBehaviour
    {
        PlayerController  player;

        void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            player.onTurnEnded += TestVisiblity;
            foreach (Renderer rend in GetComponentsInChildren<Renderer>())
            {
                rend.enabled = false;
            }

            GetComponent<AIController>().onTurnEnded += TestVisiblity;
            GetComponent<Health>().onDeath.AddListener(Death);
            GetComponent<GridMover>().onMoveStepCompleted += TestVisiblity;
        }

        void Death()
        {
            player.onTurnEnded -= TestVisiblity;
        }

        void OnDestroy()
        {
            player.onTurnEnded -= TestVisiblity;
        }

        void TestVisiblity()
        {
            bool visible = false;
            Vector2Int myPosition = TileUtilities.GridPosition(transform.position);
            Vector2Int playerPosition = TileUtilities.GridPosition(player.transform.position);
            if (Vector2.Distance(myPosition, playerPosition) < 6.0f)
            {
                if (!Visibility.ExtendedRayTrace(myPosition, player.transform.position, transform.position, transform))
                {
                    visible = true;
                }
            }
            foreach (Renderer rend in GetComponentsInChildren<Renderer>())
            {
                rend.enabled = visible;
            }
        }
    }
}