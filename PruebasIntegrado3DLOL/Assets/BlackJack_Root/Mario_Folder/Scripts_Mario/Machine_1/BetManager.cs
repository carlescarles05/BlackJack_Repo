using UnityEngine;

public class BetManager : MonoBehaviour
{
    public Player_Points playerPoints;
    public GuessTheCard guessTheCardScript;
    private bool hasBetBeenPlaced = false;

    void Start()
    {
        LockGame(); // Lock the game at the beginning
    }

    public void PlaceBet()
    {
        if (!playerPoints.HasEnoughPoints(playerPoints.minPoints))
        {
            Debug.Log("Not enough points to bet");
            return;
        }

        if (hasBetBeenPlaced)
        {
            Debug.Log("Bet already placed, wait for the round to end.");
            return;
        }

        // Deduct points for the bet
        playerPoints.DeductPoints();
        hasBetBeenPlaced = true;

        UnlockGame();
    }

    public void EndTurn()
    {
        hasBetBeenPlaced = false;
        LockGame();
    }

    private void LockGame()
    {
        guessTheCardScript.EnableInputActions(false);
        guessTheCardScript.enabled = false;
    }

    private void UnlockGame()
    {
        guessTheCardScript.EnableInputActions(true);
        guessTheCardScript.enabled = true;
        guessTheCardScript.StartGame(); // Start a new round after a bet
    }
}
