using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*public DeckManager DeckManager;    // Referencia al mazo de cartas
    public int PlayerScore = 0;        // Puntuaci�n del jugador
    public int OpponentScore = 0;      // Puntuaci�n del oponente
    public int MaxScore = 21;          // Puntuaci�n m�xima (21 en este caso)

    void PlayerTurn()
    {
        if (DeckManager == null)
        {
            Debug.LogError("DeckManager no est� asignado en el Inspector.");
            return;
        }

        Card card = DeckManager.DrawCard();
        if (card != null)
        {
            // A�adir la carta a la mano del jugador
            PlayerScore += card.Value;
            Debug.Log($"Jugador roba {card.Name}. Puntuaci�n: {PlayerScore}");

            if (PlayerScore > MaxScore)
            {
                EndGame(false); // Si el jugador supera el m�ximo, pierde
            }
        }
        else
        {
            Debug.LogWarning("No quedan cartas en el mazo.");
        }
    }

    void OpponentTurn()
    {
        if (DeckManager == null)
        {
            Debug.LogError("DeckManager no est� asignado en el Inspector.");
            return;
        }

        // L�gica b�sica de IA para el oponente
        while (OpponentScore < 17)
        {
            Card card = DeckManager.DrawCard();
            if (card != null)
            {
                OpponentScore += card.Value;
                Debug.Log($"Oponente roba {card.Name}. Puntuaci�n: {OpponentScore}");
            }
            else
            {
                Debug.LogWarning("No quedan cartas en el mazo.");
                break;
            }
        }

        if (OpponentScore > MaxScore)
        {
            EndGame(true); // Si el oponente supera el m�ximo, el jugador gana
        }
        else
        {
            DetermineWinner(); // Determinar el ganador
        }
    }

    void EndGame(bool playerWins)
    {
        Debug.Log(playerWins ? "�Has ganado!" : "Has perdido...");
        // Aqu� puedes a�adir m�s l�gica para reiniciar el juego o mostrar un men�.
    }

    void DetermineWinner()
    {
        if (PlayerScore > OpponentScore)
        {
            EndGame(true); // El jugador gana
        }
        else if (PlayerScore < OpponentScore)
        {
            EndGame(false); // El oponente gana
        }
        else
        {
            Debug.Log("Empate..."); // Ambos tienen la misma puntuaci�n
        }
    }*/
}
