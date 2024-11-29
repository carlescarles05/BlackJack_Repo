using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<Card> deck = new List<Card>(); // Lista que representa el mazo de cartas

    public Card DrawCard()
    {
        if (deck.Count > 0)
        {
            Card drawnCard = deck[0]; // Tomar la primera carta del mazo
            deck.RemoveAt(0);         // Eliminarla del mazo
            return drawnCard;
        }
        else
        {
            Debug.LogWarning("El mazo está vacío.");
            return null;
        }
    }
}
