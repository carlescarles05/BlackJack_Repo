using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCInteractio : MonoBehaviour
{
    public TextMeshProUGUI interactionKeyText; // Asignar el texto de la tecla en el inspector
    public GameObject interactionKeyBackground; // Asignar la imagen que aparece detrás del texto
    public Transform player; // El transform del jugador
    private bool playerInRange = false; // Flag para saber si el jugador está en rango

    private void Start()
    {
        // Asegurarse de que el texto y la imagen de la tecla estén desactivados inicialmente
        if (interactionKeyText != null)
        {
            interactionKeyText.gameObject.SetActive(false); // Desactiva el texto "E" al inicio
        }
        else
        {
            Debug.LogError("No se ha asignado el objeto 'interactionKeyText' en el Inspector.");
        }

        if (interactionKeyBackground != null)
        {
            interactionKeyBackground.SetActive(false); // Desactiva la imagen al inicio
        }
        else
        {
            Debug.LogError("No se ha asignado el objeto 'interactionKeyBackground' en el Inspector.");
        }
    }

    private void Update()
    {
        // Solo mostrar la tecla de interacción si el jugador está dentro del rango
        if (playerInRange)
        {
            // Colocar el texto y la imagen encima del NPC (ajustar la altura si es necesario)
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 2, 0));
            interactionKeyText.transform.position = screenPosition;
            interactionKeyBackground.transform.position = screenPosition;
            

            // Verificar si el jugador presiona la tecla "E"
            if (Input.GetKeyDown(KeyCode.E))
            {
                InteractWithNPC();
            }
        }
    }

    // Se llama cuando el jugador entra en el trigger
    private void OnTriggerEnter(Collider other)
    {
        // Verificamos si el objeto que entra es el jugador (asegúrate de que el tag esté configurado correctamente)
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador ha entrado en el trigger");
            playerInRange = true;
            interactionKeyText.gameObject.SetActive(true);
            interactionKeyBackground.SetActive(true);
        }
    }

    // Se llama cuando el jugador sale del trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false; // El jugador ha salido del rango
            interactionKeyText.gameObject.SetActive(false); // Ocultar el texto de la tecla
            interactionKeyBackground.SetActive(false); // Ocultar la imagen de la tecla
        }
    }

    private void InteractWithNPC()
    {
        // Lógica para interactuar con el NPC (por ejemplo, abrir un diálogo)
        Debug.Log("Interacción con el NPC.");
    }
}
