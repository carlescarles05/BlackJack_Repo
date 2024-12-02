using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteractio : MonoBehaviour
{
    public GameObject interactionKeyUI; // El UI de la tecla que se mostrar�
    public Transform player; // El transform del jugador
    public float interactionDistance = 3f; // Distancia a la que el jugador puede interactuar

    private void Start()
    {
        // Aseg�rate de que el UI de la tecla est� desactivado al principio
        interactionKeyUI.SetActive(false);
    }

    private void Update()
    {
        // Verifica la distancia entre el jugador y el NPC
        float distance = Vector3.Distance(player.position, transform.position);

        // Si el jugador est� cerca, mostrar el icono de la tecla
        if (distance < interactionDistance)
        {
            interactionKeyUI.SetActive(true);

            // Hacer que la tecla se posicione encima del NPC
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 2, 0)); // Ajusta la altura seg�n sea necesario
            interactionKeyUI.transform.position = screenPosition;

            // Si el jugador presiona la tecla "E", realizar la acci�n
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
        // Aqu� va la l�gica de interacci�n con el NPC
        Debug.Log("Interacci�n con NPC realizada.");
        // Por ejemplo, podr�as abrir un di�logo, mostrar opciones, etc.
    }
}
