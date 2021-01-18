using UnityEngine;

#pragma warning disable CS0649
namespace TkrainDesigns.Core
{
    public class CameraFacer : MonoBehaviour
    {

        Camera cameraToLookAt = null;
        // Start is called before the first frame update
        void Start()
        {
            cameraToLookAt = Camera.main;
        }

        // Update is called once per frame 
        void LateUpdate()
        {
            transform.LookAt(cameraToLookAt.transform);
            transform.rotation = Quaternion.LookRotation(cameraToLookAt.transform.forward);
        }
    }
}
