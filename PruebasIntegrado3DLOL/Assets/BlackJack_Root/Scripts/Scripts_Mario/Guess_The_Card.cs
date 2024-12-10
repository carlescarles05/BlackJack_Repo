using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VisualScripting;



public class GuessTheCard : MonoBehaviour
{
    //DInput
    public GameObject[] cards;
    public Text resultText;
    private int MachineNumber;
    public Player_Points player_Points;
    public Player_Clock player_Clock;
    private int selectedCardIndexPos = 0;
    private GameInputActions inputActions;

    private Vector2 navigationInput;

/// ////////////////
/// ///////
/// 

    void Start()
    {
        StartGame();
    }
    void StartGame()
    {
        //INIT
        MachineNumber = Random.Range(1, 14); // AI selects a random number between 1 and 21
        resultText.text = "Elige una carta.";
        Debug.Log($"Machine has picked card number: {MachineNumber}");
    }
    //< Updated upstream
    void Awake()

    {
        //KEYS INPUT
        inputActions = new GameInputActions();

        inputActions.Navigate.Navigate.performed += OnNavigate; //Navigation
        inputActions.Navigate.Submit.performed += OnSubmit;
        inputActions.Navigate.Enable();  //Enable the Navigate action map
    }
    void Update()
    {
       
    }

    /// <summary>
    /// ///////////// Method screen navigation
    /// </summary>

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

    // method when key down
    void OnSubmit(InputAction.CallbackContext context)
    {
        Debug.Log($"Submitted! Selected Index: {selectedCardIndexPos}");
        SelectedCardAction(cards[selectedCardIndexPos]);
    }

    /// <summary>
    /// /////////////
    /// </summary>

    void MoveSelection(int direction) 
    {
        HighLightCard(selectedCardIndexPos, false);

        //update the index
        selectedCardIndexPos += direction;

        if (selectedCardIndexPos >= cards.Length)
        {

            selectedCardIndexPos = 0;

        }
        else if (selectedCardIndexPos < 0)
        {
            selectedCardIndexPos = cards.Length-1;
        }

        HighLightCard(selectedCardIndexPos, true);
    }

    ////////////////
    /// ////////
    ////
    void HighLightCard(int index, bool highlight) 
    {
     Renderer renderer = cards[index].GetComponent<Renderer>();
        if (highlight) 
        {
            renderer.material.color = Color.yellow;
        }
        else
        {
         renderer.material.color =Color.white;
        }
    }

    //////////////
    ////////
    ////
    ///
    void HighlightMachineCard()  //Apart
    {
        foreach (var card in cards) 
        {
            int cardNumber = int.Parse(card.name.Replace("Card_", ""));
            if (cardNumber == MachineNumber)
            {
             Renderer renderer = card.GetComponent<Renderer>();
                renderer.material.color = Color.red; //machine card highlight
                break;
            }
        }
    }
    //////////////
    ////////
    ////
    ///

    void SelectedCardAction(GameObject clickedCard)
    {
        // Get the card's assigned number
        int cardNumber = int.Parse(clickedCard.name.Replace("Card_", ""));

        int difference = Mathf.Abs(cardNumber - MachineNumber);

        if (difference == 0)
        {
            player_Points.AddPoints(+250);
            player_Clock.AddTime(8 * 60);
            resultText.text = $"¡Adivinaste la carta! Tienes +8 minutos de vida. Reiniciando...";
            
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
        Invoke("RestartGame", 2f);
    }
    void RestartGame()
    {
        foreach (GameObject card in cards)
        {
        
            Renderer renderer = card.GetComponent<Renderer>();
            renderer.material.color = Color.white;
        }
        StartGame(); // Reinitialize the game state
    }
}