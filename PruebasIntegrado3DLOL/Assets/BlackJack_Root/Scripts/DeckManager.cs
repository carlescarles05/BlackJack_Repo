using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<GameObject> Deck; // Lista de prefabs de cartas
    public Transform[] PlayerCardSpawns; // Spawn Points para las cartas del jugador
    public Transform[] EnemyCardSpawns;  // Spawn Points para las cartas del enemigo

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
        if (Deck.Count == 0)
        {
            Debug.Log("No quedan cartas en el mazo.");
            return;
        }

        // Seleccionar la primera carta de la lista
        GameObject card = Deck[0];

        // Instanciar la carta en el Spawn Point
        Instantiate(card, spawnPoint.position, spawnPoint.rotation);

        // Eliminar la carta de la lista para que no se use de nuevo
        Deck.RemoveAt(0);
    }
}
