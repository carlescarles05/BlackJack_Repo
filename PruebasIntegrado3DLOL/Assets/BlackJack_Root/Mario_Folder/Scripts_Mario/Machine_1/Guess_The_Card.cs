using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GuessTheCard : MonoBehaviour
{
    [SerializeField]
    [Header("UI ELEMENTS")]
    public Player_Points player_Points;
    public Player_Clock player_Clock;
    public GameObject winPanel;
    public Text resultText;
    public Text winText;

    public GameObject[] cards;

    private int MachineNumber;
    private int selectedCardIndexPos = 0; // Selected card index
    private int turnCount = 0;            // Turn counter
    private GameInputActions inputActions;

    void Awake()
    {
        // Initialize input actions for navigation and selection
        inputActions = new GameInputActions();
        inputActions.Navigate.Navigate.performed += OnNavigate;
        inputActions.Navigate.Submit.performed += OnSubmit;
        inputActions.Navigate.Enable();
    }

    void Start()
    {
        StartGame(); // Initialize the game when the script starts
    }

    public void StartGame()
    {
        // Initialize the game logic
        MachineNumber = Random.Range(1, 14); // Random card number between 1 and 13
        ResetCardHighlightByTurn();         // Reset card highlights
        resultText.text = "Elige una carta.";
        Debug.Log($"Machine has picked card number: {MachineNumber}");
    }

    public void ResetGame()
    {
        // Reset player points and clock
        if (player_Points != null)
        {
            player_Points.ResetPoints();
        }
        if (player_Clock != null)
        {
            player_Clock.ResetClock();
        }

        // Reset turn counter and card highlights
        turnCount = 0;
        selectedCardIndexPos = 0;
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
    }

    void OnSubmit(InputAction.CallbackContext context)
    {
        SelectedCardAction(cards[selectedCardIndexPos]);
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
        // Deduce puntos al inicio de cada turno
        player_Points.DeductPoints();

        // Verifica si el jugador tiene suficientes puntos para continuar
        if (player_Points.GetPoints() < 50) // Por ejemplo: verifica si el jugador tiene menos de 50 puntos
        {
            resultText.text = "¡No tienes suficientes puntos para continuar!";
            Invoke("LoadGameOverScene", 2f); // Llama al método para cambiar de escena después de 2 segundos
            return; // Detiene la lógica adicional si no hay suficientes puntos
        }

        // Process the player's card selection
        int cardNumber = int.Parse(clickedCard.name.Replace("Card_", ""));
        int difference = Mathf.Abs(cardNumber - MachineNumber);

        // Logic for card selection
        if (difference == 0) // Winning case
        {
            player_Points.AddPoints(250); // Add points if the player wins
            player_Clock.AddTime(8 * 60);
            resultText.text = "+8 minutos de vida";
            winPanel.gameObject.SetActive(true);
            winText.text = "You won";
        }
        else if (difference == 1) // Close but not correct
        {
            player_Points.AddPoints(200); // Add points for a close guess
            player_Clock.AddTime(5 * 60);
            resultText.text = $"¡Casi aciertas! Tienes +5 minutos.";
        }
        else if (difference == 2) // No points for this guess
        {
            resultText.text = "No recibes nada esta vez.";
        }
        else // Losing case
        {
            player_Points.AddPoints(-200); // Deduct points for a further guess
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
        turnCount %= 3; // Cycle every 3 turns

        if (turnCount == 0) // First turn: Highlight first card
        {
            selectedCardIndexPos = 0;
        }
        else if (turnCount == 1) // Second turn: Highlight middle card
        {
            selectedCardIndexPos = 4; // Example: Middle index
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
//