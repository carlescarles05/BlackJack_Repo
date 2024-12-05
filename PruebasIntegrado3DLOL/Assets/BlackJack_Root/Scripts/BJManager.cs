using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BJManager : MonoBehaviour
{
    public EnemyAI enemyAI; // Referencia al script EnemyAI
    public int playerTotal = 0; // Puntos totales del jugador
    public List<GameObject> playerCards = new List<GameObject>(); // Cartas del jugador
    public List<GameObject> enemyCards = new List<GameObject>(); // Cartas del enemigo
    public Transform playerCardSpawnPoint; // Punto donde aparecen las cartas del jugador
    public Transform enemyCardSpawnPoint; // Punto donde aparecen las cartas del enemigo
    public GameObject cardPrefab; // Prefab de las cartas
    public int cardOffset = 30; // Espaciado entre cartas visibles
    public TextMeshProUGUI playerTotalText; // Referencia al texto del canvas
    public Button hitButton; // Botón "Robar carta"
    public Button standButton; // Botón "Plantarse"

    private bool isGameOver = false;
    private const int maxPoints = 21; // Puntaje máximo (21)

    public enum Turn
    {
        Player,
        Enemy
    }

    public Turn currentTurn = Turn.Player;
    private bool isPlayerTurn;
    private int enemyTotal;

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

        hitButton.onClick.AddListener(PlayerHit);
        standButton.onClick.AddListener(PlayerStand);

        UpdatePlayerTotalUI();
    }

    public void PlayerHit()
    {
        if (currentTurn != Turn.Player) return;

        int cardValue = GenerateCard();
        playerTotal += cardValue;
        UpdatePlayerTotalUI();

        GameObject card = Instantiate(cardPrefab, playerCardSpawnPoint);
        card.transform.localPosition += new Vector3(cardOffset * playerCards.Count, 0, 0);
        playerCards.Add(card);

        if (playerTotal > maxPoints)
        {
            Debug.Log("¡Te pasaste de 21!");
            EndGame(false);
            return;
        }

        EndPlayerTurn();
    }

    public void PlayerStand()
    {
        Debug.Log("Jugador se planta.");
        SetPlayerTurn(false); // Cambiar el turno al enemigo
    }

    public int GenerateCard()
    {
        return Random.Range(1, 11); // Cambia a (1, 12) si quieres incluir el 11
    }

    private void EndPlayerTurn()
    {
        currentTurn = Turn.Enemy;
        StartCoroutine(EnemyTurnRoutine());
    }

    private IEnumerator EnemyTurnRoutine()
    {
        Debug.Log("Turno del enemigo iniciado.");

        while (enemyTotal < 17)
        {
            yield return new WaitForSeconds(1f);

            int cardValue = GenerateCard();
            enemyTotal += cardValue;

            UpdateEnemyTotalUI();
            Debug.Log($"El enemigo pidió una carta: {cardValue}. Total del enemigo: {enemyTotal}");

            if (enemyTotal > 21)
            {
                Debug.Log("¡El enemigo se pasó de 21!");
                EndGame(true); // Jugador gana
                yield break;
            }
        }

        Debug.Log("El enemigo se planta.");
        CompareScores();
    }

    private void UpdateEnemyTotalUI()
    {
        if (enemyAI.enemyTotalText != null)
        {
            enemyAI.enemyTotalText.text = $"{enemyTotal}/21";
        }
        else
        {
            Debug.LogError("enemyTotalText no está asignado en el Inspector.");
        }
    }

    private void UpdatePlayerTotalUI()
    {
        if (playerTotalText != null)
        {
            playerTotalText.text = $"{playerTotal}/21";
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
            EndGame(false); // Jugador pierde
        }
        else if (enemyTotal > maxPoints)
        {
            EndGame(true); // Jugador gana
        }
        else if (playerTotal > enemyTotal)
        {
            EndGame(true); // Jugador gana
        }
        else if (enemyTotal > playerTotal)
        {
            EndGame(false); // Jugador pierde
        }
        else
        {
            EndGame(null); // Empate
        }
    }

    private void SetPlayerTurn(bool isTurn)
    {
        isPlayerTurn = isTurn;
        hitButton.interactable = isTurn;
        standButton.interactable = isTurn;

        if (!isTurn && !isGameOver)
        {
            Debug.Log("Turno del enemigo.");
            enemyAI.EnemyTurn();
        }
    }

    public void EndGame(bool? playerWins)
    {
        isGameOver = true;

        hitButton.interactable = false;
        standButton.interactable = false;

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
    }
}
