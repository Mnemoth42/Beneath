using TkrainDesigns.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TkrainDesigns.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {

        [SerializeField] GameObject ResumeButton;

        void Awake()
        {
            ResumeButton.SetActive(SavingSystem.SaveFileExists("Character"));
        }

        public void LoadScene(int scene)
        {
       
            SceneManager.LoadScene(scene);
        }

        public void LoadNewScene(int scene)
        {
            SavingSystem.Save("Character");
            SceneManager.LoadScene(scene);
        }
    }
}