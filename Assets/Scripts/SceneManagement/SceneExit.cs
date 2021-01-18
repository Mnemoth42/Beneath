using UnityEngine;

namespace TkrainDesigns.SceneManagement
{
    public class SceneExit : MonoBehaviour
    {

        System.Action sceneFinishedCallback = null;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                LoadNextScene();
            }
        }

        public void SetSceneFinishedCallback(System.Action callback) => sceneFinishedCallback = callback;

        void LoadNextScene()
        {
            sceneFinishedCallback?.Invoke();
        }
    }
}