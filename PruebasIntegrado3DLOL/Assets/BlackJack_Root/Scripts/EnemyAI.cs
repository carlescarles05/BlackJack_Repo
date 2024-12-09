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
    public List<GameObject> enemyCards = new List<GameObject>(); // Lista de cartas del enemigo

    public void EnemyTurn()
    {
        if (bjManager == null)
        {
            Debug.LogError("BJManager no asignado.");
            return;
        }

        Debug.Log("EnemyTurn ha sido llamado.");
        StartCoroutine(EnemyTurnRoutine()); // Asegúrate de que solo se llama a esta rutina una vez
    }

    private IEnumerator EnemyTurnRoutine()
    {
        Debug.Log("Turno del enemigo comenzado.");

        // Pide solo una carta, no más
        yield return new WaitForSeconds(1f); // Pausa para simular el tiempo de espera

        int cardValue = bjManager.GenerateCard();
        enemyTotal += cardValue;
        UpdateEnemyTotalUI();

        GameObject card = Instantiate(cardPrefab, enemyCardSpawnPoint);
        card.transform.localPosition += new Vector3(cardOffset * enemyCards.Count, 0, 0);
        enemyCards.Add(card);

        Debug.Log($"El enemigo pidió una carta: {cardValue}. Total del enemigo: {enemyTotal}");

        // Si el enemigo se pasa de 21, termina el juego
        if (enemyTotal > 21)
        {
            Debug.Log("¡El enemigo se pasó de 21!");
            bjManager.EndGame(true); // El jugador gana si el enemigo se pasa de 21
            yield break; // Termina el turno del enemigo
        }

        // El turno del enemigo termina después de una sola carta
        Debug.Log("El enemigo termina su turno.");
        bjManager.EndTurn(); // Cambiar al turno del jugador
    }

    private void UpdateEnemyTotalUI()
    {
        if (enemyTotalText != null)
        {
            enemyTotalText.text = $"{enemyTotal}/21"; // Asegúrate de que esto se actualice correctamente
        }
        else
        {
            Debug.LogError("enemyTotalText no está asignado en el Inspector.");
        }
    }
}
