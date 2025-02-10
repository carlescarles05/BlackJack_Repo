using UnityEngine;

public class BetManager : MonoBehaviour
{
    public GuessTheCard guessTheCardScript;
    private bool hasBetBeenPlaced = false;

    void Start()
    {
        Debug.Log("BetManager started. Game not locked yet.");
    }

    public void PlaceBet()
    {
        if (!SlotMachinePointsManager.Instance.HasEnoughPoints(200))
        {
            Debug.Log("Not enough points to bet");
           // LockGame();

            return;
        }

        if (hasBetBeenPlaced)
        {
            Debug.Log("Bet already placed, wait for the round to end.");
            return;
        }
        
        SlotMachinePointsManager.Instance.DeductPoints(200); // Instantly deduct points
       if (SFXManager.Instance != null)
        {
            Debug.Log("Bet placed! Playing deduct_points sound NOW...");
            SFXManager.Instance.Cashed();
        }
        else
        {
            Debug.LogError("SFXManager.Instance is null!");
        }

        hasBetBeenPlaced = true;
        UnlockGameNInput();

    }

    public void EndTurn()
    {
        hasBetBeenPlaced = false;

        // Check AFTER the round ends if the player has enough points
        if (!SlotMachinePointsManager.Instance.HasEnoughPoints(200))
        {
            Debug.Log("Not enough points left after the round. Locking input.");
            SFXManager.Instance.NoCredit();
            InitializeGame(); // Only lock the game if the player is out of points AFTER the round
        }
        else
        {
            Debug.Log("Enough points for next round. Keeping input unlocked.");
           LockGame(); // Regular lock at the end of the round
        }
    }

    public void InitializeGame()
    {
        LockGame();
    }

    private void LockGame()
    {
        guessTheCardScript.EnableInputActions(false);
        guessTheCardScript.enabled = false;
        Debug.Log("Game Locked - GuessTheCard script disabled");
    }

    private void UnlockGameNInput()
    {
        guessTheCardScript.EnableInputActions(true);
        guessTheCardScript.enabled = true;
        guessTheCardScript.StartGame(); // Start a new round after a bet
        Debug.Log("Game Unlocked and GuessTheCard script enabled!");
    }
}
