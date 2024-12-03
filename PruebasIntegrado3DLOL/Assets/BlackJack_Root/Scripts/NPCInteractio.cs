using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCInteractio : MonoBehaviour
{
    public GameObject interactionIndicator; // El indicador (cubo) que se activa/desactiva
    private bool playerInRange = false; // Para saber si el jugador está en rango

    private void Start()
    {
        // Asegurarnos de que el indicador esté apagado al inicio
        if (interactionIndicator != null)
        {
            interactionIndicator.SetActive(false);
        }
        else
        {
            Debug.LogError("No se asignó el indicador en el Inspector.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que entra es el jugador
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactionIndicator != null)
            {
                interactionIndicator.SetActive(true); // Activar el indicador
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Verificar si el objeto que sale es el jugador
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactionIndicator != null)
            {
                interactionIndicator.SetActive(false); // Desactivar el indicador
            }
        }
    }
}
