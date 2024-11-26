using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SitOnObject : MonoBehaviour
{
    public Transform seatPoint;        // El punto donde el jugador debe ir
    public Canvas canvasObject;       // Referencia al Canvas
    public KeyCode interactKey = KeyCode.E;  // Tecla para interactuar con la silla

    private bool isSitting = false;   // ¿Está el jugador sentado?
    private Transform playerTransform;
    private PlayerMovement playerMovement;
    private CharacterController characterController;

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        playerMovement = playerTransform.GetComponent<PlayerMovement>();
        characterController = playerTransform.GetComponent<CharacterController>();

        // Asegúrate de que el Canvas esté desactivado al inicio
        if (canvasObject != null)
        {
            canvasObject.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!isSitting)
        {
            // Detectar la cercanía del jugador con la silla
            Collider[] nearbyObjects = Physics.OverlapSphere(playerTransform.position, 2f);
            foreach (Collider obj in nearbyObjects)
            {
                if (obj.CompareTag("Chair"))
                {
                    Debug.Log("Silla detectada. Presiona 'E' para sentarte.");
                    if (Input.GetKeyDown(interactKey))
                    {
                        SitDown(obj.transform);
                        break;
                    }
                }
            }
        }
    }

    void SitDown(Transform chair)
    {
        if (seatPoint == null)
        {
            Debug.LogError("No se ha asignado un SeatPoint en el Inspector.");
            return;
        }

        isSitting = true;

        // Desactivar el movimiento del jugador
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
        if (characterController != null)
        {
            characterController.enabled = false;
        }

        // Colocar al jugador en el SeatPoint
        playerTransform.position = seatPoint.position;

        // Activar el Canvas
        if (canvasObject != null)
        {
            canvasObject.gameObject.SetActive(true);
        }

        Debug.Log("El jugador ahora está sentado.");
    }
}
