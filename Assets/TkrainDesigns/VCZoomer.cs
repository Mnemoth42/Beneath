using Cinemachine;
using UnityEngine;

public class VCZoomer : MonoBehaviour
{
    CinemachineVirtualCamera virtualCamera;
    [SerializeField] float cameraMinDistance = 5;
    [SerializeField] float cameraMaxDistance = 50;
    [SerializeField] float lerpSpeed = 5.0f;
    [SerializeField] float jogSensitivity = 10.0f;
    float desiredDistance;


    void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        desiredDistance = virtualCamera.m_Lens.FieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        desiredDistance = Mathf.Clamp(desiredDistance - (Input.GetAxis("Mouse ScrollWheel")*jogSensitivity), cameraMinDistance,
                                      cameraMaxDistance);
        float currentDistance = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, desiredDistance, lerpSpeed*Time.deltaTime);
        virtualCamera.m_Lens.FieldOfView = currentDistance;
    }
}
