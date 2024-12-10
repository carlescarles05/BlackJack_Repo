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

    void Start()
    {
        if (bjManager == null)
        {
            bjManager = FindObjectOfType<BJManager>();
        }
    }

    public void EnemyTurn()
    {
        // Verificar si bjManager está asignado correctamente
    if (bjManager != null)
    {
        // Llamamos la corutina del BJManager correctamente
        bjManager.StartCoroutine(bjManager.EnemyTurnRoutine());  // Llama a la corutina desde BJManager
            Debug.Log("f");
    }
    else
    {
        Debug.LogError("BJManager no está asignado en EnemyAI.");
    }
    }  

}
