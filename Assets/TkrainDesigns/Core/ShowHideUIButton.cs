using UnityEngine;

#pragma warning disable CS0649
namespace TkrainDesigns.Core
{
    public class ShowHideUIButton : MonoBehaviour
    {
        [SerializeField] GameObject[] uiObjectsToHide;
        [SerializeField] GameObject[] uiObjectsToShow;

        bool isShowingUI = true;

        public void Toggle()
        {
            isShowingUI = !isShowingUI;
            foreach(GameObject uiToHide in uiObjectsToHide)
            {
                uiToHide.SetActive(isShowingUI);
            }
            foreach(GameObject uiToShow in uiObjectsToShow)
            {
                uiToShow.SetActive(!isShowingUI);
            }
        }
    }
}
