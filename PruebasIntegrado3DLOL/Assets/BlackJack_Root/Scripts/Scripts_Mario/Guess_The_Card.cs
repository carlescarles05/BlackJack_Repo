using UnityEngine;
using UnityEngine.UI;

public class GuessTheCard : MonoBehaviour
{
    public GameObject[] cards; // Assign the 21 card GameObjects in the Inspector
    public Text resultText;
    private int AINumber;
    public Player_Points player_Points;
    public Player_Clock player_Clock;

    void Start()
    {
        
        foreach (GameObject card in cards)
        {
            // Add a listener to each card's button component

            Button cardButton = card.GetComponent<Button>();
            cardButton.onClick.AddListener(() => OnCardClick(card));
        }
        StartGame();
    }

    void StartGame()
    {
        AINumber = Random.Range(1, 22); // AI selects a random number between 1 and 21
        resultText.text = "Elige una carta.";
        Debug.Log($"AI has picked card number: {AINumber}");
    }

    void OnCardClick(GameObject clickedCard)
    {
        // Get the card's assigned number (set via the card's name or a script)
        int cardNumber = int.Parse(clickedCard.name.Replace("Card_", ""));

        int difference = Mathf.Abs(cardNumber - AINumber);

        if (difference == 0)
        {
            player_Points.AddPoints(+250);
            player_Clock.AddTime(8 * 60);
            resultText.text = $"¡Adivinaste la carta! +8 minutos. Reiniciando...";
           
        }
        else if (difference == 1)
        {
            player_Points.AddPoints(+200);
            player_Clock.AddTime(5 * 60);
            resultText.text = $"¡Casi aciertas! +5 minutos.";
        }
        else if (difference == 2)
        {
            resultText.text = "No recibes nada esta vez.";
        }
        else if (difference <= 5)
        {
            player_Points.AddPoints(-200);
            player_Clock.AddTime(-2 * 60);
            resultText.text = $"Te alejaste un poco. -2 minutos.";
        }
        else if (difference <= 10)
        {
            player_Points.AddPoints(-30);
            player_Clock.AddTime(-5 * 60);
            resultText.text = $"Estás algo lejos. -5 minutos.";
        }
        else
        {
            player_Points.AddPoints(-100);
            player_Clock.AddTime(-8 * 60);
            resultText.text = $"Te alejaste demasiado. -8 minutos.";
        }
    }
}