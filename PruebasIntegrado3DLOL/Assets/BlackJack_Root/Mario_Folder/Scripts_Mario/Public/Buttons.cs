using UnityEngine;

public class Buttons : MonoBehaviour
{
    public GameObject mainScreenCanvas;       // 01_MainScreen
    public GameObject guessTheCardGameCanvas; // 02_GuessTheCardGame
    private Scene_Manager sceneManager;
    void Start()
    {
        sceneManager = FindObjectOfType<Scene_Manager>();

        if (sceneManager == null)
        {
            Debug.LogError("Scene_Manager not found in the scene!");
        }

        // Start with only the main screen enabled
        ShowMainScreen();
    }
    public void ShowMainScreen()
    {
        mainScreenCanvas.SetActive(true);
        guessTheCardGameCanvas.SetActive(false);
    }

    public void ShowGuessTheCardGame()
    {
        mainScreenCanvas.SetActive(false);
        guessTheCardGameCanvas.SetActive(true);
    }

    public void GoBackToMainScene()
    {
        sceneManager.GoBackToMainScreen(); // Return to "00_Scenario"
    }

    public void LoadMachineGame()
    {
        sceneManager.LoadScene2(); // Load "01_Machine#1"
    }

    public void LoadGuessTheCardGame()
    {
        sceneManager.LoadScene3(); // Load "02_GuessTheCardGame"
    }
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