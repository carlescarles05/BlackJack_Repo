using UnityEngine;
//Calls BetManager PlaceBet
public class Buttons : MonoBehaviour
{
    public GameObject ControlScreen;       // 01_MainScreen
    public GameObject GameCanvas; // 02_GuessTheCardGame
    private Scene_Manager sceneManager;
    public GameObject cardDeck;
    void Start()
    {
        sceneManager = FindObjectOfType<Scene_Manager>();

        if (sceneManager == null)
        {
            Debug.LogError("Scene_Manager not found in the scene!");
        }
        cardDeck.gameObject.SetActive(false);
     
        // Start with only the main screen enabled
        ShowMainScreen();

        //
        if(cardDeck == null)
        {
            Debug.LogWarning("Buttons component message:No GCG component.");
        }
    }
    //SlotMachine 2



    //Guess The Card
    //guessthe card screen button
    public void ShowMainScreen()
    {
        ControlScreen.SetActive(true);
        GameCanvas.SetActive(false);
        cardDeck.gameObject.SetActive(false);
        SFXManager.Instance.DisableEnvironmentAudio();
    }
    //controls screen button
    public void ShowGuessTheCardGame()
    {
        ControlScreen.SetActive(false);

        GameCanvas.SetActive(true);
        cardDeck.gameObject.SetActive(true);
        SFXManager.Instance.EnableEnvironmentAudio();
    }
 
    //controls screen button
    public void GoBackToMainScene() //change back to playercapsule scene
    {
        ControlScreen.SetActive(false);
        GameCanvas.SetActive(false);
        cardDeck.gameObject.SetActive(false);
        sceneManager.GoBackToMainScreen(); // Return to "00_Scenario"or another
    }

    public void LoadMachineGame()
    {
        ControlScreen.SetActive(false);
        GameCanvas.SetActive(true);
        sceneManager.LoadScene2(); // Load "01_Machine#1"
    }

    public void LoadGuessTheCardGame()
    {
        sceneManager.LoadScene3(); // Load "02_GuessTheCardGame"
    }
    //guess the card screen button
    public void OnBetButtonPressed()
    {
        BetManager betManager = FindObjectOfType<BetManager>();

        if (betManager != null)
        {
            betManager.PlaceBet();
        }
        else
        {
            Debug.LogError("BetManager not found in scene!");
        }
    }
}