using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class Player_Clock : MonoBehaviour
{
    public int StartYears; // Initial years
    private int totalYears;
    private TextMeshProUGUI timer;
    private bool isTimerActive = true;
    private Coroutine timerCoroutine;

    public GuessTheCard gameManager; // Reference to GuessTheCard script

    void Start()
    {
        timer = GetComponent<TextMeshProUGUI>();
        if (timer == null)
        {
            Debug.LogError("No Text component found on this GameObject.");
            return;
        }

        gameManager = FindObjectOfType<GuessTheCard>(); // Assign reference
        if (gameManager == null)
        {
            Debug.Log("Game Manger (GuessTheCard) is missin");
        }
        totalYears = StartYears;
        UpdateTimer_UI_TXT();

        if (totalYears > 0)
        {
            timerCoroutine = StartCoroutine(TimerCountdown());
        }
        else
        {
            OnTimeOut();
        }
    }

    void UpdateTimer_UI_TXT()
    {
        timer.text = $"{totalYears} Year(s) Remaining";
    }

    public void AddYears(int years)
    {
        totalYears = Mathf.Max(0, totalYears + years);
        UpdateTimer_UI_TXT();

        if (totalYears <= 0 && isTimerActive)//force
        {
            isTimerActive = false;
            OnTimeOut();
        }
    }

    public void ResetClock()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        totalYears = StartYears;
        UpdateTimer_UI_TXT();
        if (totalYears > 0)
        {
            isTimerActive = true;
            timerCoroutine = StartCoroutine(TimerCountdown());
        }
        else
        {
            OnTimeOut();
        }
    }

    private IEnumerator TimerCountdown()
    {
        while (isTimerActive && totalYears > 0)
        {
            yield return new WaitForSeconds(4f); // Each year lasts 4 seconds
            totalYears--;
            UpdateTimer_UI_TXT();

            if (totalYears <= 0)
            {
                totalYears = 0;
                isTimerActive = false;
                OnTimeOut();
                yield break;
            }
        }
    }

    public void OnTimeOut()
    {
        isTimerActive = false;

        if (timerCoroutine != null)
        {
            StopCoroutine(TimerCountdown());
            timerCoroutine = null;
        }
        if (gameManager != null)
        {
            gameManager.LoadGameOverScene();
        }
        else
        {
            Debug.LogError("Game Manager is not assigned to Player_Clock!");
        }
    }
}