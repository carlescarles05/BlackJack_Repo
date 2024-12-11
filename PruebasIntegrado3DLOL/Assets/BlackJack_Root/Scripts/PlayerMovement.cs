using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento
    public float mouseSensitivity = 100f; // Sensibilidad del ratón
    public Transform cameraTransform; // Transform de la cámara del jugador

    private float pitch = 0f; // Rotación vertical de la cámara

    // Sonido de pasos
    public AudioClip[] footstepClips; // Clips de sonido de pasos
    public float stepInterval = 0.5f; // Intervalo entre pasos
    private float stepTimer; // Temporizador para los pasos
    private AudioSource audioSource; // Componente de audio

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
        // Verificar si el jugador se está moviendo
        bool isMoving = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;

        if (isMoving)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= stepInterval)
            {
                PlayFootstep();
                stepTimer = 0f; // Reinicia el temporizador
            }
        }
        else
        {
            stepTimer = 0f; // Reinicia si el jugador está quieto
        }
    }

    void PlayFootstep()
    {
        if (footstepClips.Length > 0)
        {
            int clipIndex = Random.Range(0, footstepClips.Length); // Selecciona un clip aleatorio
            audioSource.PlayOneShot(footstepClips[clipIndex]); // Reproduce el clip
        }
    }
}
