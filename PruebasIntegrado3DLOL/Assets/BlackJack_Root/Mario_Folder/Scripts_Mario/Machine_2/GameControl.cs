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
    [Header("JACKPOT")]
    [SerializeField]
    private TextMeshProUGUI jackPOT;
    [Header("WINBLUE")]
    [SerializeField]
    public TextMeshProUGUI blutoken;
    //sprite
    [SerializeField]
    public GameObject chip;
    public GameObject GAMECANVA;
    [Header("Script")]
    public Player_Clock playerTime;
    //sprite

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
        //winning
        blutoken.gameObject.SetActive(false);
        chip.gameObject.SetActive(false);
        jackPOT.gameObject.SetActive(false);
        foreach (var text in playerTime.timerTexts)
        {
            if (text != null)
            {
                text.enabled = true;
                text.text = "";
            }
        }
    }
    private void Start()
    {
        StartCoroutine(FindPlayerClock());
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
            prizeValue = 0;
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


                if (!SlotMachinePointsManager.Instance.HasEnoughPoints(100))
                {
                    Debug.Log("Not Enough Points");
                    SFXManagerSMtwo.Instance.NoCredit();
                    return;
                }

                SlotMachinePointsManager.Instance.DeductPoints(100);

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
 

    private void CheckResults()
    {

        if (rows[0].stoppedSlot == "Diamond"
         && rows[1].stoppedSlot == "Diamond"
         && rows[2].stoppedSlot == "Diamond")
        {
            prizeValue = 300;
            SlotMachinePointsManager.Instance.PointsWon(400);
            GAMECANVA.SetActive(false);
            StartCoroutine(delayedenableblutoken());
            StartCoroutine(delayedAddyears());
            StartCoroutine(HideJackpotAfterDelay());
            StartCoroutine(HideblutokenAfterDelay());
        }
        else
        {
         if (rows[0].stoppedSlot == "Crown"
        && rows[1].stoppedSlot == "Crown"
        && rows[2].stoppedSlot == "Crown")
            {
                prizeValue = 600;
                resultsChecked = true;
                SFXManagerSMtwo.Instance.Jackpot(); // Play jackpot sound 
                jackPOT.gameObject.SetActive(true);
                StartCoroutine(HideJackpotAfterDelay());
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
                prizeValue = 120;
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

            }
            //Double prizes.
            else if (((rows[0].stoppedSlot == rows[1].stoppedSlot)
            && (rows[0].stoppedSlot == "Diamond"))

            || ((rows[0].stoppedSlot == rows[2].stoppedSlot)
            && (rows[0].stoppedSlot == "Diamond"))

            || ((rows[1].stoppedSlot == rows[2].stoppedSlot)
            && (rows[1].stoppedSlot == "Diamond")))

            {
                prizeValue = 0;
            }

            else if (((rows[0].stoppedSlot == rows[1].stoppedSlot)
            && (rows[0].stoppedSlot == "Crown"))

            || ((rows[0].stoppedSlot == rows[2].stoppedSlot)
            && (rows[0].stoppedSlot == "Crown"))

            || ((rows[1].stoppedSlot == rows[2].stoppedSlot)
            && (rows[1].stoppedSlot == "Crown")))
            {
                prizeValue = 0;

            }
            else if (((rows[0].stoppedSlot == rows[1].stoppedSlot)
            && (rows[0].stoppedSlot == "Melon"))

            || ((rows[0].stoppedSlot == rows[2].stoppedSlot)
            && (rows[0].stoppedSlot == "Melon"))

            || ((rows[1].stoppedSlot == rows[2].stoppedSlot)
            && (rows[1].stoppedSlot == "Melon")))
            {
               // prizeValue = 0;
            }
            else if (((rows[0].stoppedSlot == rows[1].stoppedSlot)
            && (rows[0].stoppedSlot == "Bar"))

            || ((rows[0].stoppedSlot == rows[2].stoppedSlot)
            && (rows[0].stoppedSlot == "Bar"))

            || ((rows[1].stoppedSlot == rows[2].stoppedSlot)
            && (rows[1].stoppedSlot == "Bar")))
            {
                //prizeValue = 0;
            }
            else if (((rows[0].stoppedSlot == rows[1].stoppedSlot)
            && (rows[0].stoppedSlot == "Seven"))

            || ((rows[0].stoppedSlot == rows[2].stoppedSlot)
            && (rows[0].stoppedSlot == "Seven"))

            || ((rows[1].stoppedSlot == rows[2].stoppedSlot)
            && (rows[1].stoppedSlot == "Seven")))
            {
               // prizeValue = 0;
            }
            else if (((rows[0].stoppedSlot == rows[1].stoppedSlot)
            && (rows[0].stoppedSlot == "Cherry"))

            || ((rows[0].stoppedSlot == rows[2].stoppedSlot)
            && (rows[0].stoppedSlot == "Cherry"))

            || ((rows[1].stoppedSlot == rows[2].stoppedSlot)
            && (rows[1].stoppedSlot == "Cherry")))
            {
               // prizeValue = 0;
            }
            else if (((rows[0].stoppedSlot == rows[1].stoppedSlot)
             && (rows[0].stoppedSlot == "Lemon")

            || (rows[0].stoppedSlot == rows[2].stoppedSlot)
            && (rows[0].stoppedSlot == "Lemon")

            || (rows[1].stoppedSlot == rows[2].stoppedSlot)
            && (rows[1].stoppedSlot == "Lemon")))
            {
               // prizeValue = 0;
            }
        }
        resultsChecked = true;
     
        playerTime.AddYears(prizeValue);
        SFXManagerSMtwo.Instance.EarnTime(); // Play EarnTime sound
    }
    
    private IEnumerator delayedAddyears()//if winning blutoken
    {
        yield return new WaitForSeconds(3f);
        playerTime.AddYears(prizeValue);
    }
    private IEnumerator FindPlayerClock()
    {
        yield return new WaitForSeconds(0.1f);
        playerTime = FindObjectOfType<Player_Clock>();
        if (playerTime != null) 
        {
            Debug.Log("Player_Clock found successfully!");
        }
        else 
        {
            Debug.LogError("Player_Clock not found!");
        }
     
    }
        private IEnumerator HideJackpotAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        jackPOT.gameObject.SetActive(false);
    }
    private IEnumerator HideblutokenAfterDelay()
    {
       
        yield return new WaitForSeconds(3f);
        //
        blutoken.gameObject.SetActive(false);
        chip.gameObject.SetActive(false); 
        //main canvas is re enabled
        GAMECANVA.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        foreach (var text in playerTime.timerTexts)
        {
            if (text != null) 
            {
            text.gameObject.SetActive(true);
            }
            if(playerTime.earnedTimeText!=null)
            {
                playerTime.earnedTimeText.gameObject.SetActive(true);
            }
        }
    }
    private IEnumerator delayedenableblutoken()
    {
        yield return new WaitForSeconds(1.5f);
        blutoken.gameObject.SetActive(true);
        chip.gameObject.SetActive(true);
    }
    //
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
