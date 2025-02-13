using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscenaMario : MonoBehaviour
{
    // Nombre de la escena a la que deseas cambiar
    public string nombreEscena;

    // Se llama cuando otro collider entra en el trigger
    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra es el jugador
        if (other.CompareTag("Player"))
        {
            // Cambia a la escena especificada
            SceneManager.LoadScene(nombreEscena);
        }
    }
}
