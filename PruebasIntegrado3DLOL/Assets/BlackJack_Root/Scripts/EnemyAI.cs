using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public int enemyTotal = 0; // Puntos totales del enemigo
    public Transform enemyCardSpawnPoint; // Punto donde aparecen las cartas del enemigo
    public GameObject cardPrefab; // Prefab de las cartas
    public BJManager gameManager; // Referencia al BJManager

    private int maxEnemyCards = 5; // Número máximo de cartas visibles
    private int cardOffset = 30; // Espaciado entre cartas
    public List<GameObject> EnemyCards = new List<GameObject>();

    private void Start()
    {
        if (EnemyCards == null)
        {
            EnemyCards = new List<GameObject>();
        }
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<BJManager>();
            if (gameManager == null)
            {
                Debug.LogError("BJManager no encontrado. Por favor, asegúrate de asignarlo en el Inspector.");
            }
        }
    }

    public void EnemyTurn()
    {
        Debug.Log("Turno del enemigo.");

        while (enemyTotal < 17) // Lógica básica: Pedir cartas si el total es menor a 17
        {
            // Generar una nueva carta
            int newCard = gameManager.GenerateCard();
            enemyTotal += newCard;

            // Crear la carta visualmente
            GameObject card = Instantiate(cardPrefab, enemyCardSpawnPoint);

            // Ajustar la posición de la carta basándose en la lista de cartas
            card.transform.localPosition += new Vector3(cardOffset * EnemyCards.Count, 0, 0);
            EnemyCards.Add(card);

            Debug.Log($"Carta del enemigo: {newCard}");
            Debug.Log($"Total del enemigo: {enemyTotal}");

            // Verificar si el enemigo pierde
            if (enemyTotal > 21)
            {
                Debug.Log("El enemigo se pasó de 21.");
                gameManager.EndGame(true); // Jugador gana
                return;
            }
        }

        // El enemigo se planta
        Debug.Log("El enemigo se planta.");
        gameManager.CompareScores(); // Comparar puntajes
    }
}
