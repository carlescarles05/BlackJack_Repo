/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;
using TMPro;

public class GameControl : MonoBehaviour
{
    public static event Action HandlePulled;

    [Header("TextMeshComp")]
    [SerializeField]
    private TextMeshProUGUI goodLuckText;

    [Header("Script")]
    public Player_Clock playerTime;

    [Header("Variables")]
    [SerializeField]
    private Row[] rows;

    [SerializeField]
    private Transform handle;
    private int prizeValue; // Playerclock time
    private bool resultsChecked = false;

    [Header("Input System")]
    private GameInputActions inputActions;
    private InputAction handlePullAction;

    private void Awake()
    {
        // Initialize the input actions
        inputActions = new GameInputActions();
        handlePullAction = inputActions.SlotMachine.PullHandle;
        handlePullAction.Enable();

        // Enable player clock script
        playerTime = FindObjectOfType<Player_Clock>();

        if (playerTime.timer && playerTime.timerTxtSecondary != null)
        {
            playerTime.timer.text = "";
            playerTime.timerTxtSecondary.text = "";
            playerTime.timerTxtSecondary.enabled = true; // was false
            playerTime.timer.enabled = true;
        }

        if (playerTime.timer != null)
        {
            playerTime.timer.enabled = true;
        }
    }

    void Update()
    {
        // Spinning logic
        if (!rows[0].rowStopped || !rows[1].rowStopped || !rows[2].rowStopped)
        {
            SFXManagerSMtwo.Instance.Spin();
            // Hide the earned time text while spinning
            if (playerTime.earnedTimeText != null)
            {
                playerTime.earnedTimeText.gameObject.SetActive(false);
            }
            resultsChecked = false;
        }

        // Check results once all rows have stopped spinning
        if (rows[0].rowStopped && rows[1].rowStopped && rows[2].rowStopped && !resultsChecked)
        {
            CheckResults();
        }

        // Pull handle input
        if (handlePullAction.triggered)
        {
            if (rows[0].rowStopped && rows[1].rowStopped && rows[2].rowStopped)
            {
                if (!SlotMachinePointsManager.Instance.HasEnoughPoints(200))
                {
                    SFXManagerSMtwo.Instance.NoCredit();
                    Debug.Log("Not Enough Points");
                    return;
                }

                SlotMachinePointsManager.Instance.DeductPoints(100);
                SFXManagerSMtwo.Instance.Cashed();
                StartCoroutine("PullHandle");
                ShowGoodLuckText(); // Activate good luck text when starting the spin
            }
        }
    }

    private IEnumerator PullHandle()
    {
        for (int i = 0; i < 15; i += 5)
        {
            handle.Rotate(0f, 0f, i);
            yield return new WaitForSeconds(0.1f);
        }
        HandlePulled?.Invoke();
        for (int i = 0; i < 15; i += 5)
        {
            handle.Rotate(0f, 0f, -i);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void CheckResults()
    {
        if (rows[0].stoppedSlot == "Diamond"
         && rows[1].stoppedSlot == "Diamond"
         && rows[2].stoppedSlot == "Diamond")
        {
            prizeValue = 200;
        }
        else if (rows[0].stoppedSlot == "Crown"
        && rows[1].stoppedSlot == "Crown"
        && rows[2].stoppedSlot == "Crown")
        {
            prizeValue = 400;
        }
        else if (rows[0].stoppedSlot == "Bar"
        && rows[1].stoppedSlot == "Bar"
        && rows[2].stoppedSlot == "Bar")
        {
            prizeValue = 800;
        }
        else if (rows[0].stoppedSlot == "Melon"
        && rows[1].stoppedSlot == "Melon"
        && rows[2].stoppedSlot == "Melon")
        {
            prizeValue = 600;
        }
        else if (rows[0].stoppedSlot == "Seven"
        && rows[1].stoppedSlot == "Seven"
        && rows[2].stoppedSlot == "Seven")
        {
            prizeValue = 1500;
        }
        else if (rows[0].stoppedSlot == "Cherry"
        && rows[1].stoppedSlot == "Cherry"
        && rows[2].stoppedSlot == "Cherry")
        {
            prizeValue = 3000;
        }
        else if (rows[0].stoppedSlot == "Lemon"
        && rows[1].stoppedSlot == "Lemon"
        && rows[2].stoppedSlot == "Lemon")
        {
            prizeValue = 5000;
             SFXManagerSMtwo.Instance.Jackpot();
        }

        resultsChecked = true;
        playerTime.AddYears(prizeValue);
    }

    private void ShowGoodLuckText()
    {
        goodLuckText.gameObject.SetActive(true);
        StartCoroutine(HideGoodLuckTextAfterDelay());
    }

    private IEnumerator HideGoodLuckTextAfterDelay()
    {
        yield return new WaitForSeconds(4f);
        goodLuckText.gameObject.SetActive(false);
    }
}*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;
using TMPro;

public class GameControl : MonoBehaviour
{
    public static event System.Action HandlePulled;

    [Header("TextMeshComp")]
    [SerializeField]
    private TextMeshProUGUI goodLuckText;
    [SerializeField]
    //private TextMeshProUGUI jackPOT;

    [Header("Script")]
    public Player_Clock playerTime;

    [Header("Variables")]
    [SerializeField]
    private Row[] rows;

    [SerializeField]
   // private Transform handle;
    private int prizeValue; // Playerclock time
    private bool resultsChecked = false;

    [Header("Input System")]
    private GameInputActions inputActions;
    private InputAction spinReelsAction;

    private void Awake()
    {
        // Initialize the input actions
        inputActions = new GameInputActions();
        spinReelsAction = inputActions.SlotMachine.PullHandle;
        spinReelsAction.Enable();
       // jackPOT.gameObject.SetActive(false);
        // Enable player clock script
        playerTime = FindObjectOfType<Player_Clock>();

        if (playerTime.timer && playerTime.timerTxtSecondary != null)
        {
            playerTime.timer.text = "";
            playerTime.timerTxtSecondary.text = "";
            playerTime.timerTxtSecondary.enabled = true; // was false
            playerTime.timer.enabled = true;
        }

        if (playerTime.timer != null)
        {
            playerTime.timer.enabled = true;
        }
    }

    void Update()
    {
        // Spinning logic
        if (!rows[0].rowStopped || !rows[1].rowStopped || !rows[2].rowStopped)
        {
            // Hide the earned time text while spinning
            if (playerTime.earnedTimeText != null)
            {
                playerTime.earnedTimeText.gameObject.SetActive(false);
            }
            resultsChecked = false;
        }

        // Check results once all rows have stopped spinning
        if (rows[0].rowStopped && rows[1].rowStopped && rows[2].rowStopped && !resultsChecked)
        {
            if (SFXManagerSMtwo.Instance != null)
            {
                SFXManagerSMtwo.Instance.StopSpinSound(); // Stop the spin sound
            }
            CheckResults();
        }

        // Pull handle input
        if (spinReelsAction.triggered)
        {
            if (rows[0].rowStopped && rows[1].rowStopped && rows[2].rowStopped)
            {
                if (!SlotMachinePointsManager.Instance.HasEnoughPoints(200))
                {
                    Debug.Log("Not Enough Points");
                    SFXManagerSMtwo.Instance.NoCredit();
                    return;
                }

                SlotMachinePointsManager.Instance.DeductPoints(100);
              
                //StartCoroutine("PullHandle");
                ShowGoodLuckText(); // Activate good luck text when starting the spin
                SFXManagerSMtwo.Instance.Cashed(); // Play cashed sound
                StartCoroutine("TriggerReels");
            }
        }
    }
    
   private IEnumerator TriggerReels()
    {
        yield return new WaitForSeconds(1);
        HandlePulled?.Invoke();
    }
  /*  private IEnumerator PullHandle()
    {
        for (int i = 0; i < 15; i += 5)
        {
            handle.Rotate(0f, 0f, i);
            yield return new WaitForSeconds(0.1f);
        }
        HandlePulled?.Invoke();
        for (int i = 0; i < 15; i += 5)
        {
            handle.Rotate(0f, 0f, -i);
            yield return new WaitForSeconds(0.5f);//0.1
        }
    }*/

    private void CheckResults()
    {
        if (rows[0].stoppedSlot == "Diamond"
         && rows[1].stoppedSlot == "Diamond"
         && rows[2].stoppedSlot == "Diamond")
        {
            prizeValue = 120;
        }
        else if (rows[0].stoppedSlot == "Crown"
        && rows[1].stoppedSlot == "Crown"
        && rows[2].stoppedSlot == "Crown")
        {
            prizeValue = 600;
        }
        else if (rows[0].stoppedSlot == "Bar"
        && rows[1].stoppedSlot == "Bar"
        && rows[2].stoppedSlot == "Bar")
        {
            prizeValue = 100;
        }
        else if (rows[0].stoppedSlot == "Melon"
        && rows[1].stoppedSlot == "Melon"
        && rows[2].stoppedSlot == "Melon")
        {
            prizeValue = 300;
        }
        else if (rows[0].stoppedSlot == "Seven"
        && rows[1].stoppedSlot == "Seven"
        && rows[2].stoppedSlot == "Seven")
        {
            prizeValue = 70;
        }
        else if (rows[0].stoppedSlot == "Cherry"
        && rows[1].stoppedSlot == "Cherry"
        && rows[2].stoppedSlot == "Cherry")
        {
            prizeValue = 50;
        }
        else if (rows[0].stoppedSlot == "Lemon"
        && rows[1].stoppedSlot == "Lemon"
        && rows[2].stoppedSlot == "Lemon")
        {
            prizeValue = 120;
            SFXManagerSMtwo.Instance.Jackpot(); // Play jackpot sound for lemons
         //   jackPOT.gameObject.SetActive(true);
        }

        resultsChecked = true;
        playerTime.AddYears(prizeValue);
        SFXManagerSMtwo.Instance.EarnTime(); // Play EarnTime sound
    }

    private void ShowGoodLuckText()
    {
        goodLuckText.gameObject.SetActive(true);
        StartCoroutine(HideGoodLuckTextAfterDelay());
    }

    private IEnumerator HideGoodLuckTextAfterDelay()
    {
        yield return new WaitForSeconds(4f);
        goodLuckText.gameObject.SetActive(false);
    }
}
