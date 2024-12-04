using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class GuessTheCard : MonoBehaviour
{
    public GameObject[] cards;
    public Text resultText;
    private int MachineNumber;
    public Player_Points player_Points;
    public Player_Clock player_Clock;
    private int selectedCardIndex = 0;

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        // Check for a left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Cast a ray from the camera to where the user clicked
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the clicked object is one of the cards
                foreach (GameObject card in cards)
                {
                    if (hit.collider.gameObject == card)
                    {
                        OnCardClick(card); // Trigger the click logic
                        return;
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("Horizontal") > 0)
        {
            MoveSelection(1); // Move to the next card
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") < 0)
        {
            MoveSelection(-1); // Move to the previous card
        }

        // Confirm selection with Enter or gamepad A button
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Submit"))
        {
            OnCardClick(cards[ selectedCardIndex ]); // Trigger the card logic
        }
    }
    void MoveSelection(int direction) 
    {
        HighLightCard(selectedCardIndex,false);

        //update the index
        selectedCardIndex += direction;

        if (selectedCardIndex >= cards.Length)
        {

            selectedCardIndex = 0;

        }
        else if (selectedCardIndex <0)
        {
         selectedCardIndex = cards.Length-1;
        }

        HighLightCard(selectedCardIndex, true);
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
         renderer.material.color =Color.white;
        }
    }
    void StartGame()
    {
        MachineNumber = Random.Range(1, 22); // AI selects a random number between 1 and 21
        resultText.text = "Elige una carta.";
        Debug.Log($"Machine has picked card number: {MachineNumber}");
    }

    void RestartGame()
    {
        StartGame(); // Reinitialize the game state
    }

    void OnCardClick(GameObject clickedCard)
    {
        // Get the card's assigned number
        int cardNumber = int.Parse(clickedCard.name.Replace("Card_", ""));

        int difference = Mathf.Abs(cardNumber - MachineNumber);

        if (difference == 0)
        {
            player_Points.AddPoints(+250);
            player_Clock.AddTime(8 * 60);
            resultText.text = $"¡Adivinaste la carta! Tienes +8 minutos de vida. Reiniciando...";
            Invoke("RestartGame", 2f);
        }
        else if (difference == 1)
        {
            player_Points.AddPoints(+200);
            player_Clock.AddTime(5 * 60);
            resultText.text = $"¡Casi aciertas! Tienes +5 minutos.";
            Invoke("RestartGame", 2f); 
        }
        else if (difference == 2)
        {
            resultText.text = "No recibes nada esta vez.";
            Invoke("RestartGame", 2f); 
        }
        else if (difference <= 5)
        {
            player_Points.AddPoints(-200);
            player_Clock.AddTime(-2 * 60);
            resultText.text = $"Por poco. Pierdes tiempo (-2 minutos).";
            Invoke("RestartGame", 2f); 
        }
        else if (difference <= 10)
        {
            player_Points.AddPoints(-30);
            player_Clock.AddTime(-5 * 60);
            resultText.text = $"Estás algo lejos. Pierdes tiempo (-5 minutos).";
            Invoke("RestartGame", 2f); 
        }
        else
        {
            player_Points.AddPoints(-100);
            player_Clock.AddTime(-8 * 60);
            resultText.text = $"Te alejaste demasiado. Pierdes tiempo (-8 minutos).";
            Invoke("RestartGame", 2f); 
        }
    }
}