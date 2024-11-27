using UnityEngine;
using UnityEngine.UI;

public class GuessTheNumber : MonoBehaviour
{
    public InputField inputField;
    public Text resultText; 
    public Button guessButton; 

    private int AINumber; 
    private int points; 

    void Start()
    {
        guessButton.onClick.AddListener(OnGuessButtonClick); 
        StartGame();
    }

    void StartGame()
    {
        AINumber = Random.Range(1, 22); 
        points = 0; 
        resultText.text = "Elige del 1 - 21 ";
    }

    void OnGuessButtonClick()
    {
        // Verifica que el jugador haya ingresado un n�mero v�lido
        if (int.TryParse(inputField.text, out int playerGuess))
        {
            int difference = Mathf.Abs(playerGuess - AINumber); 

            if (difference == 0)
            {
                points += 8;
                resultText.text = $"�Adivinaste el n�mero! +8 minutos. Puntuaci�n: {points}. Reiniciando...";
                StartGame();
            }
            else if (difference == 1)
            {
                points += 5;
                resultText.text = $"�Casi aciertas! +5 minutos. Puntuaci�n: {points}.";
            }
            else if (difference == 2)
            {
                resultText.text = "No recibes nada esta vez.";
            }
            else if (difference <= 5)
            {
                points -= 2;
                resultText.text = $"Te alejaste un poco. -2 minutos. Puntuaci�n: {points}.";
            }
            else if (difference <= 10)
            {
                points -= 5;
                resultText.text = $"Est�s algo lejos. -5 minutos. Puntuaci�n: {points}.";
            }
            else
            {
                points -= 8;
                resultText.text = $"Te alejaste demasiado. -8 minutos. Puntuaci�n: {points}.";
            }

            inputField.text = "";
        }
        else
        {
            resultText.text = "Por favor, ingresa un n�mero v�lido.";
        }
    }
}