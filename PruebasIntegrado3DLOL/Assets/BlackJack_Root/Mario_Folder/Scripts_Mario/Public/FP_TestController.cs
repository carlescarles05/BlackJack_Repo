using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FP_TestController : MonoBehaviour
{
    [Header("Player Body")]
    public Transform playerBody;
    [Header("Player Movement Speeds")]
    public float walkspeed = 5f;

    [Header("Player Look Sensitivity")]
    public float mouseSensitivity = 2f;
    public float upDownRange = 80.0f;

    [Header("For the player Component References")]
    /*public Transform cameraTransform;*/
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera mainCamera;
    /*private Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 lookInput;*/

    [Header(" Unity Old Input System ")]
    //[SerializeField] private InputActionAsset GameInput;
    [SerializeField] private string horizontalMoveInput = "Horizontal";
    [SerializeField] private string verticalMoveInput = "Vertical";
    //rotation mouse
    [SerializeField] private string MouseXInput = "Mouse X";
    [SerializeField] private string MouseYInput = "Mouse Y";
    private float xRotation = 0f; //stores current x rotation

    private void Start()
    {
        //  rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        //  rb.freezeRotation = true;
    }
    private void Update()
    {
        MovementHandler();
        RotationHandler();
    }

    /* public void OnMove(InputAction.CallbackContext context)
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
     }*/

    /* private void FixedUpdate()
     {
         // Handle player movement
         Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
         rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);
     }*/

    //player movement with old input system
    void MovementHandler()
    {
        float verticalSpeed = Input.GetAxis(verticalMoveInput) * walkspeed;
        float horizontalSpeed = Input.GetAxis(horizontalMoveInput) * walkspeed;

        Vector3 speed = new Vector3(horizontalSpeed, 0, verticalSpeed); //local player  coordinates
        speed = transform.rotation * speed; //players is looking forward in world space
        characterController.SimpleMove(speed);

    }
    void RotationHandler()
    {
        float mouseXRotation = Input.GetAxis(MouseXInput) * mouseSensitivity; //*mouseSensitivity; get the players X coordinate from "Vertical" input stored in MouseXInput through MouseXRotation  
        float mouseYRotation = Input.GetAxis(MouseYInput) * mouseSensitivity; // vertical move for camera pitch

        //apply vertical mouse rotation(cam pitch)
        xRotation -= mouseYRotation; //now i declare this variable that will be my marker numerical in this case ,for my input and then the machine will do its calculation between this and the vertical and horizontal coordinates
        xRotation -= Mathf.Clamp(xRotation, -upDownRange, upDownRange); //vertical botton - top bar CLAMP PARAMETERS help

        //apply the calculated rotation to the camera's  local rotation(pitch)
        mainCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //apply horizontal rotation to the players body(yaw)
        playerBody.Rotate(Vector3.up * mouseXRotation);
                                                    
    }

}
