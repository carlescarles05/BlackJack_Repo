using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BJManager : MonoBehaviour
{
    public SitOnObject playerSitScript; // Asignar script del jugador en el Inspector
    public Transform playerCardSpawnPoint; // Punto inicial para generar cartas del jugador
    public GameObject cardPrefab; // Prefab de la carta
    public TextMeshProUGUI resultText; // Texto para mostrar el resultado
    public Button hitButton; // Botón "Pedir Carta"
    public Button standButton; // Botón "Plantarse"

    private int cardOffset = 30; // Espaciado entre cartas
    private int maxCardCount = 5; // Número máximo de cartas visibles

    void Start()
    {
        // Asignar los eventos de los botones
        hitButton.onClick.AddListener(PlayerHit);
        standButton.onClick.AddListener(PlayerStand);

        // Inicializar cartas
        DealInitialCards();
    }

    void DealInitialCards()
    {
        // Generar dos cartas iniciales para el jugador
        GeneratePlayerCard();
        GeneratePlayerCard();

        Debug.Log("Cartas iniciales del jugador: " + playerSitScript.PlayerTotal);
    }

    public void PlayerHit()
    {
        if (!playerSitScript.IsPlayerTurn) return;

        GeneratePlayerCard();

        Debug.Log("Nueva carta del jugador: " + playerSitScript.PlayerTotal);

        // Verificar si el jugador pierde
        if (playerSitScript.PlayerTotal > 21)
        {
            Debug.Log("¡Jugador eliminado! Total mayor a 21.");
            EndGame(false);
        }
    }

    void GeneratePlayerCard()
    {
        // Generar una nueva carta aleatoria
        int newCard = playerSitScript.GenerateCard();
        playerSitScript.PlayerTotal += newCard;

        // Crear visualmente la carta
        GameObject card = Instantiate(cardPrefab, playerCardSpawnPoint);
        card.transform.localPosition += new Vector3(cardOffset * (playerSitScript.PlayerCards.Count - 1), 0, 0);
        card.GetComponentInChildren<TextMeshProUGUI>().text = newCard.ToString();

        playerSitScript.PlayerCards.Add(card);

        // Limitar el número de cartas visibles
        if (playerSitScript.PlayerCards.Count > maxCardCount)
        {
            Destroy(playerSitScript.PlayerCards[0]);
            playerSitScript.PlayerCards.RemoveAt(0);
        }

        // Actualizar el texto del total del jugador
        if (playerSitScript.playerTotalText != null)
        {
            playerSitScript.playerTotalText.text = playerSitScript.PlayerTotal + "/21";
        }
    }

    public void PlayerStand()
    {
        if (!playerSitScript.IsPlayerTurn) return;

        Debug.Log("Jugador se planta. Turno del enemigo.");
        playerSitScript.IsPlayerTurn = false;

        EnemyTurn();
    }

    void EnemyTurn()
    {
        int enemyTotal = playerSitScript.enemyTotal;

        while (enemyTotal < 17)
        {
            int newCard = playerSitScript.GenerateCard();
            enemyTotal += newCard;
            Debug.Log("Carta del enemigo: " + newCard);
            Debug.Log("Total del enemigo: " + enemyTotal);

            if (enemyTotal > 21)
            {
                Debug.Log("¡El enemigo se pasó de 21!");
                EndGame(true);
                return;
            }
        }

        Debug.Log("El enemigo se planta.");
        CompareScores();
    }

    void CompareScores()
    {
        int playerTotal = playerSitScript.PlayerTotal;
        int enemyTotal = playerSitScript.enemyTotal;

        if (playerTotal > enemyTotal && playerTotal <= 21 || enemyTotal > 21)
        {
            EndGame(true);
        }
        else
        {
            EndGame(false);
        }
    }

    void EndGame(bool playerWon)
    {
        resultText.text = playerWon ? "¡Ganaste!" : "Perdiste. ¡Intenta de nuevo!";
        resultText.gameObject.SetActive(true);

        hitButton.interactable = false;
        standButton.interactable = false;
    }
}
