using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BJManager : MonoBehaviour
{
    public SitOnObject playerSitScript; // Asigna el script de jugador en el Inspector

    void Start()
    {
        // Inicializa las cartas del jugador al inicio del juego
        DealInitialCards();
    }

    void DealInitialCards()
    {
        // Generar cartas iniciales del jugador
        int initialCard1 = playerSitScript.GenerateCard();
        int initialCard2 = playerSitScript.GenerateCard();
        playerSitScript.PlayerTotal = initialCard1 + initialCard2;

        Debug.Log("Cartas iniciales del jugador: " + playerSitScript.PlayerTotal);
    }

    public void PlayerHit()
    {
        // Lógica para pedir una nueva carta
        int newCard = playerSitScript.GenerateCard();
        playerSitScript.PlayerTotal += newCard;

        Debug.Log("Nueva carta del jugador: " + newCard);
        Debug.Log("Total actual del jugador: " + playerSitScript.PlayerTotal);

        // Aquí puedes añadir lógica para verificar si el jugador ha perdido
        if (playerSitScript.PlayerTotal > 21)
        {
            Debug.Log("¡Jugador eliminado! Total mayor a 21.");
            EndGame(false); // Ejemplo de función para finalizar el juego
        }
    }

    void EndGame(bool playerWon)
    {
        if (playerWon)
        {
            Debug.Log("¡Ganaste!");
        }
        else
        {
            Debug.Log("Perdiste. ¡Intenta de nuevo!");
        }
    }
}
