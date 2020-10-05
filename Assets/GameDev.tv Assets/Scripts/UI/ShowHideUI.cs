using UnityEngine;

namespace GameDevTV.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] KeyCode toggleKey = KeyCode.Escape;
        [SerializeField] GameObject uiContainer = null;
        [SerializeField] GameObject uiHideWhenContainerActive = null;

        // Start is called before the first frame update
        void Start()
        {            
            uiContainer?.SetActive(false);
            uiHideWhenContainerActive?.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                ToggleUI();
            }
        }

        public void ToggleUI()
        {
            if (uiContainer)
            {
                uiContainer.SetActive(!uiContainer.activeSelf);
            }
            if (uiHideWhenContainerActive)
            {
                uiHideWhenContainerActive.SetActive(!uiHideWhenContainerActive.activeSelf);
            }
        }
    }
}