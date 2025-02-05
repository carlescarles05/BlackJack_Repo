using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AdivinaLaCarta : MonoBehaviour
{
    public TextMeshProUGUI resultadoText; // Texto para mostrar el resultado
    public GameObject canvasJuego; // Referencia al Canvas que debe activarse
    private int cartaCorrecta; // Carta correcta en forma de número
    private Transform playerTransform;
    private bool jugadorCerca = false; // Detecta si el jugador está en el área
    private bool isMinigameActive = false;
    public BJManager bM; // Referencia al script Cronometro
    public bool canPlay;
    public TextMeshProUGUI roundsTotalText;
    public PlayerMovement playerMovement;

    public KeyCode interactKey = KeyCode.E; // Tecla de interacción

    private List<string> cartas = new List<string>
    {
        "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11"
    };

    public int roundCount = 1; // Contador de rondas
    private const int maxRounds = 10; // Número máximo de rondas

    public void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        bM = BJManager.Instance;
        canvasJuego.SetActive(false); // Asegura que el Canvas esté oculto al inicio
        SeleccionarCartaAleatoria();
    }

    private void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(interactKey))
        {
            Playgame();
            playerMovement.canMove = false;
        }
    }

    void Playgame()
    {
        if (canPlay == false) return;
        canvasJuego.SetActive(true); // Activa el Canvas al presionar E
    }
    

    private void SeleccionarCartaAleatoria()
    {
        int index = Random.Range(0, cartas.Count);
        cartaCorrecta = int.Parse(cartas[index]); // Convertir el texto a número
        resultadoText.text = "¿Cuál es la carta correcta?";
    }

    public void VerificarCarta(string cartaSeleccionada)
    {
        int numeroSeleccionado = int.Parse(cartaSeleccionada);
        int diferencia = Mathf.Abs(cartaCorrecta - numeroSeleccionado); // Calcula qué tan cerca está

        if (numeroSeleccionado == cartaCorrecta)
        {
            resultadoText.text = "¡Correcto! Era " + cartaCorrecta;
            bM.AddTime(20); // Si es exacta, suma 10 años
        }
        else
        {
            if (diferencia == 1)
            {
                resultadoText.text = "¡Casi! Solo fallaste por 1 número.";
                bM.AddTime(10); // Si está a 1 número de diferencia, suma 5 años
            }
            else if (diferencia <= 3)
            {
                resultadoText.text = "Cerca, sigue intentando.";
                bM.AddTime(5); // Si está a 3 números o menos, suma 3 años
            }
            else
            {
                resultadoText.text = "Lejos, intenta de nuevo.";
                bM.AddTime(0); // Si está muy lejos, solo suma 1 año
            }
        }

        roundCount++;

        UpdateRoundsText();

        // Reiniciar el juego después de 2 segundos
        Invoke("ReiniciarJuego", 5f);
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

    public void ExitButton()
    {
        canPlay = false;
        playerMovement.canMove = true;
    }

    public void UpdateRoundsText()
    {
        if (roundsTotalText != null)
        {
            if (roundCount <= 10) roundsTotalText.text = "ROUND  " + roundCount + "/10";
            Debug.Log("Se suma una ronda al minijuego");
        }
        else
        {
            roundsTotalText.text = "FIN DEL JUEGO";
            // forzar el salir
            // bloquear la maquina
        }
    }
}
