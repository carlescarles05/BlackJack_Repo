using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GuessTheCard : MonoBehaviour
{
    [Header("UI ELEMENTS")]
    public GameObject winPanel;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI winText;

    [Header("Input Actions")]
    public GuessCardInputActions inputActions;

    [Header("My Card Deck")]
    public GameObject[] cards;

    [Header("Scripts")]
    public Player_Points player_Points;
    public Player_Clock player_Clock;
    private BetManager betManager;

    [Header("Variables")]
    private int MachineNumber;
    private int selectedCardIndexPos = 0;
    private int turnCount = 0;

    void Awake()
    {
        inputActions = new GuessCardInputActions();
        inputActions.GuessTheCardGame.Navigation.performed += OnNavigate;
        inputActions.GuessTheCardGame.Submition.performed += OnSubmit;
    }

    void Start()
    {
        betManager = FindObjectOfType<BetManager>();
        if (betManager == null)
        {
            Debug.LogError("BetManager not found in scene!");
        }

        LockGame();
    }

    public void StartGame()
    {
        MachineNumber = Random.Range(1, 14);
        ResetCardHighlightByTurn();
        Debug.Log($"Machine picked card number: {MachineNumber}");
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
    public void EnableInputActions(bool enable)
    {
        if (enable)
            inputActions.GuessTheCardGame.Enable();
        else
            inputActions.GuessTheCardGame.Disable();
    }

    void OnNavigate(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        MoveSelection(input.x > 0 ? 1 : -1);
    }

    void OnSubmit(InputAction.CallbackContext context)
    {
        if (!betManager)
        {
            Debug.LogError("BetManager is missing!");
            return;
        }

        if (cards == null || cards.Length == 0) return;
        if (selectedCardIndexPos < 0 || selectedCardIndexPos >= cards.Length) return;

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

    void SelectedCardAction(GameObject clickedCard)
    {
        int cardNumber = int.Parse(clickedCard.name.Replace("Card_", ""));
        int difference = Mathf.Abs(cardNumber - MachineNumber);

        if (!player_Points.HasEnoughPoints(player_Points.minPoints))
        {
            resultText.text = "Not enough points to continue!";
            Invoke("LoadGameOverScene", 2f);
            return;
        }

        if (difference == 0) { player_Clock.AddYears(200); winPanel.SetActive(true); }
        else if (difference == 1) { player_Clock.AddYears(120); resultText.text = "So close! +120 years!"; }
        else if (difference <= 5) { player_Clock.AddYears(-50); resultText.text = "Not today!"; }
        else { player_Clock.AddYears(-80); resultText.text = "Oof! -80 years."; }

        HighlightMachineCard();
        Invoke("EndRound", 2f);
    }

    void HighlightMachineCard()
    {
        foreach (var card in cards)
        {
            if (int.Parse(card.name.Replace("Card_", "")) == MachineNumber)
            {
                card.GetComponent<Renderer>().material.color = Color.red;
                break;
            }
        }
    }

    void EndRound()
    {
        betManager.EndTurn();
        ResetCards();
    }

    void ResetCards()
    {
        foreach (GameObject card in cards)
        {
            card.GetComponent<Renderer>().material.color = Color.white;
        }
        winPanel.SetActive(false);
    }

    public void LoadGameOverScene()
    {
        SceneManager.LoadScene("CarlesTesting");
    }

    private void LockGame()
    {
        EnableInputActions(false);
        this.enabled = false;
    }
}