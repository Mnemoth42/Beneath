using Cinemachine;
using UnityEngine;

namespace TkrainDesigns.Core.Cameras
{
    public class CameraTurner : MonoBehaviour
    {
        [SerializeField] float zoomSpeed = 10.0f;
        [SerializeField] float turnSpeed = 10.0f;

        CinemachinePOV pov;
        CinemachineFramingTransposer framing;
        CinemachineVirtualCamera cam;
        float targetAngle=0f;
        float currentAngle = 0f;

        void Awake()
        {
            cam = GetComponent<CinemachineVirtualCamera>();
            if(!cam) Debug.LogError("Virtual Camera not found");
            pov = cam.GetCinemachineComponent<CinemachinePOV>();
            if(!pov) Debug.LogError("POV not found");

        }

        void Update()
        {
            UpdateFieldOfView();
        
            currentAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * turnSpeed);
            pov.m_HorizontalAxis.Value = currentAngle;
        }

        void UpdateFieldOfView()
        {
            float lensefov = cam.m_Lens.FieldOfView;
            float scrollwheel = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            if (scrollwheel > 0)
            {
                cam.m_Lens.FieldOfView = Mathf.Max(lensefov - scrollwheel, 20);
            }
            else if (scrollwheel < 0)
            {
                cam.m_Lens.FieldOfView = Mathf.Min(lensefov - scrollwheel, 75);
            }
        }

        public void TurnCamera(bool left)
        {
        
            targetAngle+= left ? -60f : 60f;
        

        }
    }
}