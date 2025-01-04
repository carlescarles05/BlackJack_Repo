using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPController : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed = 5f;
    public float lookSensitivity = 2f;

    [Header("References")]
    public Transform cameraTransform;

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 lookInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Store movement input
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // Store look input
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // Perform jump on button press
        if (context.performed && IsGrounded())
        {
            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        // Handle player movement
        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);
    }

    private void Update()
    {
        // Handle camera rotation
        transform.Rotate(Vector3.up * lookInput.x * lookSensitivity);
        cameraTransform.localRotation = Quaternion.Euler(-lookInput.y * lookSensitivity, 0f, 0f);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}
