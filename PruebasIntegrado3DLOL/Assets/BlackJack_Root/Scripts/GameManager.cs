using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DeckManager deckManager; // Referencia al DeckManager
    public int playerScore = 0;
    public int opponentScore = 0;

    public void StartGame()
    {
        // Iniciar el reparto de cartas
        if (deckManager != null)
        {
            deckManager.DrawCards();
        }
        else
        {
            Debug.LogError("DeckManager no está asignado en el Inspector.");
        }
    }

}
