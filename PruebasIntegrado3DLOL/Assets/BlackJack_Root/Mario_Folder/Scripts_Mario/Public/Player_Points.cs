using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_Points : MonoBehaviour
{
    [Header("Points Script Settings")]
    public int startingPoints; // Starting points (default to 1000)
    public int deductionAmount; // Amount to deduct after each card selection (default to 50)
    public int minPoints = 50;
    public TextMeshProUGUI playerPointsText; // Reference to the Text UI element for displaying points

    void Start()
    {
        UpdatePlayerPointsText();
    }

    public void DeductPoints()
    {
        startingPoints -= deductionAmount; // Deduct the specified amount
        if (startingPoints < 0) startingPoints = 0;
        UpdatePlayerPointsText();
    }

    private void UpdatePlayerPointsText()
    {
        if (playerPointsText != null)
        {
            playerPointsText.text = startingPoints.ToString();
        }
        else
        {
            Debug.LogError("Player Points Text component is not assigned!");
        }
    }

    public bool HasEnoughPoints(int requiredPoints)
    {
        Debug.Log($"Checking points: {startingPoints} vs required {requiredPoints}");
        return startingPoints >= requiredPoints;
    }
}