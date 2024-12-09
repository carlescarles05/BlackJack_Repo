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
    public bool isPlayerStanding = false;
    public BJManager bjManager; // Asegúrate de tener esta variable pública en el script

    private bool isGameOver = false;
    private const int maxPoints = 21; // Puntaje máximo (21)
    private int enemyTotal;

    [SerializeField] private EnemyAI EnemyAI;  // Si prefieres mantener la variable privada

    public enum Turn
    {
        Player,
        Enemy
    }

    public Turn currentTurn = Turn.Player;

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
        StartPlayerTurn(); // Asegurarnos de que empiece el turno del jugador
    }

    public void PlayerHit()
    {
        if (currentTurn != Turn.Player) return;

        int cardValue = GenerateCard();
        playerTotal += cardValue; // Acumular el valor de la carta al total del jugador
        UpdatePlayerTotalUI();

        GameObject card = Instantiate(cardPrefab, playerCardSpawnPoint);
        card.transform.localPosition += new Vector3(cardOffset * playerCards.Count, 0, 0);
        playerCards.Add(card);

        if (playerTotal > maxPoints)
        {
            Debug.Log("¡Te pasaste de 21!");
            EndGame(false); // Finaliza el juego si el jugador se pasa de 21
            return;
        }

        // Al final de tu turno, cambia al turno del enemigo
        EndTurn(); // Ahora pasamos el control al enemigo
    }

    public void PlayerStand()
    {
        if (currentTurn != Turn.Player || isGameOver) return;

        Debug.Log("Jugador se planta.");

        // Finalizar el turno del jugador
        EndTurn();

        // Ahora es el turno del enemigo
        // Verificar que el turno haya cambiado a enemigo antes de llamar a EnemyTurn()
        if (currentTurn == Turn.Enemy)
        {
            enemyAI.EnemyTurn();  // Llamamos al turno del enemigo directamente
        }
    }

    public int GenerateCard()
    {
        return Random.Range(1, 7); // Cambia a (1, 7)
    }

    public void EndTurn()
    {
        if (currentTurn == Turn.Player)
        {
            currentTurn = Turn.Enemy;
            StartEnemyTurn();
        }
        else if (currentTurn == Turn.Enemy)
        {
            currentTurn = Turn.Player;
            StartPlayerTurn();
        }
    }

    private void StartPlayerTurn()
    {
        if (isGameOver) return;

        Debug.Log("Es el turno del jugador.");
        hitButton.interactable = true;
        standButton.interactable = true;
        UpdatePlayerTotalUI();
    }

    private void StartEnemyTurn()
    {
        if (currentTurn != Turn.Enemy || isGameOver) return;
        StartCoroutine(EnemyTurnRoutine());
    }

    public IEnumerator EnemyTurnRoutine()
    {
        Debug.Log("Turno del enemigo comenzado.");

        yield return new WaitForSeconds(1f); // Pausa simulada para el turno del enemigo

        // Generar carta
        int cardValue = GenerateCard();
        enemyTotal += cardValue;
        Debug.Log($"Valor de la carta: {cardValue}, Total enemigo: {enemyTotal}");

        UpdateEnemyTotalUI();

        // Instanciar la carta visualmente
        GameObject card = Instantiate(cardPrefab, enemyCardSpawnPoint);
        card.transform.localPosition += new Vector3(cardOffset * enemyCards.Count, 0, 0);
        enemyCards.Add(card);

        Debug.Log($"El enemigo pidió una carta: {cardValue}. Total del enemigo: {enemyTotal}");

        // Verificar si el enemigo se pasó de 21
        if (enemyTotal > 21)
        {
            Debug.Log("¡El enemigo se pasó de 21!");
            EndGame(true); // El jugador gana
            yield break;
        }

        // Finalizar el turno del enemigo
        Debug.Log("El enemigo termina su turno.");
        EndTurn(); // Cambiar al turno del jugador
    }

    private void UpdateEnemyTotalUI()
    {
        if (enemyAI != null && enemyAI.enemyTotalText != null)
        {
            enemyAI.enemyTotalText.text = $"{enemyTotal}/21";
        }
        else
        {
            Debug.LogError("enemyAI o enemyTotalText no está asignado en el Inspector.");
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
