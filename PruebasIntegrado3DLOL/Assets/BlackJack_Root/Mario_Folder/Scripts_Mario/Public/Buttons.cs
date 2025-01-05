using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    [Header("This Scene Canvas Event Handler")]
    public GameObject Register;
    public GameObject GamePANEL;
    public GameObject cardDeck;

    private GuessTheCard guessTheCard;

    void Start()
    {
        // Get the GuessTheCard component from the cardDeck GameObject
        if (cardDeck != null)
        {
            guessTheCard = cardDeck.GetComponent<GuessTheCard>();
        }
        cardDeck.SetActive(false);
        GamePANEL.SetActive(false);
    }

    // Activate the menu canvas
    public void SwitchToRegisterPANEL()
    {
        GamePANEL.SetActive(false);
        Register.SetActive(true);
        cardDeck.SetActive(false);
    }

    // Activate the gameANDUI canvas
    public void ButtonGameCardPANEL()
    {
        GamePANEL.SetActive(true);
        Register.SetActive(false);
        cardDeck.SetActive(true);

        // Reset the game state
        if (guessTheCard != null)
        {
            guessTheCard.ResetGame();
        }
    }
}
