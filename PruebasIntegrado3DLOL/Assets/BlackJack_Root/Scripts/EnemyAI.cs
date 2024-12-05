using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public BJManager bjManager; // Referencia al BJManager
    public Transform enemyCardSpawnPoint; // Punto donde aparecen las cartas del enemigo
    public GameObject cardPrefab; // Prefab de las cartas
    public TextMeshProUGUI enemyTotalText; // Texto para mostrar el total de puntos del enemigo
    public int enemyTotal = 0; // Total de puntos del enemigo

    private int cardOffset = 30; // Espaciado entre cartas visibles del enemigo
    private List<GameObject> enemyCards = new List<GameObject>(); // Lista de cartas del enemigo

    public void EnemyTurn()
    {
        Debug.Log("EnemyTurn ha sido llamado."); // Confirmar que entra aquí
        StartCoroutine(EnemyTurnRoutine());
    }

    private IEnumerator EnemyTurnRoutine()
    {
        Debug.Log("Turno del enemigo.");

        while (enemyTotal < 17) // Lógica básica: el enemigo pide carta si tiene menos de 17 puntos
        {
            yield return new WaitForSeconds(1f); // Simula una pausa entre acciones de la IA

            // Generar y añadir una carta
            int cardValue = bjManager.GenerateCard(); // Llama al método de BJManager
            enemyTotal += cardValue;
            UpdateEnemyTotalUI();

            // Instanciar una nueva carta
            GameObject card = Instantiate(cardPrefab, enemyCardSpawnPoint);
            card.transform.localPosition += new Vector3(cardOffset * enemyCards.Count, 0, 0);
            enemyCards.Add(card);

            Debug.Log($"El enemigo pidió una carta: {cardValue}. Total del enemigo: {enemyTotal}");

            // Verificar si el enemigo se pasó de 21
            if (enemyTotal > 21)
            {
                Debug.Log("¡El enemigo se pasó de 21!");
                bjManager.EndGame(true); // Jugador gana
                yield break;
            }
        }

        Debug.Log("El enemigo se planta.");
        bjManager.CompareScores();
    }

    private void UpdateEnemyTotalUI()
    {
        if (enemyTotalText != null)
        {
            enemyTotalText.text = $"{enemyTotal}/21";
            Debug.Log($"Actualizando el puntaje del enemigo: {enemyTotal}/21");
        }
        else
        {
            Debug.LogError("EnemyTotalText no está asignado en el Inspector.");
        }
    }
}
