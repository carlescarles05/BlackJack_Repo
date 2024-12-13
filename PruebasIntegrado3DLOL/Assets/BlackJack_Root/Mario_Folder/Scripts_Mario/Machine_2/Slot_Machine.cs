using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot_Machine : MonoBehaviour
{
    public GameObject[] symbolPrefabs; // Array of symbol prefabs
    public GridLayoutGroup gridLayoutGroup; // The grid layout for displaying symbols

    private List<GameObject> currentSymbols = new List<GameObject>(); // List of instantiated symbols

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

        // Populate the grid with random symbols
        int maxSlots = gridLayoutGroup.constraintCount * gridLayoutGroup.constraintCount; // Adjust based on your grid setup
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

    private IEnumerator CheckResults()
    {
        // Simulate delay for visual effect
        yield return new WaitForSeconds(1);

        Debug.Log("Checking results...");

        // Check for matches
        CheckMatch();
    }

    private void CheckMatch()
    {
        // Define winning lines
        List<int[]> selectedLines = new List<int[]>
        {
            new int[] { 0, 1, 2, 3, 4 },  // Horizontal line 1
            new int[] { 5, 6, 7, 8, 9 },  // Horizontal line 2
            new int[] { 10, 11, 12, 13, 14 },  // Horizontal line 3
            new int[] { 0, 6, 12, 8, 4 },  // Diagonal 1
            new int[] { 10, 6, 2, 8, 14 }  // Diagonal 2
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
                Debug.Log("You win! Line: " + string.Join(",", line));
                return; // Exit once a winning line is found
            }
        }

        Debug.Log("No winning lines.");
    }
}
