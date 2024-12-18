using UnityEngine;
using UnityEngine.UI;

public class Player_Points : MonoBehaviour
{
    [Header("Points Settings")]
    public int startingPoints = 1000; // Starting points (default to 1000)
    public int deductionAmount = 50;  // Amount to deduct after each card selection (default to 50)

    public int points; // Player's current points
    public Text playerPointsText; // Reference to the Text UI element for displaying points

    // Initialize points at the start of the game
    void Start()
    {
        points = startingPoints; // Set starting points
        UpdatePlayerPointsText(); // Update the displayed points text
    }

    // Add points to the player's balance
    public void AddPoints(int value)
    {
        points += value;
        if (points < 0) points = 0; // Prevent negative points
        UpdatePlayerPointsText(); // Update the displayed points text
    }

    // Deduct a fixed amount of points from the player's balance
    public void DeductPoints()
    {
        points -= deductionAmount; // Deduct the specified amount (e.g., 50 points)
        if (points < 0) points = 0; // Prevent negative points
        UpdatePlayerPointsText(); // Update the UI text with the new points
    }


    // Get the player's current points
    public int GetPoints()
    {
        return points;
    }
    //
    // Reset the player's points to the starting value
    public void ResetPoints()
    {
        points = startingPoints; // Reset to the starting points
        UpdatePlayerPointsText(); // Update the displayed points text
    }

    // Update the points text component
    private void UpdatePlayerPointsText()
    {
        if (playerPointsText != null)
        {
            playerPointsText.text = points.ToString(); // Set the points value in the text
        }
        else
        {
            Debug.LogError("Player Points Text component is not assigned!");
        }
    }

    // Check if the player has enough points to perform an action (e.g., to play the game)
    public bool HasEnoughPoints(int requiredPoints)
    {
        return points >= requiredPoints;
    }
}
