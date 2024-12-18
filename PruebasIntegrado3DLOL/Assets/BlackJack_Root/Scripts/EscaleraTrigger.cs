using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscaleraTrigger : MonoBehaviour
{
    // Referencia al objeto de texto en la UI
    public GameObject mensajeUI;

    // Se llama cuando otro collider entra en el trigger
    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra es el jugador
        if (other.CompareTag("Player"))
        {
            // Activa el mensaje en la UI
            if (mensajeUI != null)
            {
                mensajeUI.SetActive(true);
            }
            else
            {
                Debug.LogError("No se ha asignado el objeto de mensaje UI en el Inspector.");
            }
        }
    }

    // Se llama cuando otro collider sale del trigger
    private void OnTriggerExit(Collider other)
    {
        // Verifica si el objeto que sale es el jugador
        if (other.CompareTag("Player"))
        {
            // Desactiva el mensaje en la UI
            if (mensajeUI != null)
            {
                mensajeUI.SetActive(false);
            }
        }
    }
}
