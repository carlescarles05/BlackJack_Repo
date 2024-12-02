using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BJManager : MonoBehaviour
{
    public SitOnObject playerSitScript; // Script del jugador
    public Transform playerCardSpawnPoint; // Objeto vacío para spawn de cartas del jugador
    public GameObject cardPrefab; // Prefab de la carta
    public Button hitButton; // Botón "Pedir Carta"
    public Button standButton; // Botón "Plantarse"

    private int cardOffset = 30; // Espaciado entre cartas
    private int maxCardCount = 5; // Número máximo de cartas visibles

    void Start()
    {
        // Asignar eventos a los botones
        hitButton.onClick.AddListener(PlayerHit);
        standButton.onClick.AddListener(PlayerStand);

        // Generar las cartas iniciales
        DealInitialCards();
    }

    void DealInitialCards()
    {
        // Generar dos cartas iniciales para el jugador
        GeneratePlayerCard();
        GeneratePlayerCard();
    }

    public void PlayerHit()
    {
        if (!playerSitScript.IsPlayerTurn) return;

        GeneratePlayerCard();

        // Verificar si el jugador pierde
        if (playerSitScript.PlayerTotal > 21)
        {
            Debug.Log("¡Jugador eliminado! Total mayor a 21.");
            EndGame(false);
        }
    }

    void GeneratePlayerCard()
    {
        if (cardPrefab == null || playerCardSpawnPoint == null)
        {
            return;
        }

        // Generar una nueva carta aleatoria
        int newCard = playerSitScript.GenerateCard();
        playerSitScript.PlayerTotal += newCard;

        // Crear visualmente la carta
        GameObject card = Instantiate(cardPrefab, playerCardSpawnPoint.position, Quaternion.identity);
        card.transform.SetParent(playerCardSpawnPoint); // Asegura que la carta esté dentro del spawn point en la jerarquía
        card.transform.localScale = Vector3.one; // Normaliza la escala si es necesario

        // Activar el efecto de partículas (si existe)
        ParticleSystem particleSystem = playerCardSpawnPoint.GetComponentInChildren<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Play();
        }

        // Añadir el valor de la carta al texto
        TextMeshProUGUI cardText = card.GetComponentInChildren<TextMeshProUGUI>();
        if (cardText != null)
        {
            cardText.text = newCard.ToString();
        }
        else
        {
            Debug.LogWarning("El prefab de la carta no tiene un componente TextMeshProUGUI.");
        }

        // Agregar la carta a la lista de cartas del jugador
        playerSitScript.PlayerCards.Add(card);

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
        if (playerWon)
        {
            Debug.Log("¡Ganaste!");
        }
        else
        {
            Debug.Log("Perdiste. ¡Intenta de nuevo!");
        }

        // Desactivar los botones
        hitButton.interactable = false;
        standButton.interactable = false;

        // Reiniciar o realizar acciones adicionales si es necesario
        playerSitScript.IsPlayerTurn = false;
    }
}
