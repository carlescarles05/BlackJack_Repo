using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AdivinaLaCarta : MonoBehaviour
{
    public TextMeshProUGUI resultadoText; // Texto para mostrar el resultado
    public GameObject canvasJuego; // Referencia al Canvas que debe activarse
    private string cartaCorrecta; // Carta que el jugador debe adivinar
    private Transform playerTransform;
    private bool jugadorCerca = false; // Detecta si el jugador está en el área

    public KeyCode interactKey = KeyCode.E; // Tecla de interacción

    private List<string> cartas = new List<string> { "carta1", "carta2", "carta3", "carta4", "carta5", "carta6", "carta7", "carta8", "carta9", "carta10", "carta11" };

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        canvasJuego.SetActive(false); // Asegura que el Canvas esté oculto al inicio
        SeleccionarCartaAleatoria();
    }

    private void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(interactKey))
        {
            canvasJuego.SetActive(true); // Activa el Canvas al presionar E
        }
    }

    private void SeleccionarCartaAleatoria()
    {
        int index = Random.Range(0, cartas.Count);
        cartaCorrecta = cartas[index];
        resultadoText.text = "¿Cuál es la carta correcta?";
    }

    public void VerificarCarta(string cartaSeleccionada)
    {
        if (cartaSeleccionada == cartaCorrecta)
        {
            resultadoText.text = "¡Correcto! Era " + cartaCorrecta;
        }
        else
        {
            resultadoText.text = "Incorrecto, intenta de nuevo.";
        }

        // Reiniciar el juego después de 2 segundos
        Invoke("ReiniciarJuego", 2f);
    }

    private void ReiniciarJuego()
    {
        SeleccionarCartaAleatoria(); // Selecciona una nueva carta correcta
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true; // Detecta que el jugador está cerca
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false; // El jugador sale del área
            canvasJuego.SetActive(false); // Oculta el Canvas al salir
        }
    }
}
