using UnityEngine;

public class GimbleRotator : MonoBehaviour
{
    [SerializeField] float rotationSpeed=10;
    float desiredRotation = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            desiredRotation += 60;
            if (desiredRotation > 360) desiredRotation = 0;
        }

        float currentRotation = transform.eulerAngles.y;
        if (currentRotation < 0) currentRotation += 360; //normalize direction.
        currentRotation = Mathf.Lerp(currentRotation, desiredRotation, rotationSpeed * Time.deltaTime);
        transform.eulerAngles=new Vector3(45,currentRotation, 0);
    }
}
