using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void CambiarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    public void SalirDelJuego()
    {
        Application.Quit();
        Debug.Log("El juego se ha cerrado."); // Este mensaje aparecer� en la consola durante el modo de juego en el editor.
    }

    public void IraMenu()
    {
        SceneManager.LoadScene(0);
    }

    public GameObject exitConfirmationCanvas; // Arrastra el canvas de confirmaci�n desde el editor

    // Llamado cuando se presiona el bot�n "Salir del juego"
    public void OnExitButtonPressed()
    {
        // Activa el Canvas de confirmaci�n
        exitConfirmationCanvas.SetActive(true);
    }

    // Llamado cuando se presiona el bot�n "S�"
    public void ConfirmExit()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit(); // Cierra el juego (solo funciona en la versi�n compilada)
    }

    // Llamado cuando se presiona el bot�n "No"
    public void CancelExit()
    {
        // Desactiva el Canvas de confirmaci�n
        exitConfirmationCanvas.SetActive(false);
    }

}
