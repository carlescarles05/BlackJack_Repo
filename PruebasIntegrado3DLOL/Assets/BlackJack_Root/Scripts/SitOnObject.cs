using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SitOnObject : MonoBehaviour
{
    public Transform seatPoint;          // Punto donde el jugador debe sentarse
    public GameObject canvasObject;      // Referencia al Canvas que aparecerá sobre la mesa
    public KeyCode interactKey = KeyCode.E;  // Tecla para interactuar con la silla

    private Transform playerTransform;   // Transform del jugador
    private PlayerMovement playerMovement; // Script de movimiento del jugador
    private bool isSitting = false;      // ¿Está el jugador sentado?

    void Start()
    {
        // Obtener el transform del jugador y su script de movimiento
        playerTransform = GameObject.FindWithTag("Player").transform;
        playerMovement = playerTransform.GetComponent<PlayerMovement>();

        // Asegurarse de que el canvas esté desactivado inicialmente
        if (canvasObject != null)
        {
            canvasObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!isSitting)
        {
            // Detectar cercanía del jugador con la silla
            if (Vector3.Distance(playerTransform.position, seatPoint.position) < 2f)
            {
                Debug.Log("Silla detectada. Presiona 'E' para sentarte.");
                if (Input.GetKeyDown(interactKey))
                {
                    SitDown(); // Llamar a la función para sentarse
                }
            }
        }
    }

    void SitDown()
    {
        isSitting = true;

        // Desactivar el movimiento del jugador
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        // Posicionar al jugador en el SeatPoint
        playerTransform.position = seatPoint.position;

        // Mostrar el Canvas sobre la mesa
        if (canvasObject != null)
        {
            canvasObject.SetActive(true);
        }

        Debug.Log("El jugador ahora está sentado y el Canvas aparece sobre la mesa.");
    }
}
