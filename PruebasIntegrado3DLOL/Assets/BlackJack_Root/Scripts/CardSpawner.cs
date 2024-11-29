using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    public Transform playerCardSpawn; // Referencia al punto donde se generar�n las cartas del jugador
    public Transform enemyCardSpawn;  // Referencia al punto donde se generar�n las cartas del enemigo

    public void SpawnCards()
    {
        // Aseg�rate de que las referencias playerCardSpawn y enemyCardSpawn no sean nulas
        if (playerCardSpawn == null || enemyCardSpawn == null)
        {
            Debug.LogError("Los puntos de spawn no est�n asignados correctamente.");
            return;
        }

        // C�digo para hacer aparecer las cartas en los puntos de spawn
        // (Tu c�digo de Spawn deber�a estar aqu�, utilizando playerCardSpawn y enemyCardSpawn)
    }
}
