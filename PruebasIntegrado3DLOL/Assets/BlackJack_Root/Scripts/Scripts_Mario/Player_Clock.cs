using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Clock : MonoBehaviour
{
    public int StartMinutes = 1;
    public int startSeconds = 30;
    private int totalTime;

    public Text timer;
    private bool isTimerRunning = true;
    // Start is called before the first frame update
    void Start()
    {
        totalTime = (StartMinutes * 60) + startSeconds;
        UpdateTimer_UI_TXT();
        StartCoroutine(CountdownCoroutine());
    }
    IEnumerator CountdownCoroutine()
    {
     while(isTimerRunning && totalTime > 0) 
        {
         yield return new WaitForSeconds(1);
         totalTime--;
            UpdateTimer_UI_TXT();
            if (totalTime < 0) 
            {
                totalTime = 0;
            }
            if (totalTime <= 0) 
            {
               timer.text = string.Format("{0:D2}:{1:D2}", 00,00);
                isTimerRunning = false;
            Debug.Log("Time's up!"); //new action to trigger when time is up
            }
        }
    }

    // Update is called once per frame
    void UpdateTimer_UI_TXT()
    {
        int minutes = totalTime / 60;
        int seconds = totalTime % 60;
        timer.text = string.Format("{0:D2}:{1:D2}", minutes,seconds);
    }
    public void StopTimer() 
    {
        isTimerRunning=false;
    }
    public void AddTime(int seconds)   
    {
        totalTime = Mathf.Max(0, totalTime + seconds);
        UpdateTimer_UI_TXT();
    }
}
