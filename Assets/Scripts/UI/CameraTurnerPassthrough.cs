using TkrainDesigns.Core;
using UnityEngine;

namespace TkrainDesign.Core.Cameras
{
    public class CameraTurnerPassthrough : MonoBehaviour
    {
        public void Clicked(bool left)
        {
            FindObjectOfType<CameraTurner>().TurnCamera(left);
        }
    }
}