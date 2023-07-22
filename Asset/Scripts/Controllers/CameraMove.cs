using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class CameraMove : MonoBehaviour
{
    private CharacterController characterController;

    private float horizontalAxis, verticalAxis;
    private Vector3 movementDirection, movementInput;

    [SerializeField] 
    private Camera mainCamera;

    [SerializeField] 
    private float movementSpeed = 5f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void MyInput()
    {
        horizontalAxis = Input.GetAxisRaw("Horizontal");
        verticalAxis = Input.GetAxisRaw("Vertical");
    }

    private void HandleMovement()
    {
        MyInput();

        movementInput = Quaternion.Euler(mainCamera.transform.eulerAngles.x, mainCamera.transform.eulerAngles.y, 0) * new Vector3(horizontalAxis, 0, verticalAxis);
        movementDirection = movementInput.normalized;

        characterController.Move(movementDirection * movementSpeed * Time.deltaTime);
    }
}