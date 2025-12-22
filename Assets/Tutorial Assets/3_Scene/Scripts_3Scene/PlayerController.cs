using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;







[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public bool CanMove { get; private set; } = true;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float gravity = 20f;
    [SerializeField] private float jumpForce = 8f;

    [Header("Mouse Look")]
    [SerializeField] private float lookSpeedX = 2f;
    [SerializeField] private float lookSpeedY = 2f;
    [SerializeField] private float upperLookLimit = 80f;
    [SerializeField] private float lowerLookLimit = 80f;

    private Camera playerCamera;
    private CharacterController characterController;

    private Vector3 moveDirection;
    private float verticalVelocity;
    private float rotationX;

    void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!CanMove) return;

        HandleMovement();
        HandleMouseLook();
        HandleJumpAndGravity();

        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void HandleMovement()
    {
        float moveZ = Input.GetAxis("Vertical");   // W / S
        float moveX = Input.GetAxis("Horizontal"); // A / D

        Vector3 forward = transform.forward * moveZ;
        Vector3 right = transform.right * moveX;

        moveDirection.x = (forward.x + right.x) * walkSpeed;
        moveDirection.z = (forward.z + right.z) * walkSpeed;
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeedX;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeedY;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleJumpAndGravity()
    {
        if (characterController.isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = -2f; // keep grounded

            if (Input.GetButtonDown("Jump")) // Space
                verticalVelocity = jumpForce;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        moveDirection.y = verticalVelocity;
    }
}
