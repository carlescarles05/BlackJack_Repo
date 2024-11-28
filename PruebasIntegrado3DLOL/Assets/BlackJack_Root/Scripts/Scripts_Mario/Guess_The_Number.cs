using UnityEngine;
using UnityEngine.UI;

public class GuessTheNumber : MonoBehaviour
{
    public InputField inputField;
    public Text resultText;
    public Button guessButton; 
    private int AINumber;

    public Player_Points player_Points;
    public Player_Clock player_Clock;
    void Start()
    {
        guessButton.onClick.AddListener(OnGuessButtonClick); 
        StartGame();
    }

    void StartGame()
    {
        AINumber = Random.Range(1, 22); 
    
        resultText.text = "Elige del 1 - 21 ";
    }

    void OnGuessButtonClick()
    {
        // verify valid input
        if (int.TryParse(inputField.text, out int playerGuess))
        {
            int difference = Mathf.Abs(playerGuess - AINumber); 

            if (difference == 0)
            {

                player_Points.AddPoints(+250);
                player_Clock.AddTime(8*60);
                resultText.text = $"¡Adivinaste el número! +8 minutos.Reiniciando...";
                StartGame();
            }
            else if (difference == 1)
            {
                player_Points.AddPoints(+200);
                player_Clock.AddTime(5*60);
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
                player_Clock.AddTime(-5 *60);
                resultText.text = $"Estás algo lejos. -5 minutos.";
            }
            else
            {
                player_Points.AddPoints(-100);
                player_Clock.AddTime(-8*60);
                resultText.text = $"Te alejaste demasiado. -8 minutos.";
            }

            inputField.text = "";
        }
        else
        {
            resultText.text = "enter a valid number";
        }
    }
}