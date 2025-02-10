using UnityEngine;
using TMPro;

public class Player_Points : MonoBehaviour
{
    [Header("Points Script Settings")]
    public int deductAmount = 100; // Set the amount to deduct per bet
    public TextMeshProUGUI playerPointsText;
    public TextMeshProUGUI playerPointsTextSecondary;

    void Start()
    {
        if (playerPointsText == null)
        {
            Debug.LogError("Player Points Text is not assigned in the Inspector!");
        }
        else
        {
            UpdatePlayerPointsText();
        }
    }
    //Guess the card, specific
    public void DeductPoints()
    {
        if (SlotMachinePointsManager.Instance.HasEnoughPoints(deductAmount))
        {
            SlotMachinePointsManager.Instance.DeductPoints(deductAmount); // Instantly deducts points
            UpdatePlayerPointsText();
        }
        else
        {
            Debug.Log("Not enough points to continue.");
            SFXManager.Instance.NoCredit();
        }
    }

    public void UpdatePlayerPointsText()
    {
        if (playerPointsText != null)
        {
            playerPointsText.text = SlotMachinePointsManager.Instance.playerPoints.ToString();
            Debug.Log($"Player Points Updated: {SlotMachinePointsManager.Instance.playerPoints}"); 
        }

        if (playerPointsTextSecondary != null)
        {
            playerPointsTextSecondary.text = SlotMachinePointsManager.Instance.playerPoints.ToString();
        }
    }
}

