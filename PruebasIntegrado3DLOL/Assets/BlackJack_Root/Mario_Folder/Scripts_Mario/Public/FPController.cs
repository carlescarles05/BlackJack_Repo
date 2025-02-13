using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
//
public class FPController : MonoBehaviour
{
    [Header("Player Body")]
    public Transform playerBody;


    [Header("Player Movement Speeds")]
    private float nextStepTime;
    private bool isMoving;
    [SerializeField] private float walkspeed = 5f;
    [SerializeField] private float sprintMultiplier = 2.0f;


    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity = 9.81f;

    [Header("Player Look Sensitivity")]
    public float mouseSensitivity = 2f;
    public float upDownRange;

    [Header("For the player Component References")]

    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera mainCamera;

    
    [Header(" New Input System ")]
    [SerializeField] private InputActionAsset gameInputActions;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private Vector2 moveInput;
    private Vector2 lookInput;

    //
    private float verticalRotation = 0f; //stores current x rotation
    private Vector3 currentMovement = Vector3.zero;
    private int lastPlayedIndex = -1;

    [Header("Sounds")]//
    [SerializeField] public AudioSource footstepSource;
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private float walkStepInterval = 0.5f;
    [SerializeField] private float sprintStepInterval = 0.3f;
    [SerializeField] private float velocityThreshold = 2.0f;

    
    private void Awake()
    {
        
        
        /*Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;*/
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
            if (characterController == null)
            {
                Debug.LogError("Character controller is missing on the gameObject");
            }
        }
        if (mainCamera == null) 
        {
            mainCamera = Camera.main;
            if (mainCamera == null) 
            {
                Debug.Log("No main Camera found in the scene");
            }
        }
        if (gameInputActions == null )
        {
            Debug.LogError("Input Actions assets not assigned in the inspector");
            return;
        }
        var actionMap = gameInputActions.FindActionMap("XboxControl");
        if (actionMap == null) 
        {
            Debug.LogError("ActionMap GameInputActionMap not found in InputActionAsset!");
            return;
        }
        //setup Input Actions
        moveAction = actionMap.FindAction("Move");
        lookAction = actionMap.FindAction("look");
        jumpAction = actionMap.FindAction("Jump");
        sprintAction = actionMap.FindAction("Sprint");
        if (moveAction == null || lookAction == null || jumpAction == null || sprintAction == null)
        {
            Debug.LogError("One or more actions(Move, Look, Jump, Sprint) are missing in the InputActionAsset");
            return;
        }
        moveAction.performed += context => moveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => moveInput = Vector2.zero;
        lookAction.performed += context => lookInput = context.ReadValue<Vector2>();
        lookAction.canceled += context => lookInput = Vector2.zero;
    }
    private void OnEnable()
    {
        moveAction?.Enable();
        lookAction?.Enable();
        jumpAction?.Enable();
        sprintAction?.Enable();
    }
    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        jumpAction?.Disable();
        sprintAction?.Disable();
    }
    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleFootsteps();
    }
    void HandleMovement() 
    {
        //Get
        

        float speedMultiplier = sprintAction.ReadValue<float>() >0 ? sprintMultiplier : 1f;
        //
        float verticalSpeed = moveInput.y * walkspeed * speedMultiplier;
        float horizontalSpeed = moveInput.x * walkspeed * speedMultiplier;

        Vector3 horizontalMovement = new Vector3(horizontalSpeed, 0, verticalSpeed); //local player  coordinates
        horizontalMovement = transform.rotation * horizontalMovement; //players is looking forward in world space
                                                                      // characterController.SimpleMove(speed);

        //////////////////////////////////////////////////////////////////////////////////////
        GravityandJumpHandler();
        currentMovement.x = horizontalMovement.x;
        currentMovement.z = horizontalMovement.z;
        characterController.Move(currentMovement * Time.deltaTime);
      
        isMoving = moveInput.y != 0 || moveInput.x != 0;
    }


    void GravityandJumpHandler()
    {
        if (characterController.isGrounded)
        {
            currentMovement.y = -0.5f;
            if (jumpAction?.triggered == true)
            {
                currentMovement.y = jumpForce;
            }
        }
        else
        {
            currentMovement.y -= gravity * Time.deltaTime;
        }
    }

        void HandleRotation()
    {
        float mouseXRotation = lookInput.x * mouseSensitivity; //*mouseSensitivity; get the players X coordinate from "Vertical" input stored in MouseXInput through MouseXRotation  
        transform.Rotate(0, mouseXRotation, 0); //x rotation on the y axis.no idea why.
        verticalRotation -= lookInput.y * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);


    }
    void HandleFootsteps()
    {
        float currentStepInterval = sprintAction.ReadValue<float>() > 0 ? sprintStepInterval : walkStepInterval;
        if (characterController.isGrounded && isMoving && Time.time > nextStepTime && characterController.velocity.magnitude > velocityThreshold)
        {
            //playfootsteps
            PlayFootStepSounds();
            nextStepTime = Time.time + currentStepInterval;
        }
    }

    void PlayFootStepSounds()
    {
        if (footstepSounds.Length == 0) return;
        int randomIndex = Random.Range(0,footstepSounds.Length);
       
            if (randomIndex == lastPlayedIndex && footstepSounds.Length >1)
            {
            randomIndex = (randomIndex +1) % footstepSounds.Length;
            }
        
        lastPlayedIndex = randomIndex;
        footstepSource.clip = footstepSounds[randomIndex];
        footstepSource.Play();
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
    //player movement with old input system

}//
