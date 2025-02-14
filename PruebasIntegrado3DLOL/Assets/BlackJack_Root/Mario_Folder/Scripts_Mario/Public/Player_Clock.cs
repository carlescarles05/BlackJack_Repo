using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting.Antlr3.Runtime;
public class Player_Clock : MonoBehaviour
{
    //  public int StartYears;  // Initial years
    //  private int totalYears;
    [Header("TextMeshProComponent")]
    [SerializeField] public TextMeshProUGUI[] timerTexts;
    public  TextMeshProUGUI earnedTimeText;
    public TextMeshProUGUI childEarnedTimeText;
    [Header("Variable")]
    private bool isTimerActive = true;
    private Coroutine timerCoroutine;
    [Header("Script")]
    public GuessTheCard gameManager; // Reference to GuessTheCard script
    void Start()
    {
        //Earned time
        if (earnedTimeText == null)
        {
            Debug.LogError("Earned timer TextMeshPro is not assigned.");
        }
        if (childEarnedTimeText == null) 
        {
            Debug.LogWarning("ChildEarnedTime component not assigned");
        }
        else 
        {
            earnedTimeText.gameObject.SetActive(false);
        }
        gameManager = FindObjectOfType<GuessTheCard>(); // Assign reference
        if (gameManager == null)
        {
            Debug.LogWarning("Game Manger (GuessTheCard) is missing");
        }
        //totalYears = StartYears;
        UpdateTimer_UI_TXT();

        if (SlotMachinesTimeManager.Instance.TotalYears > 0)
        {
            timerCoroutine = StartCoroutine(TimerCountdown());
        }
        else
        {
            OnTimeOut();
        }
    }

    public void UpdateTimer_UI_TXT()
    {
        string timeText = $"{SlotMachinesTimeManager.Instance.TotalYears}";
        foreach (var text in timerTexts)
        {
            if (text != null)
            { 
            text.text = timeText;
                text.ForceMeshUpdate();
            }
        }
    }
    public void AddYears(int years)
    {
        SlotMachinesTimeManager.Instance.AddYears(years);
        ShowEarnedTime(years);

        StartCoroutine(earnTBftimer());
    }
    IEnumerator earnTBftimer()
    { 
     yield return new WaitForSeconds(2.2f);
        UpdateTimer_UI_TXT();
    }
    public void ShowEarnedTime(int years)
    {
        if (!gameObject.activeInHierarchy)
        {
            Debug.LogWarning("Player_Clock is inactive! Skipping coroutine.");
            return; 
        }

        if (earnedTimeText != null)
        {
            earnedTimeText.gameObject.SetActive(true);
            earnedTimeText.text = $"{years} LifePoints";
            childEarnedTimeText.text = $"{years} LifePoints";
            StartCoroutine(HideEarnedTimeAfterDelay());
        }
    }
    private IEnumerator HideEarnedTimeAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        if (earnedTimeText != null)
        {
         earnedTimeText.gameObject.SetActive(false);
        }
    }
    private IEnumerator TimerCountdown()
    {
        while (isTimerActive && SlotMachinesTimeManager.Instance.TotalYears > 0)
        {
            yield return new WaitForSeconds(4f); // Each year lasts 4 seconds
            SlotMachinesTimeManager.Instance.AddYears(-1);
            UpdateTimer_UI_TXT();

            if (SlotMachinesTimeManager.Instance.TotalYears <= 0/*totalYears <= 0*/)
            {
               // totalYears = 0;
                isTimerActive = false;
                OnTimeOut();
                yield break;
            }
        }
    }
    public void OnTimeOut() //and reset to default
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
            Debug.LogWarning("Time's up!Game Over");
        }
        bool resetImmediately = true;
        if (resetImmediately && SceneManager.GetActiveScene().name == "GameOverScene")
        {
            SlotMachinesTimeManager.Instance.ResetToDefault();
            Debug.Log("Timer reset to default.");
        }
    }

}