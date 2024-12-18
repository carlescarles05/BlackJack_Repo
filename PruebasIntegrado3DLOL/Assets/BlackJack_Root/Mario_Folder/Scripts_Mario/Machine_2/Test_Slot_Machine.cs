using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSlot_Machine : MonoBehaviour
{
    public GameObject[] symbolPrefabs; // Array of symbol prefabs
    public GridLayoutGroup gridLayoutGroup; // The grid layout for displaying symbols

    private List<GameObject> currentSymbols = new List<GameObject>(); // List of instantiated symbols
    private int rows = 3; // Number of rows
    private int columns = 5; // Number of columns
    public Text Winning_Text; // Text component to display the winning message

    // Function to populate the grid at the start
    private void Start()
    {
        PopulateGrid();
    }

    // Function to populate the grid with symbols
    public void PopulateGrid()
    {
        Debug.Log("Initializing the grid...");

        // Destroy current symbols in the grid
        foreach (Transform child in gridLayoutGroup.transform)
        {
            Destroy(child.gameObject);
        }
        currentSymbols.Clear();

        // Generate new symbols for the grid (3x5)
        int totalSlots = rows * columns;
        for (int i = 0; i < totalSlots; i++)
        {
            int randomIndex = Random.Range(0, symbolPrefabs.Length); // Choose a random prefab
            GameObject newSymbol = Instantiate(symbolPrefabs[randomIndex], gridLayoutGroup.transform);
            currentSymbols.Add(newSymbol); // Keep track for match-checking
        }
    }

    // Function to spin the reels
    public void SpinReel()
    {
        Debug.Log("Spinning the reels...");

        // Clear the grid first
        foreach (GameObject symbol in currentSymbols)
        {
            Destroy(symbol);
        }
        currentSymbols.Clear();

        // Populate the grid with random symbols (3x5 grid)
        int maxSlots = rows * columns; // 3 rows * 5 columns
        for (int i = 0; i < maxSlots; i++)
        {
            int randomIndex = Random.Range(0, symbolPrefabs.Length);
            GameObject randomSymbol = Instantiate(symbolPrefabs[randomIndex], gridLayoutGroup.transform);

            // Get the Slot_Symbol script and log the symbol's name
            Slot_Symbol symbolScript = randomSymbol.GetComponent<Slot_Symbol>();
            if (symbolScript != null)
            {
                Debug.Log($"Spawned symbol: {symbolScript.symbolName}");
            }

            currentSymbols.Add(randomSymbol);
        }

        // Start checking results after spinning
        StartCoroutine(CheckResults());
    }

    // Coroutine to check results after a delay
    private IEnumerator CheckResults()
    {
        // Simulate delay for visual effect
        yield return new WaitForSeconds(1);

        Debug.Log("Checking results...");

        // Check for matches
        CheckMatch();
    }

    // Function to check for winning lines
    private void CheckMatch()
    {
        // Define winning lines for a 3x5 grid (horizontal and diagonals)
        List<int[]> selectedLines = new List<int[]>
        {
            new int[] { 0, 1, 2, 3, 4 },  // Horizontal line 1
            new int[] { 5, 6, 7, 8, 9 },  // Horizontal line 2
            new int[] { 10, 11, 12, 13, 14 },  // Horizontal line 3
            new int[] { 0, 5, 10 },  // Vertical line 1
            new int[] { 1, 6, 11 },  // Vertical line 2
            new int[] { 2, 7, 12 },  // Vertical line 3
            new int[] { 3, 8, 13 },  // Vertical line 4
            new int[] { 4, 9, 14 },  // Vertical line 5
            new int[] { 0, 6, 12 },  // Diagonal line 1
            new int[] { 4, 8, 12 }   // Diagonal line 2
        };

        // Loop through each line to check for matches
        foreach (int[] line in selectedLines)
        {
            List<Slot_Symbol> lineSymbols = new List<Slot_Symbol>();

            // Collect symbols in the line
            foreach (int index in line)
            {
                if (index >= currentSymbols.Count) continue; // Boundary check
                Slot_Symbol symbolScript = currentSymbols[index].GetComponent<Slot_Symbol>();
                if (symbolScript != null)
                {
                    lineSymbols.Add(symbolScript);
                }
            }

            // Check if the line is a winning line
            if (lineSymbols.Count == line.Length &&
                (lineSymbols[0].isWild || lineSymbols[0].isJackpot || Slot_Symbol.IsWinningLine(lineSymbols)))
            {
                Winning_Text.text = "You win! Line: " + string.Join(",", line);
                Debug.Log("You win! Line: " + string.Join(",", line));

                // Start Coroutine to hide the winning text after 3 seconds
                StartCoroutine(HideWinningTextAfterDelay(3f));

                return; // Exit once a winning line is found
            }
        }

        Debug.Log("No winning lines.");
    }//

    // Coroutine to hide winning text after a delay
    private IEnumerator HideWinningTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Winning_Text.text = ""; // Clear the winning message
    }
}
