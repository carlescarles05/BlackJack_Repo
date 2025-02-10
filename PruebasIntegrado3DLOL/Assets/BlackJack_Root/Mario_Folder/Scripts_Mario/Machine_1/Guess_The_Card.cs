
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GuessTheCard : MonoBehaviour
{
    [Header("UI ELEMENTS")]
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI winText;

    [Header("Input Actions")]
    public GuessCardInputActions inputActions;

    [Header("My Card Deck")]
    public GameObject[] cards;

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
    }

    void Start()
    {
        resultText.gameObject.SetActive(false);
        betManager = FindObjectOfType<BetManager>();
        if (betManager == null)
        {
            Debug.LogError("BetManager not found in the scene!");
        }

        LockGame();
    }

    public void StartGame()
    {
        MachineNumber = Random.Range(1, 14);
        ResetCardHighlightByTurn();
        Debug.Log($"Machine picked card number: {MachineNumber}");
    }

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
        renderer.material.color = highlight ? Color.yellow : Color.white;
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
            if (int.Parse(card.name.Replace("Card_", "")) == MachineNumber)
            {
                card.GetComponent<Renderer>().material.color = Color.red;
               SFXManager.Instance.MachineCardSound();
                break;
            }
            else if(int.Parse(card.name.Replace("Card_", "")) == 0)
            {
                card.GetComponent<Renderer>().material.color = Color.blue;
                break;
            }
        }
    }

    void SelectedCardAction(GameObject clickedCard)
    {
        int cardNumber = int.Parse(clickedCard.name.Replace("Card_", ""));
        int difference = Mathf.Abs(cardNumber - MachineNumber);
       // cardNOne = cardNumber;
        diff = difference;
        resultText.gameObject.SetActive(true);
        if (diff == 0)
        {
            // player_Clock.AddYears(200);
            StartCoroutine(DelayedAddYears(200));
            
            resultText.text = $"You did it!!Your card is: {MachineNumber}";

        }
        else if (diff == 1)
        {
            //player_Clock.AddYears(120);
            StartCoroutine(DelayedAddYears(120));
            resultText.text = "So close! +120 years!";

        }
        else if (diff <= 5)
        {
            //player_Clock.AddYears(50);
            StartCoroutine(DelayedAddYears(50));
            resultText.text = "Not today! + 50 years";

        }
        else
        {
            StartCoroutine(DelayedAddYears(10));
            resultText.text = "Oof! +10 years.";

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
        // Step 1: Highlight the machine's card
        HighlightMachineCard();
        yield return new WaitForSeconds(2.20f); // Wait before the next step

        // Step 2: Play Earn Time sound and update earned time
        if (diff == 0)
        {
            // Winning case
            SFXManager.Instance.Win(); // Play winning sound
        }
        else
        {
            SFXManager.Instance.EarnTime(); // Play earned time sound
        }

        yield return new WaitForSeconds(1.5f); // Wait for the sound effect
        HideResultText();

        yield return new WaitForSeconds(1f);
        EndRound();
    }

    void HideResultText()
    {
        resultText.gameObject.SetActive(false);
    }
    
    void EndRound()
    {
        betManager.EndTurn();//Lock input.
        ResetCards();
    }

    void ResetCards()
    {
        foreach (GameObject card in cards)
        {
            card.GetComponent<Renderer>().material.color = Color.white;
        }
        resultText.gameObject.SetActive(false); //Hide resultText at the start

    }
    IEnumerator DelayedAddYears(int years)
    {
        yield return new WaitForSeconds(2f);
        player_Clock.AddYears(years);
    }
    private void LockGame()
    {
        EnableInputActions(false);
        this.enabled = false;
    }
    public void LoadGameOverScene()
    {
        SceneManager.LoadScene("CarlesTesting");
    }
}
