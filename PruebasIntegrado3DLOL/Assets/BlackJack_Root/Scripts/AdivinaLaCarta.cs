using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AdivinaLaCarta : MonoBehaviour
{
    public TextMeshProUGUI resultadoText; // Texto para mostrar el resultado
    public GameObject canvasJuego; // Referencia al Canvas que debe activarse
    private int cartaCorrecta; // Carta correcta en forma de n�mero
    private Transform playerTransform;
    private bool jugadorCerca = false; // Detecta si el jugador est� en el �rea
    private bool isMinigameActive = false;
    public BJManager bM; // Referencia al script Cronometro
    public bool canPlay;
    public TextMeshProUGUI roundsTotalText;
    public PlayerMovement playerMovement;
    public GameObject InGame;
    public GameObject EndGame;

    public KeyCode interactKey = KeyCode.E; // Tecla de interacci�n

    private List<string> cartas = new List<string>
    {
        "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11"
    };

    public int roundCount = 1; // Contador de rondas
    private const int maxRounds = 10; // N�mero m�ximo de rondas

    public void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        bM = BJManager.Instance;
        canvasJuego.SetActive(false); // Asegura que el Canvas est� oculto al inicio
        SeleccionarCartaAleatoria();
    }

    private void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(interactKey))
        {
            if (canPlay)
            {
                Playgame();
                playerMovement.canMove = false;
            }
            else
            {
                Debug.Log("YA HAS JUGADO PICHON");
            }     
        }
    }

    void Playgame()
    {
        if (!canPlay) return;
        canvasJuego.SetActive(true); // Activa el Canvas al presionar E
    }
    

    private void SeleccionarCartaAleatoria()
    {
        int index = Random.Range(0, cartas.Count);
        cartaCorrecta = int.Parse(cartas[index]); // Convertir el texto a n�mero
        resultadoText.text = "�Cu�l es la carta correcta?";
    }

    public void VerificarCarta(string cartaSeleccionada)
    {
        int numeroSeleccionado = int.Parse(cartaSeleccionada);
        int diferencia = Mathf.Abs(cartaCorrecta - numeroSeleccionado); // Calcula qu� tan cerca est�

        if (numeroSeleccionado == cartaCorrecta)
        {
            resultadoText.text = "�Correcto! Era " + cartaCorrecta;
            bM.AddTime(20); // Si es exacta, suma 10 a�os
        }
        else
        {
            if (diferencia == 1)
            {
                resultadoText.text = "�Casi! Solo fallaste por 1 n�mero.";
                bM.AddTime(10); // Si est� a 1 n�mero de diferencia, suma 5 a�os
            }
            else if (diferencia <= 3)
            {
                resultadoText.text = "Cerca, sigue intentando.";
                bM.AddTime(5); // Si est� a 3 n�meros o menos, suma 3 a�os
            }
            else
            {
                resultadoText.text = "Lejos, intenta de nuevo.";
                bM.AddTime(0); // Si est� muy lejos, solo suma 1 a�o
            }
        }

        roundCount++;

        UpdateRoundsText();

        // Reiniciar el juego despu�s de 2 segundos
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
            jugadorCerca = true; // Detecta que el jugador est� cerca
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false; // El jugador sale del �rea
            canvasJuego.SetActive(false); // Oculta el Canvas al salir
        }
    }

    public void ExitButton()
    {
          // Desactiva permanentemente el juego
        playerMovement.canMove = true;
        jugadorCerca = false;
        isMinigameActive = false;
        canvasJuego.SetActive(false); // Asegura que el Canvas se cierre
    }

    public void UpdateRoundsText()
    {
        if (canPlay && roundsTotalText != null)
        {
            if (roundCount < 10) roundsTotalText.text = "ROUND  " + roundCount + "/10";
            else
            {
                canPlay = false;
                InGame.SetActive(false);
                EndGame.SetActive(true);
            }
            Debug.Log("Se suma una ronda al minijuego");
            Playgame();
            playerMovement.canMove = false;
        }
    }
}
