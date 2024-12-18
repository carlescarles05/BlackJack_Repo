using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    [Header("This Scene Canvas Event Handler")]
    public GameObject Register;
    public GameObject Game1Canvas;
    public GameObject cardDeck;

    private GuessTheCard guessTheCard;

    void Start()
    {
        // Get the GuessTheCard component from the cardDeck GameObject
        if (cardDeck != null)
        {
            guessTheCard = cardDeck.GetComponent<GuessTheCard>();
        }
    }

    // Activate the menu canvas
    public void SwitchToMENUCanvas()
    {
        Game1Canvas.SetActive(false);
        Register.SetActive(true);
        cardDeck.SetActive(false);
    }

    // Activate the game canvas
    public void ButtonGameCardCANVA()
    {
        Game1Canvas.SetActive(true);
        Register.SetActive(false);
        cardDeck.SetActive(true);

        // Reset the game state
        if (guessTheCard != null)
        {
            guessTheCard.ResetGame();
        }
    }
}
