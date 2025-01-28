using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    [Header("This Scene Canvas Event Handler")]
    public GameObject one_Register;
    public GameObject Game1PANEL;
    public GameObject Game2PANEL;
    public GameObject two_pickGame;
    public GameObject cardDeck;
    public GameObject balancePanelMsg;
    public Text messageText;
    //

    private GuessTheCard guessTheCard; //sceipt reference
    public  Scene_Manager sceneManager;
    private Player_Points pointsManager;

    void Start()
    {
        // Get the GuessTheCard component from the cardDeck GameObject
        if (cardDeck != null)
        {
            guessTheCard = cardDeck.GetComponent<GuessTheCard>();
        }
        GameObject PlayerpointManager = GameObject.Find("Points Space");
       
        if (PlayerpointManager != null)
        { 
            pointsManager = PlayerpointManager.GetComponent<Player_Points>();
        }
        else 
        {
            Debug.LogError("PlayerpointManager or Player_Points script is missing!"); 
        }
        //Initialize UI
        one_Register.SetActive(true);
        two_pickGame.SetActive(false);
        cardDeck.SetActive(false);
        Game1PANEL.SetActive(false);
        Game2PANEL.SetActive(false);
        //feedback
        balancePanelMsg.SetActive(false);
        //
    }
    public void OnBetButtonPressed() 
    {
        if (!pointsManager.HasEnoughPoints(pointsManager.minPoints))
        {
            //If not enough points
            LockGameInputs();
            guessTheCard.EnableInputActions(false);
            ShowBalanceMsg("Not Enough Points");
           
           
        }
        else 
        {
            //Enough points
            UnlockGameInputs();
            guessTheCard.EnableInputActions(true);
            Debug.Log("Bet placed.Player has enough points.");
        }
    }
    private void LockGameInputs() 
    {
        if (guessTheCard != null)
        {
            guessTheCard.enabled = false; //Disable Guess the card script
        }
    }
    //Unlock inputs to enable GuessTheCard
    private void UnlockGameInputs()
    {
        if (guessTheCard!= null)
        {
            guessTheCard.enabled = true;
        }
    }
    public void XClose() 
    {
        one_Register.SetActive(false);
        two_pickGame.SetActive(false);
        cardDeck.SetActive(false);
        Game1PANEL.SetActive(false);
        Game2PANEL.SetActive(false);
        sceneManager.LoadScene2();
        //00_Mario_Object_MACHINE#1
    }
    // Activate the menu canvas
    public void SwitchToRegisterPANEL()
    {
        Debug.Log("Switch to register panel called");
        Game1PANEL.SetActive(false);
        Game2PANEL.SetActive(false);
        one_Register.SetActive(true);
        two_pickGame.SetActive(false);
        cardDeck.SetActive(false);
        CloseBalanceMessage();
    }
    public void ShowBalanceMsg(string message)
    {
        if (balancePanelMsg != null && balancePanelMsg != null)
        {
            balancePanelMsg.gameObject.SetActive(true);
            messageText.text = message;
            LockGameInputs();
        }
    }
    public void CloseBalanceMessage()
    { 
     balancePanelMsg.SetActive(false); //if needed.

    }
    public void pickGameMenu()
    {
        Debug.Log("Pick you games menu activated");
        Game1PANEL.SetActive(false);
        Game2PANEL.SetActive(false);
        one_Register.SetActive(false);
        cardDeck.SetActive(false);
       two_pickGame.SetActive(true);

    }
    // Activate the gameANDUI canvas
    public void game13CardGame()
    {
        Debug.Log("Button Game#1 Card script called");
        one_Register.SetActive(false);
        two_pickGame.SetActive(false);
        Game1PANEL.SetActive(true);
        Game2PANEL.SetActive(false);
        cardDeck.SetActive(true);
    }
    public void game2CARDPANEL() 
    {
        Debug.Log("Game2Activated");
        one_Register.SetActive(false);
        two_pickGame.SetActive(false);
        Game1PANEL.SetActive(false);
        Game2PANEL.SetActive(true) ;
        cardDeck.SetActive (false);
        CloseBalanceMessage();
    }

}
