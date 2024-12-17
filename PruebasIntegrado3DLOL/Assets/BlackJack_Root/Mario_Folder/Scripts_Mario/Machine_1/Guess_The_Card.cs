using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GuessTheCard : MonoBehaviour
{
   
    [SerializeField]
    [Header("UI ELEMENTS")]
    public Player_Points player_Points;
    public Player_Clock player_Clock;
    public GameObject winPanel;
    public Text resultText;
    public Text winText;
    ///
    public GameObject[] cards;
    ///
    private int MachineNumber;
    private int selectedCardIndexPos = 0; // Selected card index
    private int turnCount = 0;            // New: Turn counter
    private GameInputActions inputActions;
    private Vector2 navigationInput;
    ///
    void Start()
    {
        StartGame();
        winPanel.gameObject.SetActive(false);
    }

    void Awake()
    {
        //KEYS INPUT
        inputActions = new GameInputActions();

        inputActions.Navigate.Navigate.performed += OnNavigate; //Navigation
        inputActions.Navigate.Submit.performed += OnSubmit;
        inputActions.Navigate.Enable();  //Enable the Navigate action map
    }

    void StartGame()
    {
        //INIT
        MachineNumber = Random.Range(1, 14); // Cards between 1 and 13 selection
        ResetCardHighlightByTurn();
        resultText.text = "Elige una carta.";
        Debug.Log($"Machine has picked card number: {MachineNumber}");
    }

    void OnNavigate(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        if (input.x > 0)  // Move right
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
        Debug.Log($"Submitted! Selected Index: {selectedCardIndexPos}");
        SelectedCardAction(cards[selectedCardIndexPos]);
    }

    void MoveSelection(int direction)
    {
        HighLightCard(selectedCardIndexPos, false);

        // Update the index
        selectedCardIndexPos += direction;

        if (selectedCardIndexPos >= cards.Length)
        {
            selectedCardIndexPos = 0;
        }
        else if (selectedCardIndexPos < 0)
        {
            selectedCardIndexPos = cards.Length - 1;
        }

        HighLightCard(selectedCardIndexPos, true);
    }

    void HighLightCard(int index, bool highlight)
    {
        Renderer renderer = cards[index].GetComponent<Renderer>();
        if (highlight)
        {
            renderer.material.color = Color.yellow;
        }
        else
        {
            renderer.material.color = Color.white;
        }
    }

    void HighlightMachineCard()
    {
        foreach (var card in cards)
        {
            int cardNumber = int.Parse(card.name.Replace("Card_", ""));
            if (cardNumber == MachineNumber)
            {
                Renderer renderer = card.GetComponent<Renderer>();
                renderer.material.color = Color.red; // machine card highlight
                break;
            }
        }
    }

    void SelectedCardAction(GameObject clickedCard)
    {
        int cardNumber = int.Parse(clickedCard.name.Replace("Card_", ""));
        int difference = Mathf.Abs(cardNumber - MachineNumber);

        if (difference == 0)
        {
            player_Points.AddPoints(+250);
            player_Clock.AddTime(8 * 60);
            resultText.text = "+8 minutos de vida";
            winPanel.gameObject.SetActive(true);
            winText.text = "You won";
        }
        else if (difference == 1)
        {
            player_Points.AddPoints(+200);
            player_Clock.AddTime(5 * 60);
            resultText.text = $"¡Casi aciertas! Tienes +5 minutos.";
        }
        else if (difference == 2)
        {
            resultText.text = "No recibes nada esta vez.";
        }
        else if (difference <= 5)
        {
            player_Points.AddPoints(-200);
            player_Clock.AddTime(-2 * 60);
            resultText.text = $"Por poco. Pierdes tiempo (-2 minutos).";
        }
        else if (difference <= 8)
        {
            player_Points.AddPoints(-30);
            player_Clock.AddTime(-5 * 60);
            resultText.text = $"Estás algo lejos. Pierdes tiempo (-5 minutos).";
        }
        else
        {
            player_Points.AddPoints(-100);
            player_Clock.AddTime(-8 * 60);
            resultText.text = $"Te alejaste demasiado. Pierdes tiempo (-8 minutos).";
        }

        HighlightMachineCard();
        turnCount++; // Increment the turn counter
        Invoke("RestartGame", 2f);
    }

    void RestartGame()
    {
        foreach (GameObject card in cards)
        {
            Renderer renderer = card.GetComponent<Renderer>();
            renderer.material.color = Color.white;
        }
        winPanel.gameObject.SetActive(false);
        StartGame(); // Reinitialize the game state
    }

    /// <summary>
    /// Resets the card highlight position based on the turn count.
    /// </summary>
    void ResetCardHighlightByTurn()
    {
        turnCount %= 3; // Cycle every 3 turns

        if (turnCount == 0) // First turn: Highlight first card
        {
            selectedCardIndexPos = 0;
        }
        else if (turnCount == 1) // Second turn: Highlight middle card
        {
            selectedCardIndexPos = 4; //cards.Length / 2;
        }
        else if (turnCount == 2) // Third turn: Highlight last card
        {
            selectedCardIndexPos = cards.Length - 1;
        }

        HighLightCard(selectedCardIndexPos, true);
    }
}
