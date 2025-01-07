using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroDialogue : MonoBehaviour
{
    public GameObject introCanvas; // El canvas de introducci�n
    public GameObject introPanel;  // El panel dentro del canvas
    public TextMeshProUGUI introText; // El texto dentro del panel
    public string introSentence = "Bienvenido al juego."; // Frase de bienvenida
    public float textSpeed = 0.05f; // Velocidad de aparici�n del texto
    public float waitTimeAfterText = 2f; // Tiempo de espera tras completar el texto

    private PlayerMovement playerMovement; // Referencia al script de movimiento del jugador

    void Start()
    {
        // Buscar el componente de movimiento del jugador
        playerMovement = FindObjectOfType<PlayerMovement>();

        // Desactivar el movimiento del jugador al inicio
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        // Iniciar la secuencia de introducci�n
        StartCoroutine(StartIntro());
    }

    IEnumerator StartIntro()
    {
        // Activar el panel de introducci�n
        introPanel.SetActive(true);

        // Mostrar el texto poco a poco
        yield return StartCoroutine(TypeSentence(introSentence));

        // Esperar unos segundos tras completar el texto
        yield return new WaitForSeconds(waitTimeAfterText);

        // Desactivar el canvas de introducci�n
        introCanvas.SetActive(false);

        // Reactivar el movimiento del jugador
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        introText.text = ""; // Limpiar el texto actual
        foreach (char letter in sentence)
        {
            introText.text += letter; // A�adir letra por letra
            yield return new WaitForSeconds(textSpeed); // Esperar antes de la siguiente letra
        }
    }
}
