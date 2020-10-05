using System;
using System.Collections;
using System.Collections.Generic;
using TkrainDesigns.Saving;
using TkrainDesigns.Tiles.Control;
using TkrainDesigns.Tiles.Dungeons;
using UnityEngine;

namespace TkrainDesigns.Tiles.SceneManagement
{
    public class SceneExit : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerController>(out PlayerController controller))
            {
                StartCoroutine(LoadNextScene(controller));
            }
        }

        IEnumerator LoadNextScene(PlayerController controller)
        {
            SaveableEntity entity = controller.GetComponent<SaveableEntity>();
            yield return null;
            Dungeon dungeon = FindObjectOfType<Dungeon>();
            dungeon.GenerateNewDungeon();
        }
    }
}