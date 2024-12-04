using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BJManager : MonoBehaviour
{
    public EnemyAI enemyAI; // Referencia al script EnemyAI
    public int playerTotal = 0; // Puntos totales del jugador
    public List<GameObject> PlayerCards = new List<GameObject>(); // Cartas del jugador
    public Transform playerCardSpawnPoint; // Punto donde aparecen las cartas del jugador
    public GameObject cardPrefab; // Prefab de las cartas del jugador
    public int cardOffset = 30; // Espaciado entre cartas visibles del jugador

    public TextMeshProUGUI playerTotalText; // Referencia al texto del canvas
    private bool isGameOver = false;
    private const int maxPoints = 21; // Puntaje máximo (21)

    private void Start()
    {
        if (enemyAI == null)
        {
            enemyAI = FindObjectOfType<EnemyAI>();
            if (enemyAI == null)
            {
                Debug.LogError("EnemyAI no encontrado. Por favor, asigna el script EnemyAI en el Inspector.");
            }
        }

        UpdatePlayerTotalUI(); // Inicializar el texto en el canvas
    }

    public void PlayerHit() // Método para botón "Robar carta"
    {
        if (isGameOver)
        {
            Debug.LogWarning("El juego ha terminado. No puedes robar más cartas.");
            return;
        }

        // Generar una nueva carta para el jugador
        int newCard = GenerateCard();
        playerTotal += newCard;

        // Crear la carta visualmente
        GameObject card = Instantiate(cardPrefab, playerCardSpawnPoint);
        card.transform.localPosition += new Vector3(cardOffset * PlayerCards.Count, 0, 0);
        PlayerCards.Add(card);

        Debug.Log($"Carta del jugador: {newCard}");
        Debug.Log($"Total del jugador: {playerTotal}");

        // Actualizar el texto del canvas
        UpdatePlayerTotalUI();

        // Verificar si el jugador pierde
        if (playerTotal > maxPoints)
        {
            Debug.Log("¡Te pasaste de 21! Has perdido.");
            EndGame(false); // Jugador pierde
        }
    }

    public void PlayerStand() // Método para botón "Plantarse"
    {
        if (isGameOver)
        {
            Debug.LogWarning("El juego ya ha terminado.");
            return;
        }

        Debug.Log("El jugador se planta.");
        enemyAI.EnemyTurn(); // Comienza el turno del enemigo
    }

    public int GenerateCard()
    {
        return Random.Range(1, 11); // Generar una carta aleatoria entre 1 y 10
    }

    private void UpdatePlayerTotalUI()
    {
        if (playerTotalText != null)
        {
            // Mostrar el puntaje como "total actual / 21"
            playerTotalText.text = $"{playerTotal}/{maxPoints}";
        }
        else
        {
            Debug.LogError("PlayerTotalText no está asignado en el Inspector.");
        }
    }

    public void CompareScores()
    {
        Debug.Log("Comparando los puntajes...");

        if (playerTotal > maxPoints)
        {
            Debug.Log("¡El jugador se pasó de 21! Has perdido.");
            EndGame(false); // Jugador pierde
        }
        else if (enemyAI.enemyTotal > maxPoints)
        {
            Debug.Log("¡El enemigo se pasó de 21! Has ganado.");
            EndGame(true); // Jugador gana
        }
        else if (playerTotal > enemyAI.enemyTotal)
        {
            Debug.Log("¡Ganaste! El jugador tiene un puntaje mayor.");
            EndGame(true); // Jugador gana
        }
        else if (enemyAI.enemyTotal > playerTotal)
        {
            Debug.Log("¡Perdiste! El enemigo tiene un puntaje mayor.");
            EndGame(false); // Jugador pierde
        }
        else
        {
            Debug.Log("¡Es un empate!");
            EndGame(null); // Empate, si necesitas manejarlo
        }
    }

    public void EndGame(bool? playerWins)
    {
        isGameOver = true;

        if (playerWins == true)
        {
            Debug.Log("El jugador ganó la partida.");
        }
        else if (playerWins == false)
        {
            Debug.Log("El jugador perdió la partida.");
        }
        else
        {
            Debug.Log("El juego terminó en empate.");
        }

        // Aquí puedes implementar cualquier lógica adicional para reiniciar o terminar la partida.
    }
}
