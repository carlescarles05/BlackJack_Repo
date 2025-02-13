using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public BJManager bjManager; // Referencia al BJManager
    public Transform enemyCardSpawnPoint; // Punto donde aparecen las cartas del enemigo
    public GameObject cardPrefab; // Prefab de las cartas
    public TextMeshProUGUI enemyTotalText; // Texto para mostrar el total de puntos del enemigo

    public int enemyFirstCardValue;//valor de la primera carta
    public int enemyRestCardTotalValue; //valor de la suma del resto de cartas
    public bool enemyStand=false;

    public int enemyTotal; // Total de puntos del enemigo

    public List<GameObject> enemyCards = new List<GameObject>(); // Lista de cartas del enemigo

    void Start()
    {
        // Asegúrate de que el BJManager esté correctamente asignado
        if (bjManager == null)
        {
            bjManager = FindObjectOfType<BJManager>();
        }
    }

    // Método que inicia el turno del enemigo
    public void EnemyTurn()
    {
        // Verificar si bjManager está asignado correctamente
        if (bjManager != null)
        {
            // Llamamos la corutina de EnemyTurnRoutine del BJManager
            bjManager.StartCoroutine(EnemyTurnRoutine());
        }
        else
        {
            Debug.LogError("BJManager no está asignado en EnemyAI.");
        }
    }


    public void enemyAddValueToTotal(int value) 
    {
        if (enemyTotal == 0) 
        {
            enemyFirstCardValue= value;
            enemyTotal+=value;
        }
        else 
        {
            enemyRestCardTotalValue += value;
            enemyTotal+=value;
        }
    }

    public void ResetValues() 
    {
        enemyTotal = 0;
        enemyFirstCardValue = 0;
        enemyRestCardTotalValue = 0;
        enemyStand = false;
    }
    // Esta corutina maneja el turno del enemigo
    IEnumerator EnemyTurnRoutine()
    {
        // Mientras el total del enemigo sea menor que 17, el enemigo sigue pidiendo cartas
        if (enemyTotal < 17)
        {
            yield return new WaitForSeconds(1f); // Espera un segundo entre cada carta

            // Llamamos al método EnemyHit del BJManager para que el enemigo pida una carta
            bjManager.EnemyHit(); // Hace que el enemigo pida una carta

            // Esperamos un poco antes de continuar
            yield return new WaitForSeconds(1f); // Se puede ajustar este tiempo si lo deseas
        }
        else
        {
            Debug.Log("Carles");
            yield return new WaitForSeconds(1f); // Espera un segundo entre cada carta

            bjManager.EnemyStand();

            yield return new WaitForSeconds(1f); // Se puede ajustar este tiempo si lo deseas
        }
        // Al finalizar el turno del enemigo, cambia al turno del jugador
        bjManager.EndTurn();
    }

    // Método para actualizar la UI del total de cartas del enemigo
    public void UpdateEnemyTotalUI()
    {
        if (enemyTotalText != null)
        {
            if(enemyStand==false) enemyTotalText.text = "? +"+ enemyRestCardTotalValue + "/21"; // Actualiza el texto con el total
            else enemyTotalText.text = enemyTotal + "/21";
        }
    }

}
