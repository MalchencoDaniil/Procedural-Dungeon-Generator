using UnityEngine;

[RequireComponent(typeof(Camera))]

public class CameraController : MonoBehaviour
{
    private float horizontalAxis, verticalAxis, axisRotation = 0;

    [SerializeField] 
    private Transform target;

    [SerializeField]
    private float mouseSensetivity = 10f;

    private void Update()
    {
        MyInput();
        MoveCamera();
    }

    private void MyInput()
    {
        horizontalAxis = Input.GetAxisRaw("Mouse X") * mouseSensetivity * Time.deltaTime;
        verticalAxis = Input.GetAxisRaw("Mouse Y") * mouseSensetivity * Time.deltaTime;
    }

    private void MoveCamera()
    {
        axisRotation -= verticalAxis;
        axisRotation = Mathf.Clamp(axisRotation, -80f, 80f);

        transform.localRotation = Quaternion.Euler(axisRotation, 0.0f, 0.0f);
        target.Rotate(Vector3.up * horizontalAxis);
    }
}