using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5.0f;
    public float gravity = -9.81f;

    [Header("Look Settings")]
    public Transform playerCamera;
    public float mouseSensitivity = 2.0f;
    public float lookUpLimit = -85.0f;
    public float lookDownLimit = 85.0f;

    private CharacterController characterController;
    private float cameraVerticalRotation = 0f;
    private Vector3 velocity;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock the cursor to the center of the screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Automatically grab the child camera if forgotten in inspector
        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>().transform;
        }
    }

    void Update()
    {
        HandleLook();
        HandleMovement();
    }

    void HandleMovement()
    {
        // 1. Get WASD / Arrow Key Inputs
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down

        // 2. Calculate movement direction relative to where the player is facing
        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;

        // 3. Apply movement via Character Controller
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        // 4. Apply basic gravity so you don't float away
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Slight downward force to keep grounded securely
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    void HandleLook()
    {
        // 1. Get Mouse Inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // 2. Rotate the player body Left/Right (Y-Axis)
        transform.Rotate(Vector3.up * mouseX);

        // 3. Rotate the camera Up/Down (X-Axis) and Clamp it so you don't flip upside down
        cameraVerticalRotation -= mouseY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, lookUpLimit, lookDownLimit);
        
        playerCamera.localRotation = Quaternion.Euler(cameraVerticalRotation, 0f, 0f);
    }
}