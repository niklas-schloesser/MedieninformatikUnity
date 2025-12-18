using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody playerRigidbody;

    private float currentSpeed;
    public float moveSpeed = 4f;
    [SerializeField] private float sprintSpeed = 6f;

    [SerializeField] private float jumpForce = 5f;

    [SerializeField] private LayerMask groundMask;
    [HideInInspector] public bool allowedToJump;
    private bool isGrounded;

    void Start()
    {
        playerRigidbody.freezeRotation = true;
        currentSpeed = moveSpeed;
        allowedToJump = true;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && allowedToJump)
        {
            isGrounded = false;
            playerRigidbody.AddForce(UnityEngine.Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
        {
            currentSpeed = sprintSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentSpeed = moveSpeed;
        }
    }

    void FixedUpdate()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        UnityEngine.Vector3 move = (transform.forward * ver + transform.right * hor) * currentSpeed;
        UnityEngine.Vector3 velocityNew = new UnityEngine.Vector3(move.x, playerRigidbody.linearVelocity.y, move.z);
        playerRigidbody.linearVelocity = velocityNew;

        isGrounded = Physics.Raycast(transform.position, UnityEngine.Vector3.down, 1.1f, groundMask);
    }
}