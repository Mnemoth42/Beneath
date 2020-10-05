using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTurnerPassthrough : MonoBehaviour
{
    public void Clicked(bool left)
    {
        FindObjectOfType<CameraTurner>().TurnCamera(left);
    }
}
