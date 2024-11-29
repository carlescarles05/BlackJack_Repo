using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameObject[] Deck; // Array de prefabs de cartas
    public Transform[] PlayerCardSpawns; // Spawn Points para las cartas del jugador
    public Transform[] EnemyCardSpawns;  // Spawn Points para las cartas del enemigo

    private int currentIndex = 0;

    public void DrawCards()
    {
        // Robar dos cartas para el jugador
        for (int i = 0; i < PlayerCardSpawns.Length; i++)
        {
            SpawnCard(PlayerCardSpawns[i]);
        }

        // Robar dos cartas para el enemigo
        for (int i = 0; i < EnemyCardSpawns.Length; i++)
        {
            SpawnCard(EnemyCardSpawns[i]);
        }
    }

    private void SpawnCard(Transform spawnPoint)
    {
        if (currentIndex >= Deck.Length)
        {
            Debug.Log("No quedan cartas en el mazo.");
            return;
        }

        // Instanciar la carta en el Spawn Point
        GameObject card = Instantiate(Deck[currentIndex], spawnPoint.position, spawnPoint.rotation);
        currentIndex++;
    }
}
