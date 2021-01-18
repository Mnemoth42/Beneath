using UnityEngine;
using UnityEngine.SceneManagement;

namespace TkrainDesigns.Core
{
    public class QuitGame : MonoBehaviour
    {
        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}
