using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.InputSystem.HID;
using System.Collections.Generic;
using static Unity.Collections.AllocatorManager;
using UnityEngine.EventSystems;

public class GuessTheCard : MonoBehaviour
{
    [Header("UI ELEMENTS")]
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI WinText;
    public GameObject winPanel;
    public GameObject controlsImage;

    [Header("Input Actions")]
    public GuessCardInputActions inputActions;

    [Header("Cursor Settings")]
    public RectTransform cursorTransform; // Assign in Inspector (UI Image)
    public float cursorSpeed = 1000f; // Adjust speed
    private Vector2 cursorPosition;
  

    [Header("My Card Deck")]
    public GameObject[] cards;
    //cards scale
    private Dictionary<GameObject, Vector3> originalScales = new Dictionary<GameObject, Vector3>();

    [Header("Scripts")]
    public Player_Clock player_Clock;
    private BetManager betManager;

    [Header("Variables")]
    private int MachineNumber;
    private int selectedCardIndexPos = 0;
    private int turnCount = 0;
    int cardNOne;
    int diff;
    void Awake()
    {
        inputActions = new GuessCardInputActions();
        inputActions.GuessTheCardGame.Navigation.performed += OnNavigate;
        inputActions.GuessTheCardGame.Submition.performed += OnSubmit;
        /*inputActions.GuessTheCardGame.PointerMovement.performed += MoveCursor;
        inputActions.GuessTheCardGame.PointerClick.performed += Click;*/
        
    } 
    //cursor movement
  /*  void OnEnable() => inputActions.GuessTheCardGame.Enable();
    void OnDisable() => inputActions.GuessTheCardGame.Disable();
  */
    void Start()
    {
        //Text
        controlsImage.gameObject.SetActive(false);
        HideResultText();
        winPanel.SetActive(false);
        WinText.gameObject.SetActive(false);
        //
        betManager = FindObjectOfType<BetManager>();
        if (betManager == null)
        {
            Debug.LogError("BetManager not found in the scene!");
        }
        foreach (GameObject card in cards)
        {
            originalScales[card] = card.transform.localScale;
        }
       
        LockGame();
    }

    public void StartGame()
    {
        MachineNumber = Random.Range(1, 14);
        ResetCardHighlightByTurn();
        Debug.Log($"Machine picked card number: {MachineNumber}");
    }

    //GPad movement setup
   /** void MoveCursor(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        cursorPosition += input * cursorSpeed * Time.deltaTime;
        cursorTransform.anchoredPosition = cursorPosition;
    }
    void Click(InputAction.CallbackContext context)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = cursorTransform.position
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            Button button = result.gameObject.GetComponent<Button>();
            if (button != null) button.onClick.Invoke(); // Simulate click
        }
    }*/
    public void EnableInputActions(bool enable)
    {
        if (enable)
        {
            inputActions.GuessTheCardGame.Enable();
            Debug.Log("Input Actions Enabled!");
        }
        else
        {
            inputActions.GuessTheCardGame.Disable();
        }
    }
   
    void OnNavigate(InputAction.CallbackContext context)
    {

        Vector2 input = context.ReadValue<Vector2>();
        if (input.x > 0)
        {
            MoveSelection(1);
            if (SFXManager.Instance != null)
            {
                SFXManager.Instance.CardHoverSound();
            }
        }
        else if (input.x < 0)
        {
            MoveSelection(-1);
            if (SFXManager.Instance != null)
            {
                SFXManager.Instance.CardHoverSound();
            }
        }
    }

    void OnSubmit(InputAction.CallbackContext context)
    {
        if (!betManager)
        {
            Debug.LogError("BetManager is missing!");
            return;
        }

        if (cards == null || cards.Length == 0) return;

        GameObject selectedCard = cards[selectedCardIndexPos];
        if (selectedCard == null) return;
        //Inmediate lock.
        EnableInputActions(false);

        SelectedCardAction(selectedCard);
    }

    void MoveSelection(int direction)
    {
        HighLightCard(selectedCardIndexPos, false);
        selectedCardIndexPos = (selectedCardIndexPos + direction + cards.Length) % cards.Length;
        HighLightCard(selectedCardIndexPos, true);
    }

    void HighLightCard(int index, bool highlight)
    {
        Renderer renderer = cards[index].GetComponent<Renderer>();
        Material material = renderer.material;
        Transform cardTransform = cards[index].transform;//Get cards transform
        if (highlight)
        {
            material.SetColor("_EmissionColor", Color.yellow * 0.5f);
            material.EnableKeyword("_EMISSION");
            cardTransform.localScale *= 1.10f;
        }
        else 
        {
            material.SetColor("_EmissionColor",Color.black);
            cardTransform.localScale = new Vector3(0.0908f, 0.0908f, 0.0908f);
        }

    }
    void ResetCardHighlightByTurn()
    {
        if (cards == null || cards.Length == 0)
        {
            Debug.LogError("Cards array is not initialized or empty!");
            return;
        }

        turnCount %= 3;

        if (turnCount == 0) // First turn: Highlight first card
        {
            selectedCardIndexPos = 0;
        }
        else if (turnCount == 1) // Second turn: Highlight middle card
        {
            selectedCardIndexPos = cards.Length / 2;
        }
        else if (turnCount == 2) // Third turn: Highlight last card
        {
            selectedCardIndexPos = cards.Length - 1;
        }

        Debug.Log($"Highlighting card at index {selectedCardIndexPos}");

        // Ensure the selected index is within a valid range
        if (selectedCardIndexPos >= 0 && selectedCardIndexPos < cards.Length)
        {
            HighLightCard(selectedCardIndexPos, true);
        }
        else
        {
            Debug.LogError($"Invalid selectedCardIndexPos: {selectedCardIndexPos}");
        }
    }
    void HighlightMachineCard()
    {

        foreach (var card in cards)
        {
            int cardNumber = int.Parse(card.name.Replace("Card_",""));
            Renderer renderer = card.GetComponent<Renderer>();
            Material material = renderer.material;
            Transform cardTransform = card.transform;

             if (cardNumber == MachineNumber && diff !=0)
            {


                //Apply emission effects
                material.SetColor("_EmissionColor", Color.red * 0.5f); //Make it glow red
                material.EnableKeyword("_EMISSION");
                //Slightly scale up the card
                //cardTransform.localScale = Vector3.one * 1.6f;  /scale up
                SFXManager.Instance.MachineCardSound();
                break;
            }

        }
    }
    void ResetCards()
    {
        foreach (GameObject card in cards)
        {
            Renderer renderer = card.GetComponent<Renderer>();
            Material material = renderer.material;
            Transform cardTransform = card.transform;

            //Reset color and glow
            material.SetColor("_EmissionColor", Color.black);
            if (originalScales.ContainsKey(card))
            {
                cardTransform.localScale = originalScales[card];
            }

        }
        resultText.gameObject.SetActive(false); //Hide resultText at the start

    }
    void SelectedCardAction(GameObject clickedCard)
    {
        int cardNumber = int.Parse(clickedCard.name.Replace("Card_", ""));
        int difference = Mathf.Abs(cardNumber - MachineNumber);

        diff = difference;
        resultText.gameObject.SetActive(true);
        if (diff == 0) //winninf case
        {
            Renderer renderer = clickedCard.GetComponent<Renderer>();  
            Material material = renderer.material;

            material.SetColor("_EmissionColor", Color.green * 0.5f);
            material.EnableKeyword("_EMISSION");
            StartCoroutine(DelayedAddYears(200));

            resultText.text = $"You nailed it! The card was {MachineNumber}. +200 life points for your magical instincts!";
        
        
        }
        else if (diff == 1)
        {
            StartCoroutine(DelayedAddYears(120));
            resultText.text = "Not bad! This card has some serious power. +120 life points!";
        }
        else if (diff <= 5)
        {
            StartCoroutine(DelayedAddYears(50));
            resultText.text = "This card’s got potential! Maybe next turn it’ll be a winner. +50 life points! ";
        }
        else
        {
            StartCoroutine(DelayedAddYears(10));
            resultText.text = "Well... it's something! +10 life points. Try picking a stronger card next time!";
        }

        StartCoroutine(SequenceEvents());

    }
    //temporary commented out
    /*  IEnumerator SequenceEvents()
      {

          // Step 1: Highlight the machine card first

          // Wait 1.5 seconds before updating the timer and playing the sound

          // Step 2: Update the timer and play sound

          if (diff == 0)
          {
              //
              yield return new WaitForSeconds(0.5f);
              //
              SFXManager.Instance.Win();
              player_Clock.UpdateTimer_UI_TXT();
          }
          else
          {
              HighlightMachineCard();
              //
              yield return new WaitForSeconds(1.5f);
              //
              SFXManager.Instance.EarnTime();
              //
              yield return new WaitForSeconds(1f);
              player_Clock.UpdateTimer_UI_TXT();
          }

          // Step 3: Hide result text and end the round after another delay
          yield return new WaitForSeconds(0.5f);
          HideResultText();

          yield return new WaitForSeconds(0.5f);
          EndRound();
      }*/
    IEnumerator SequenceEvents()
    {
        Buttons Canvadeactive = FindObjectOfType<Buttons>();
        
        // HighlightMachineCard();
        yield return new WaitForSeconds(0.8f); // Wait before the next step

        // Step 2: Play Earn Time sound and update earned time
        if (diff == 0)
        {
            //Step 1
            // Winning case
            HighlightMachineCard();
            SFXManager.Instance.Win(); // Play winning sound
            //SlotMachinePointsManager.Instance.PointsWon();
            
            if (Canvadeactive != null)
            {
                foreach (Transform child in Canvadeactive.transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
            else 
            { Debug.LogError("GameCanvs not pulled,GTC call"); }
            winPanel.SetActive(true);
            WinText.gameObject.SetActive(true);
            SlotMachinePointsManager.Instance.PointsWon(200);
        }
        else if(diff != 0) 
        {
            // Step 1: Highlight the machine's card
            HighlightMachineCard();
           // winPanel.SetActive(false);
            //WinText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1.7f); // Wait before the next step
            SFXManager.Instance.EarnTime(); // Play earned time sound
        }
        //active main game canvas
        
        //Deactivate
        yield return new WaitForSeconds(2f); // Wait for the sound effect...
        foreach (Transform child in Canvadeactive.transform)
        { 
         child.gameObject.SetActive(true);
        }
       
        HideResultText();
        winPanel.SetActive(false);
        WinText.gameObject.SetActive(false);
        
        yield return new WaitForSeconds(1f);
        EndRound();
    }

  
    private void LockGame()
    {
        EnableInputActions(false);
        this.enabled = false;
    }
    void EndRound()
    {
        betManager.EndTurn();//Lock input.
        ResetCards();
    }

  
    IEnumerator DelayedAddYears(int years)
    {
        yield return new WaitForSeconds(1.7f);
        player_Clock.AddYears(years);
    }
    void HideResultText()
    {
        resultText.gameObject.SetActive(false);
    }

    public void LoadGameOverScene()
    {
        SceneManager.LoadScene("CarlesTesting");
    }
}//