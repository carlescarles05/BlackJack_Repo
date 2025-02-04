using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento
    public float mouseSensitivity = 100f; // Sensibilidad del ratón
    public Transform cameraTransform; // Transform de la cámara del jugador

    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float verticalVelocity;            // Velocidad vertical

    private float pitch = 0f; // Rotación vertical de la cámara
    private float yaw = 0f;
    // Sonido de pasos
    public AudioClip[] footstepClips; // Clips de sonido de pasos
    public float stepInterval = 0.5f; // Intervalo entre pasos
    private float stepTimer; // Temporizador para los pasos
    private AudioSource audioSource; // Componente de audio
    public float jumpHeight = 2f;              // Altura de salto
    public float gravity = -9.81f;             // Fuerza de gravedad
    public bool isSitting;
    public bool canMove;

    private CharacterController characterController; // Controlador de personaje
    private AdivinaLaCarta adivinaLaCarta;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Añadir AudioSource si no existe
        characterController = GetComponent<CharacterController>(); // Obtener el CharacterController
        isSitting = false;
        if (GameManager.Instance != null)
        {
            transform.position = GameManager.Instance.GetPlayerPosition();
        }
    }

    void Update()
    {
        if (canMove)
        {
            MovePlayer();
            
        }
        RotateCamera();
        HandleFootsteps();
        ApplyGravity();
    }

    void MovePlayer()
    {
        // Entrada de teclado
        float horizontal = Input.GetAxis("Horizontal"); // A/D o flechas izquierda/derecha
        float vertical = Input.GetAxis("Vertical");     // W/S o flechas arriba/abajo

        // Dirección de movimiento relativa a la cámara
        Vector3 movement = (transform.right * horizontal + transform.forward * vertical).normalized;

        // Mover al jugador
        characterController.Move(movement * moveSpeed * Time.deltaTime);
    }
    
    public void RotateCamera()
    {
        if ((Input.GetMouseButton(1)))
        {
            // Bloquear el cursor al centro de la pantalla
            Cursor.lockState = CursorLockMode.Locked;
            // Hacer el cursor invisible
            Cursor.visible = false;
            // Entrada del ratón

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            if (!isSitting)
            {
                // Control normal del jugador (sin restricciones adicionales)
                yaw += mouseX; // Acumulamos la rotación horizontal
                pitch -= mouseY; // Acumulamos la rotación vertical
                pitch = Mathf.Clamp(pitch, -90f, 90f); // Limitar la rotación vertical entre -90° y 90°

                // Aplicar rotaciones
                transform.localRotation = Quaternion.Euler(0f, yaw, 0f); // Girar al jugador (horizontalmente)
                cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f); // Rotar la cámara (verticalmente)
            }
            else
            {
                // Restricción de rotación cuando el personaje está sentado
                yaw += mouseX; // Acumulamos la rotación horizontal
                pitch -= mouseY; // Acumulamos la rotación vertical

                // Limitar la rotación vertical entre -90° y 90° (mirar hacia arriba y hacia abajo)
                pitch = Mathf.Clamp(pitch, -60f, 60f);

                // Limitar la rotación horizontal (yaw) dentro de un rango específico (por ejemplo, -45° a 45°)
                yaw = Mathf.Clamp(yaw, 130f, 190f);

                // Aplicar rotaciones con límites
                transform.localRotation = Quaternion.Euler(0f, yaw, 0f); // Rotar al jugador (horizontalmente, limitado)
                cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f); // Rotar la cámara (verticalmente)
            }
        }
        else
        {
            // Bloquear el cursor al centro de la pantalla
            Cursor.lockState = CursorLockMode.None;
            // Hacer el cursor invisible
            Cursor.visible = true;
        }


    }

    void HandleFootsteps()
    {
        bool isMoving = (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && IsOnGround();

        if (isMoving)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= stepInterval)
            {
                PlayFootstep();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    bool IsOnGround()
    {
        // Radio de la esfera para el SphereCast
        float sphereRadius = 0.2f;
        // Distancia ligeramente mayor que el radio para asegurar la detección
        float sphereDistance = characterController.height / 2 + 0.1f;

        // Lanza un SphereCast desde el centro del jugador hacia abajo
        return Physics.SphereCast(transform.position, sphereRadius, Vector3.down, out RaycastHit hit, sphereDistance);
    }

    void PlayFootstep()
    {
        if (footstepClips.Length > 0)
        {
            int clipIndex = Random.Range(0, footstepClips.Length); // Selecciona un clip aleatorio
            audioSource.PlayOneShot(footstepClips[clipIndex]); // Reproduce el clip
        }
    }

    void ApplyGravity()
    {
        // Si el personaje está en el suelo, resetear velocidad vertical y permitir salto
        if (characterController.isGrounded)
        {
            verticalVelocity = -0.5f; // Mantenerlo pegado al suelo
        }
        else
        {
            // Aplicar gravedad si el personaje no está en el suelo
            verticalVelocity += gravity * Time.deltaTime;
        }

        // Aplicar velocidad vertical al movimiento
        Vector3 gravityMovement = new Vector3(0f, verticalVelocity, 0f);
        characterController.Move(gravityMovement * Time.deltaTime);

        if (characterController.isGrounded)
        {
            verticalVelocity = -0.5f; // Mantener al personaje pegado al suelo
        }
    }
    /*public float moveSpeed = 5f;
    public float mouseSensitivity = 100f;
    public Transform cameraTransform;

    private float verticalVelocity;
    private CharacterController characterController;
    private GameInputActions playerInput; // New Input System

    private Vector2 moveInput;  // Stores movement input
    private Vector2 lookInput;  // Stores camera look input

    private float pitch = 0f;
    private float yaw = 0f;
    //
    public AudioClip[] footstepClips;
    public float stepInterval = 0.5f;
    private float stepTimer;
    private AudioSource audioSource;
    public float jumpHeight = 2f;
    public bool isSitting;
    public bool canMove = true;

    void Awake()
    {
        playerInput = new GameInputActions(); // Initialize Input System
    }

    /*void OnEnable()
    {
        playerInput.XboxControl.Enable();
        playerInput.XboxControl.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInput.XboxControl.Move.canceled += ctx => moveInput = Vector2.zero;

        playerInput.XboxControl.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerInput.XboxControl.Look.canceled += ctx => lookInput = Vector2.zero;

      //  playerInput.XboxControl.Jump.performed += ctx => Jump();
    }

    void OnDisable()
    {
        playerInput.XboxControl.Disable();
    }

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();
        isSitting = false;
    }

    void Update()
    {
        if (canMove)
        {
            MovePlayer();
        }
        RotateCamera();
        HandleFootsteps();
        ApplyGravity();
    }

    void MovePlayer()
    {
        // Convert input to movement direction
        Vector3 movement = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized;
        characterController.Move(movement * moveSpeed * Time.deltaTime);
    }

    public void RotateCamera()
    {
        if (Mouse.current.rightButton.isPressed) // Right-click check
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            yaw += lookInput.x * mouseSensitivity * Time.deltaTime;
            pitch -= lookInput.y * mouseSensitivity * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, -90f, 90f);

            transform.localRotation = Quaternion.Euler(0f, yaw, 0f);
            cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void HandleFootsteps()
    {
        bool isMoving = (moveInput.x != 0 || moveInput.y != 0) && IsOnGround();

        if (isMoving)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= stepInterval)
            {
                PlayFootstep();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    bool IsOnGround()
    {
        float sphereRadius = 0.2f;
        float sphereDistance = characterController.height / 2 + 0.1f;
        return Physics.SphereCast(transform.position, sphereRadius, Vector3.down, out _, sphereDistance);
    }

    void PlayFootstep()
    {
        if (footstepClips.Length > 0)
        {
            int clipIndex = Random.Range(0, footstepClips.Length);
            audioSource.PlayOneShot(footstepClips[clipIndex]);
        }
    }

    void ApplyGravity()
    {
        if (characterController.isGrounded)
        {
            verticalVelocity = -0.5f;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        characterController.Move(new Vector3(0f, verticalVelocity, 0f) * Time.deltaTime);
    }

    void Jump()
    {
        if (IsOnGround())
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
        }
    }*/
}
