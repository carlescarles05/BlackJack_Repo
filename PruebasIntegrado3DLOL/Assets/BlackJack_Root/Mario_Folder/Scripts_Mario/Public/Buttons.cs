using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEditor.Search;
public class Buttons : MonoBehaviour
{
    //UI
    public GameObject gameAbout;       // 01_MainScreen
    public GameObject GameCanvas; // 02_GuessTheCardGame
    public Button guessButton;
    //Scripts
    private Scene_Manager sceneManager;
    public GuessTheCard guessthecardscript;
    public GuessCardInputActions inputActions;//
    public GameObject cardDeck;
    private EventSystem eventSystem;
    public void Awake()
    {
        inputActions = new GuessCardInputActions();
        inputActions.GuessTheCardGame.Bet.performed += ctx => SimulateButtonClick(guessButton);
        eventSystem = EventSystem.current;
    }
    public void OnEnable() => inputActions.GuessTheCardGame.Enable();
    public void OnDisable() => inputActions.GuessTheCardGame.Disable();
    void Start()
    {
        sceneManager = FindObjectOfType<Scene_Manager>();
        if (sceneManager == null)
        {
            Debug.LogError("Scene_Manager not found in the scene!");
        }
        guessthecardscript = FindObjectOfType<GuessTheCard>();
        if (guessthecardscript == null)
        {
            Debug.LogError("GTC not found in the scene!,Buttons call");
        }


        cardDeck.gameObject.SetActive(false);  //active on GTCS

        // Start with only the main screen enabled
        ShowMainScreen();

        //
        if (cardDeck == null)
        {
            Debug.LogWarning("Buttons component message:No GCG component.");
        }
    }
    public void SimulateButtonClick(Button button)
    {
        if (button != null)
        {
            eventSystem.SetSelectedGameObject(button.gameObject);
            button.onClick.Invoke();
            Debug.Log("Apuesta button clicked via Input System!");
        }
        else
        {
            Debug.LogError("Apuesta button is not assigned in the Inspector!");
        }
    }
    public void ShowMainScreen()
    {
        gameAbout.SetActive(true);
        GameCanvas.SetActive(false);
        cardDeck.gameObject.SetActive(false);
        SFXManager.Instance.DisableEnvironmentAudio();
    }
    //controls screen button
    public void ShowGuessTheCardGame()
    {
        
        
        StartCoroutine(DelayedGameActivation()); 
    }
    public void activaControlsImage()
    {
     guessthecardscript.controlsImage.SetActive(true);
     StartCoroutine(DelayedGameActivation());
    }
    IEnumerator ControlsDeactivation()
    { 
      yield return new WaitForSeconds(4f);
      guessthecardscript.controlsImage.SetActive(false);
    }
     IEnumerator DelayedGameActivation()
    {
        guessthecardscript.controlsImage.SetActive(true);
        yield return new WaitForSeconds(3f);
        gameAbout.SetActive(false);
        guessthecardscript.controlsImage.SetActive(false);
        GameCanvas.SetActive(true);
        cardDeck.gameObject.SetActive(true);
        SFXManager.Instance.EnableEnvironmentAudio();
    }

    //controls screen button
    public void GoBackToMainScene() //change back to playercapsule's scene
    {
        gameAbout.SetActive(false);
        GameCanvas.SetActive(false);
       if(cardDeck != null) cardDeck.gameObject.SetActive(false);
        sceneManager.GoBackToMainScreen(); // Return to "00_Scenario"or another
    }

    public void LoadMachineGame()
    {
        gameAbout.SetActive(false);
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
            if (guessthecardscript != null)
            {
            
                Debug.Log("Guess The Card script enabled after bet!,Buttons call");
            }
        }
        else
        {
            Debug.LogError("BetManager not found in scene!,buttons call");
        }
    }
}