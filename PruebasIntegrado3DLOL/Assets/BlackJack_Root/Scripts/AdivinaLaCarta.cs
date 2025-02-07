using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AdivinaLaCarta : MonoBehaviour
{
    public TextMeshProUGUI resultadoText; // Texto para mostrar el resultado
    public GameObject canvasJuego; // Referencia al Canvas que debe activarse
    private bool triggerDesactivado = false;
    public TextMeshProUGUI text;
    private int cartaCorrecta; // Carta correcta en forma de número
    private Transform playerTransform;
    public bool jugadorCerca = false; // Detecta si el jugador está en el área
    private bool isMinigameActive = false;
    public BJManager bM; // Referencia al script Cronometro
    public bool canPlay;
    public TextMeshProUGUI roundsTotalText;
    public PlayerMovement playerMovement;
    public GameObject InGame;
    public GameObject EndGame;

    public List<Button> botonesCartas; // Lista de botones en el juego

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
        text.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(interactKey))
        {
            if (canPlay)
            {
                Playgame();
                playerMovement.canMove = false;
                text.gameObject.SetActive(false);
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
        cartaCorrecta = int.Parse(cartas[index]); // Convertir el texto a número
        resultadoText.text = "¿Cuál es la carta correcta?";
    }

    public void VerificarCarta(string cartaSeleccionada)
    {
        // Desactivar botones temporalmente
        StartCoroutine(DesactivarBotonesTemporalmente());

        int numeroSeleccionado = int.Parse(cartaSeleccionada);
        int diferencia = Mathf.Abs(cartaCorrecta - numeroSeleccionado); // Calcula qué tan cerca está

        if (numeroSeleccionado == cartaCorrecta)
        {
            resultadoText.text = "¡Correcto! Era " + cartaCorrecta;
            bM.AddTime(100); // Si es exacta, suma 10 años

        }
        else
        {
            if (diferencia == 1)
            {
                resultadoText.text = "¡Casi! Solo fallaste por 1 número.";
                bM.AddTime(50); // Si está a 1 número de diferencia, suma 5 años

            }
            else if (diferencia <= 3)
            {
                resultadoText.text = "Cerca, sigue intentando.";
                bM.AddTime(25); // Si está a 3 números o menos, suma 3 años

            }
            else
            {
                resultadoText.text = "Lejos, intenta de nuevo.";
                bM.AddTime(0); // Si está muy lejos, solo suma 1 año

            }
        }

        roundCount++;

        UpdateRoundsText();

        StartCoroutine(ResaltarBotonCorrecto());

        // Reiniciar el juego después de 2 segundos
        Invoke("ReiniciarJuego", 5f);
    }

    private IEnumerator DesactivarBotonesTemporalmente()
    {
        // Desactivar botones
        foreach (Button btn in botonesCartas)
        {
            btn.interactable = false;
        }

        // Esperar 2 segundos
        yield return new WaitForSeconds(5f);

        // Reactivar botones
        foreach (Button btn in botonesCartas)
        {
            btn.interactable = true;
        }
    }

    private IEnumerator ResaltarBotonCorrecto()
    {
        Button botonCorrecto = ObtenerBotonPorNumero(cartaCorrecta);
        Color originalColor = botonCorrecto.image.color;

        botonCorrecto.image.color = Color.red; // Iluminar en rojo
        yield return new WaitForSeconds(2f); // Esperar 2 segundos
        botonCorrecto.image.color = originalColor; // Restaurar el color original
    }

    private Button ObtenerBotonPorNumero(int numero)
    {
        foreach (Button boton in botonesCartas)
        {
            if (boton.GetComponentInChildren<TextMeshProUGUI>().text == numero.ToString())
            {
                return boton;
            }
        }
        return null;
    }

    private void ReiniciarJuego()
    {
        SeleccionarCartaAleatoria(); // Selecciona una nueva carta correcta
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggerDesactivado)
        {
            jugadorCerca = true; // Detecta que el jugador está cerca
            //interaction.SetActive(true);
            text.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false; // El jugador sale del área
            canvasJuego.SetActive(false); // Oculta el Canvas al salir
            //interaction.SetActive(false);
            text.gameObject.SetActive(false);
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
                roundsTotalText.text = "ROUND  " + roundCount + "/10";
                canPlay = false;
                StartCoroutine(FinalizarJuego()); // Espera 5 segundos antes de terminar
                text.gameObject.SetActive(false);
                triggerDesactivado = true;
            }
            Debug.Log("Se suma una ronda al minijuego");
            Playgame();
            playerMovement.canMove = false;
        }
    }

    private IEnumerator FinalizarJuego()
    {
        Debug.Log("El juego terminará en 5 segundos...");
        yield return new WaitForSeconds(5f); // Espera 5 segundos

        InGame.SetActive(false);  // Oculta el panel del juego
        EndGame.SetActive(true);  // Muestra el panel de fin del juego

        Debug.Log("El juego ha terminado.");
    }
}
