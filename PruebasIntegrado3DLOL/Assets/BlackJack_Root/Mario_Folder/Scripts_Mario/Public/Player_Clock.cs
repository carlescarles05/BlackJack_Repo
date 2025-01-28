using UnityEngine;
using UnityEngine.UI; // Use this namespace for legacy Text

public class Player_Clock : MonoBehaviour
{
    public int StartMinutes = 1; // Minutes set in the Inspector
    public int StartSeconds = 30; // Seconds set in the Inspector
    private int totalTime;

    private Text timer; // Legacy Unity Text component

    void Start()
    {
        // Get the Text component attached to this GameObject
        timer = GetComponent<Text>();

        // Check if the timer is null
        if (timer == null)
        {
            Debug.LogError("No Text component found on this GameObject. Make sure Player_Clock is attached to the correct object.");
            return;
        }

        // Calculate total time in seconds
        totalTime = (StartMinutes * 60) + StartSeconds;
        UpdateTimer_UI_TXT();
    }

    void UpdateTimer_UI_TXT()
    {
        int minutes = totalTime / 60;
        int seconds = totalTime % 60;
        timer.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }

    public void AddTime(int seconds)
    {
        totalTime = Mathf.Max(0, totalTime + seconds);
        UpdateTimer_UI_TXT();
    }

    public void ResetClock()
    {
        totalTime = (StartMinutes * 60) + StartSeconds;
        UpdateTimer_UI_TXT();
    }
}
