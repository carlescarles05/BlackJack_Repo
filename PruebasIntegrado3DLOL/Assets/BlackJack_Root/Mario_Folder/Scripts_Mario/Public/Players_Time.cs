using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Players_Time : MonoBehaviour
{

    // Start is called before the first frame update
    private int elapsedTime = 0;
    public Text timer;
    private bool isTimerRunning= true;
    //
    void Start()
    {
        StartCoroutine(TimerCoroutine());
    }
    IEnumerator TimerCoroutine()
    {
        while (isTimerRunning) 
        {
            yield return new WaitForSeconds(elapsedTime);
            elapsedTime++;
            UpdateTimerUI();
        }
    }


    // Update is called once per frame
    void UpdateTimerUI()
    {
        int minutes = elapsedTime / 60;
        int seconds = elapsedTime % 60;

        timer.text = string.Format("{0:D2}:{1:D2}",minutes,seconds);
    }
    public void StopTimer()
    { 
     isTimerRunning = false;    
    }

}
