using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteractio : MonoBehaviour
{
    public GameObject interactionKeyUI; // El UI de la tecla que se mostrará
    public Transform player; // El transform del jugador
    public float interactionDistance = 3f; // Distancia a la que el jugador puede interactuar

    private void Start()
    {
        // Asegúrate de que el UI de la tecla esté desactivado al principio
        interactionKeyUI.SetActive(false);
    }

    private void Update()
    {
        // Verifica la distancia entre el jugador y el NPC
        float distance = Vector3.Distance(player.position, transform.position);

        // Si el jugador está cerca, mostrar el icono de la tecla
        if (distance < interactionDistance)
        {
            interactionKeyUI.SetActive(true);

            // Hacer que la tecla se posicione encima del NPC
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 2, 0)); // Ajusta la altura según sea necesario
            interactionKeyUI.transform.position = screenPosition;

            // Si el jugador presiona la tecla "E", realizar la acción
            if (Input.GetKeyDown(KeyCode.E))
            {
                InteractWithNPC();
            }
        }
        else
        {
            // Si el jugador se aleja, ocultar el icono
            interactionKeyUI.SetActive(false);
        }
    }

    private void InteractWithNPC()
    {
        // Aquí va la lógica de interacción con el NPC
        Debug.Log("Interacción con NPC realizada.");
        // Por ejemplo, podrías abrir un diálogo, mostrar opciones, etc.
    }
}
