using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEditor.ShaderGraph;
// I wont use Input_Manager here.

public class GuessTheCard : MonoBehaviour
{
    [SerializeField]
    [Header("UI ELEMENTS")]
    public Player_Points player_Points;
    public Player_Clock player_Clock;
    public GameObject winPanel;
    public Text resultText;
    public Text winText;
    public GuessCardInputActions inputActions;

    public GameObject[] cards;

    private int MachineNumber;
    private int selectedCardIndexPos = 0; // Selected card index
    private int turnCount = 0;            // Turn counter
                                       
    void Awake()
    {
        // Initialize input actions for navigation and selection
        inputActions = new GuessCardInputActions();
        inputActions.GuessTheCardGame.Enable();
        inputActions.GuessTheCardGame.Navigation.performed += OnNavigate;
        inputActions.GuessTheCardGame.Submition.performed += OnSubmit;

    }
    private void OnEnable()
    {
        inputActions.GuessTheCardGame.Navigation.performed += OnNavigate;
        // inputActions.Navigate3D_Bcontrol.Submit.performed += OnSubmit;
        var inputAction = inputActions.GuessTheCardGame.Submition;
        inputAction.performed += OnSubmit;
        inputAction.Enable();
    }
    private void OnDisable()
    {
        inputActions.GuessTheCardGame.Navigation.performed -= OnNavigate;
        // inputActions.Navigate3D_Bcontrol.Submit.performed -= OnSubmit;
        var inputAction = inputActions.GuessTheCardGame.Submition;
        inputAction.performed -= OnSubmit;
        inputAction.Disable();
    }

    //Reference fo Buttons script
    public void EnableInputActions(bool enable)
    {
        if (enable)
        {
            inputActions.GuessTheCardGame.Enable();
        }
        else 
        {
            inputActions.GuessTheCardGame.Disable();
        }
    }

    void Start()
    {
        EnableInputActions(false);
        StartGame(); // Initialize the game when the script starts
    }

    public void StartGame()
    {
        // Initialize the game logic
        MachineNumber = Random.Range(1, 14); // Random card number between 1 and 13
        ResetCardHighlightByTurn();         // Reset card highlights
                                        
        Debug.Log($"Machine has picked card number: {MachineNumber}");
    }

    public void ResetGame()
    {
        // Reset player points and clock
        //player_Points?.ResetPoints();
        player_Clock?.ResetClock();

        // Reset turn counter and card highlights
        turnCount = 0; //every 3
        selectedCardIndexPos = 0; //HL
        foreach (GameObject card in cards)
        {
            Renderer renderer = card.GetComponent<Renderer>();
            renderer.material.color = Color.white; // Reset card colors
        }
        // Hide win panel
        winPanel.gameObject.SetActive(false);
        // Restart game logic
        StartGame();
    }

    void OnNavigate(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        if (input.x > 0) // Move right
        {
            MoveSelection(1);
        }
        else if (input.x < 0) // Move left
        {
            MoveSelection(-1);
        }
        Debug.Log("CURRENT CARD IS " + selectedCardIndexPos);
    }

    void OnSubmit(InputAction.CallbackContext context)
    {
        Debug.Log("Submit action triggered!"); // Test if the action works
        Debug.Log($"Selected Card Index: {selectedCardIndexPos}");

        if (cards == null || cards.Length == 0)
        {
            Debug.LogError("Cards array is null or empty!");
            return;
        }

        if (selectedCardIndexPos < 0 || selectedCardIndexPos >= cards.Length)
        {
            Debug.LogError($"Invalid selectedCardIndexPos: {selectedCardIndexPos}");
            return;
        }

        GameObject selectedCard = cards[selectedCardIndexPos];
        if (selectedCard == null)
        {
            Debug.LogError("Selected card is null!");
            return;
        }

        Debug.Log($"Submitting card: {selectedCard.name}");
        SelectedCardAction(selectedCard);
        // SelectedCardAction(cards[selectedCardIndexPos]);
    }

    void MoveSelection(int direction)
    {
        HighLightCard(selectedCardIndexPos, false); // Remove highlight from the previous card

        // Update the selected card index
        selectedCardIndexPos += direction;
        if (selectedCardIndexPos >= cards.Length)
        {
            selectedCardIndexPos = 0;
        }
        else if (selectedCardIndexPos < 0)
        {
            selectedCardIndexPos = cards.Length - 1;
        }

        HighLightCard(selectedCardIndexPos, true); // Highlight the new card
    }

    void HighLightCard(int index, bool highlight)
    {
        Renderer renderer = cards[index].GetComponent<Renderer>();
        renderer.material.color = highlight ? Color.yellow : Color.white;
    }

    //RED
    void HighlightMachineCard()
    {
        foreach (var card in cards)
        {
            int cardNumber = int.Parse(card.name.Replace("Card_", ""));
            if (cardNumber == MachineNumber)
            {
                Renderer renderer = card.GetComponent<Renderer>();
                renderer.material.color = Color.red; // Highlight the machine's card
                break;
            }
        }
    }

    void SelectedCardAction(GameObject clickedCard)
    {
        // Deduct points at the start of the turn
        player_Points.DeductPoints();

        // Check if the player has enough points to continue

        if (!player_Points.HasEnoughPoints(player_Points.minPoints)) 
        {
            resultText.text = "¡Not enough points to continue!";
            Invoke("LoadGameOverScene", 2f); // GameOVer Scene
            return; // Stop further processing if not enough points
        }

        // Process the player's card selection
        int cardNumber = int.Parse(clickedCard.name.Replace("Card_", ""));
        int difference = Mathf.Abs(cardNumber - MachineNumber);

        // Logic for card selection
        if (difference == 0) // Winning case
        {
            
            player_Clock.AddTime(8 * 60);
            resultText.text = "";
            winPanel.gameObject.SetActive(true);
            winText.text = "You won";
        }
        else if (difference == 1) // Close but not correct
        {
          
            player_Clock.AddTime(5 * 60);
            resultText.text = $"¡Casi aciertas! Tienes +5 minutos.";
        }
        else if (difference == 2) // No points for this guess
        {
            resultText.text = "No recibes nada esta vez.";
        }
        else if (difference <= 5) // Losing case
        {
            player_Clock.AddTime(-5 * 60);
            resultText.text = "That was Close";
        }
        else if (difference == 10) 
        {
            player_Clock.AddTime(-8*60);
            resultText.text = "Too far";
        }
        else
        {
         
            player_Clock.AddTime(-2 * 60);
            resultText.text = $"Por poco. Pierdes tiempo (-2 minutos).";
        }

        HighlightMachineCard();
        turnCount++; // Increment the turn counter
        Invoke("RestartGame", 2f); // Restart the game after a short delay
    }

    void RestartGame()
    {
        foreach (GameObject card in cards)
        {
            Renderer renderer = card.GetComponent<Renderer>();
            renderer.material.color = Color.white; // Reset card colors
        }
        winPanel.gameObject.SetActive(false);
        StartGame(); // Restart game logic
    }

    void ResetCardHighlightByTurn()
    {
        turnCount %= 3;

        if (turnCount == 0) // First turn: Highlight first card
        {
            selectedCardIndexPos = 0;
        }
        else if (turnCount == 1) // Second turn: Highlight middle card
        {
            selectedCardIndexPos = 4; // Middle index
        }
        else if (turnCount == 2) // Third turn: Highlight last card
        {
            selectedCardIndexPos = cards.Length - 1;
        }

        HighLightCard(selectedCardIndexPos, true);
    }

    void LoadGameOverScene()
    {
        SceneManager.LoadScene("CarlesTesting"); // Reemplaza "NombreDeLaEscena" con el nombre de la escena que deseas cargar
    }
}