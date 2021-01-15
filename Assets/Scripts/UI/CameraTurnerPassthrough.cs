using System.Collections;
using System.Collections.Generic;
using TkrainDesigns.Core.Cameras;
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