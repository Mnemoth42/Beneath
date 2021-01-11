using System.Collections;
using TkrainDesigns.Dungeons;
using UnityEngine;

namespace TkrainDesigns.Tiles.SceneManagement
{
    public class SceneExit : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                LoadNextScene();
            }
        }

        void LoadNextScene()
        {
            Dungeon dungeon = FindObjectOfType<Dungeon>();
            dungeon.StartFromBeginning();
        }
    }
}