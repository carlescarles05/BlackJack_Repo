using UnityEngine;

public class SlotMachinePointsManager : MonoBehaviour
{
    public static SlotMachinePointsManager Instance { get; private set; }

    [Header("Variables")]
    public int playerPoints = 2000; // Shared Points
    public int wonPoints = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DeductPoints(int amount)
    {
        playerPoints -= amount;
        if (playerPoints < 0) playerPoints = 0;

        Debug.Log($"Updated Shared Points: {playerPoints}");

        // Notify Player_Points to update the text
        Player_Points playerPointScript = FindObjectOfType<Player_Points>();
        if (playerPointScript != null)
        {
            playerPointScript.UpdatePlayerPointsText();
        }
    }

    public bool HasEnoughPoints(int requiredPoints)
    {
        return playerPoints >= requiredPoints;
    }
  public void PointsWon(int amount)
    {

        playerPoints += amount;
        Debug.Log($"Points won! New total: {playerPoints}");
        //Update UI
        Player_Points playerPointScript = FindObjectOfType<Player_Points>();
        if (playerPointScript != null) 
        {
            playerPointScript.UpdatePlayerPointsText();
        }
    }
}
