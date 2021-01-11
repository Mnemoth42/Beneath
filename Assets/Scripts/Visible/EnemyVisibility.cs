using System;
using TkrainDesigns.Attributes;
using TkrainDesigns.Tiles.Control;
using TkrainDesigns.Tiles.Grids;
using TkrainDesigns.Tiles.Movement;
using UnityEngine;

namespace TkrainDesigns.Tiles.Visible
{
    public class EnemyVisibility : MonoBehaviour
    {
        PlayerController  player;
        public event Action<bool> onChangeVisibility;
        bool visible;

        public bool Visible => visible;

        void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            player.onTurnEnded += TestVisiblity;
            foreach (Renderer rend in GetComponentsInChildren<Renderer>())
            {
                rend.enabled = false;
            }

            visible = false;
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
            visible = false;

            Vector2Int myPosition = transform.position.ToGridPosition(); //TileUtilities.GridPosition(transform.position);
            Vector2Int playerPosition = player.transform.position.ToGridPosition(); //TileUtilities.GridPosition(player.transform.position);
            float distance = Vector2.Distance(myPosition, playerPosition);
            if (distance< 6.0f)
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
            onChangeVisibility?.Invoke(visible);
            if (visible)
            {
                float glow = (5.0f-distance)/12.0f;
                
                glow = Mathf.Clamp(glow, .1f, .5f);
                foreach(Renderer rend in GetComponentsInChildren<Renderer>())
                {
                    rend.material.SetFloat("Glow", glow);
                }
            }
        }
    }
}