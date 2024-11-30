using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    private float moveSpeed = 50f;  // Movement speed of the player
    public float rotationSpeed = 10f;  // Speed at which the player rotates

    [Header("Camera Settings")]
    public float mouseSensitivity = 2f;  // Mouse sensitivity for camera rotation
    
    private float xRotation = 0f;  // Store the camera's vertical rotation (to clamp it)
    private float yRotation = 0f;  // Store the camera's horizontal rotation

    public Vector3 moveDirection;
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;  // To lock the cursor
        Cursor.visible = false; 
    }

    // Update is called once per frame
    void Update()
    {
        // Handle player movement
        MovePlayer();

        // Handle camera rotation (mouse look)
        RotateCamera();
    }

    private void MovePlayer()
    {
        // Get input for movement (WASD or Arrow keys)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float moveY = 0f; // Default to no vertical movement

        // Get the camera's forward, right, and up directions
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        Vector3 cameraUp = Camera.main.transform.up;  // Get the up direction of the camera

        // Flatten the camera directions on the X and Z axes to avoid any unwanted vertical movement
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraUp.x = 0f; // Keep up and down movement only on the Y axis
        cameraUp.z = 0f;

        // Normalize the camera directions to get proper movement
        cameraForward.Normalize();
        cameraRight.Normalize();
        cameraUp.Normalize();

        // Calculate the movement direction based on camera orientation
        moveDirection = cameraForward * moveZ + cameraRight * moveX;

        // Vertical movement (up/down) is based on the camera's up direction
        if (Input.GetKey(KeyCode.Space))  // Move up if Space is held
        {
            moveY = 1f;
        }
        else if (Input.GetKey(KeyCode.LeftShift))  // Move down if Left Shift is held
        {
            moveY = -1f;
        }

        moveDirection.y = moveY;

        // Apply movement to the player, including Y axis movement
        rb.AddForce(moveDirection * moveSpeed, ForceMode.Force);

    }

    private void RotateCamera()
    {
        // Get mouse input for camera rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the camera vertically (X-axis) with clamping to prevent flipping
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // Clamp vertical rotation
        yRotation += mouseX;

        // Apply the camera rotation
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Rotate camera vertically
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f); // Rotate player horizontally
    }
}
