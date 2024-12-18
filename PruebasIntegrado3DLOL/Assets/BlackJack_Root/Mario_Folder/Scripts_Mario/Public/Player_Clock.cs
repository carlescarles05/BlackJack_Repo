using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Clock : MonoBehaviour
{
    public int StartMinutes = 1; // Minutes set in the Inspector
    public int StartSeconds = 30; // Seconds set in the Inspector
    private int totalTime;

    public Text timer; // The UI Text to display the time

    void Start()
    {
        // Calculate the total time in seconds from the Inspector values
        totalTime = (StartMinutes * 60) + StartSeconds;
        UpdateTimer_UI_TXT();
    }

    /// <summary>
    /// Updates the timer text on the UI with the current total time.
    /// </summary>
    void UpdateTimer_UI_TXT()
    {
        int minutes = totalTime / 60;
        int seconds = totalTime % 60;
        timer.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }//

    /// <summary>
    /// Adds or subtracts time (in seconds) based on external logic.
    /// </summary>
    /// <param name="seconds">Time in seconds to add (use negative values to subtract).</param>
    public void AddTime(int seconds)
    {
        totalTime = Mathf.Max(0, totalTime + seconds); // Ensure total time doesn't go below zero
        UpdateTimer_UI_TXT(); // Update the UI
    }

    /// <summary>
    /// Resets the clock to the initial time set in the Inspector.
    /// </summary>
    public void ResetClock()
    {
        totalTime = (StartMinutes * 60) + StartSeconds; // Reset total time
        UpdateTimer_UI_TXT(); // Update the UI
    }
}
