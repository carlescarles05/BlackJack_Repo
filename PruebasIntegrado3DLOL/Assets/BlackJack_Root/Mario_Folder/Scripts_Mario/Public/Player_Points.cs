using UnityEngine;
using UnityEngine.UI;

public class Player_Points : MonoBehaviour
{
    [Header("Points Settings")]
    bool enoughBalance = true;
    public int startingPoints ; // Starting points (default to 1000)
    public int deductionAmount ;  // Amount to deduct after each card selection (default to 50)
    public int minPoints = 50;
    public Text playerPointsText; // Reference to the Text UI element for displaying points
    private Buttons buttonsScript;
    // Initialize points at the start of the game
    void Start()
    {
        UpdatePlayerPointsText(); // Update the displayed points text
    }

    // Deduct a fixed amount of points from the player's balance
    public void DeductPoints()
    {
        startingPoints -= deductionAmount; // Deduct the specified amount (ex 50 points)
        if (startingPoints < 0) startingPoints = 0; 

        UpdatePlayerPointsText(); 
    }

    // Update the points text component
    private void UpdatePlayerPointsText()
    {
        if (playerPointsText != null)
        {
            playerPointsText.text = startingPoints.ToString(); // Set the points value in the text
           
        }
        else
        {
            Debug.LogError("Player Points Text component is not assigned!");
        }
    }

    // Check if the player has enough points to perform an action (e.g., to play the game)
    public bool HasEnoughPoints(int requiredPoints)//call on Button script
    {
        Debug.Log($"Checking points:{startingPoints}vs required{requiredPoints }");
        return startingPoints >= requiredPoints;
    }
}
