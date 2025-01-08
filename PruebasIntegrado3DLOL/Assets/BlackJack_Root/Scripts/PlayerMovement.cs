using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento
    public float mouseSensitivity = 100f; // Sensibilidad del ratón
    public Transform cameraTransform; // Transform de la cámara del jugador

    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float _velocity;
    private float _direction;
    private float verticalVelocity;            // Velocidad vertical

    private float pitch = 0f; // Rotación vertical de la cámara

    // Sonido de pasos
    public AudioClip[] footstepClips; // Clips de sonido de pasos
    public float stepInterval = 0.5f; // Intervalo entre pasos
    private float stepTimer; // Temporizador para los pasos
    private AudioSource audioSource; // Componente de audio
    public float jumpHeight = 2f;              // Altura de salto
    public float gravity = -9.81f;             // Fuerza de gravedad

    private CharacterController characterController; // Controlador de personaje

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Añadir AudioSource si no existe
        characterController = GetComponent<CharacterController>(); // Obtener el CharacterController
    }

    void Update()
    {
        MovePlayer();
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

    void RotateCamera()
    {
        // Entrada del ratón
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Girar al jugador horizontalmente (eje Y)
        transform.Rotate(Vector3.up * mouseX);

        // Rotación vertical de la cámara (eje X)
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f); // Limitar el ángulo de la cámara para evitar que gire completamente

        // Aplicar rotación a la cámara
        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
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

            // Eliminar o comentar esta parte para deshabilitar el salto
            /*
            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity); // Calcular velocidad de salto
            }
            */
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

    public void SetMovementEnabled(bool enabled)
    {
        characterController.enabled = enabled; // Activar o desactivar el CharacterController
    }
}
