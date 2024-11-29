using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    public Transform playerCardSpawn; // Referencia al punto donde se generarán las cartas del jugador
    public Transform enemyCardSpawn;  // Referencia al punto donde se generarán las cartas del enemigo

    public void SpawnCards()
    {
        // Asegúrate de que las referencias playerCardSpawn y enemyCardSpawn no sean nulas
        if (playerCardSpawn == null || enemyCardSpawn == null)
        {
            Debug.LogError("Los puntos de spawn no están asignados correctamente.");
            return;
        }

        // Código para hacer aparecer las cartas en los puntos de spawn
        // (Tu código de Spawn debería estar aquí, utilizando playerCardSpawn y enemyCardSpawn)
    }
}
