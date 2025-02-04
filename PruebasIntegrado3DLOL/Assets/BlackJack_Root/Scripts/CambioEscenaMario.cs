using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscenaMario : MonoBehaviour
{
    //public string nombreEscena; // Nombre de la escena a cargar
    
    private bool jugadorCerca = false; // Variable para detectar si el jugador está en el trigger

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true; // Activa la bandera cuando el jugador entra
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false; // Desactiva la bandera cuando el jugador sale
        }
    }

    private void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(1); // Cambia de escena al presionar 'E'
        }
    }
}
