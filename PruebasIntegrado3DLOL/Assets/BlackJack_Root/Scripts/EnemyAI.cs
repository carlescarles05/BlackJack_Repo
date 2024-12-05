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
        if (bjManager == null)
        {
            Debug.LogError("BJManager no asignado. Asegúrate de arrastrar el objeto en el Inspector.");
            return;
        }

        Debug.Log("EnemyTurn ha sido llamado.");
        StartCoroutine(EnemyTurnRoutine());
    }

    private IEnumerator EnemyTurnRoutine()
    {
        Debug.Log("Turno del enemigo.");
        while (enemyTotal < 17)
        {
            yield return new WaitForSeconds(1f);

            int cardValue = bjManager.GenerateCard();
            enemyTotal += cardValue;
            UpdateEnemyTotalUI();

            GameObject card = Instantiate(cardPrefab, enemyCardSpawnPoint);
            card.transform.localPosition += new Vector3(cardOffset * enemyCards.Count, 0, 0);
            enemyCards.Add(card);

            Debug.Log($"El enemigo pidió una carta: {cardValue}. Total del enemigo: {enemyTotal}");

            if (enemyTotal > 21)
            {
                Debug.Log("¡El enemigo se pasó de 21!");
                bjManager.EndGame(true);
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
        }
        else
        {
            Debug.LogError("EnemyTotalText no está asignado en el Inspector.");
        }
    }
}
