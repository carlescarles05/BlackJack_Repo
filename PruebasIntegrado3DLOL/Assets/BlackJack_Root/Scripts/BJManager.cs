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
        /*if (currentTurn != Turn.Player || isGameOver) return;

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
        }*/
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
        EndTurn();
    }

    public int GenerateCard()
    {
        return Random.Range(1, 11); // Cambia a (1, 12) si quieres incluir el 11
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
        if (isGameOver) return;

        Debug.Log("Es el turno del enemigo.");
        hitButton.interactable = false;
        standButton.interactable = false;

        StartCoroutine(EnemyTurnRoutine());
    }

    private IEnumerator EnemyTurnRoutine()
    {
        while (enemyTotal < 17 && !isGameOver)
        {
            yield return new WaitForSeconds(1f);

            int cardValue = GenerateCard();
            enemyTotal += cardValue;

            UpdateEnemyTotalUI();
            Debug.Log($"El enemigo pidió una carta: {cardValue}. Total del enemigo: {enemyTotal}");

            GameObject card = Instantiate(cardPrefab, enemyCardSpawnPoint);
            card.transform.localPosition += new Vector3(cardOffset * enemyCards.Count, 0, 0);
            enemyCards.Add(card);

            if (enemyTotal > maxPoints)
            {
                Debug.Log("¡El enemigo se pasó de 21!");
                EndGame(true);
                yield break;
            }
        }

        Debug.Log("El enemigo se planta.");
        EndTurn();
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
