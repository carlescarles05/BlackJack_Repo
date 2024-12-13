using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Slot_Machine : MonoBehaviour
{
    
    public string Name;
    public int Rarity; //1(common) to 5(legendary)
    public int Payout;
    public bool isWild;
    public bool isJackpot;
    public GameObject[] symbolPrefabs;
    public GridLayoutGroup gridLayoutGroup;
    private List<GameObject> currentSymbols = new List<GameObject>();
    // Start is called before the first frame update
    public void SpinReel()
    {
        // Clear the grid and destroy previous symbols
        foreach (GameObject symbol in currentSymbols)
        {
            Destroy(symbol);
        }
        currentSymbols.Clear();

        // Fill the grid with new random symbols
        for (int i = 0; i < gridLayoutGroup.transform.childCount; i++)
        {
            int randomIndex = Random.Range(0, symbolPrefabs.Length); // Pick a random symbol prefab
            GameObject randomSymbol = Instantiate(symbolPrefabs[randomIndex], gridLayoutGroup.transform); // Instantiate in grid
            currentSymbols.Add(randomSymbol); // Add to the list for reference
        }

        // Check results after spinning
        StartCoroutine(CheckResults());
    }
    private IEnumerator CheckResults()
    {
        yield return new WaitForSeconds(1); // Wait for visual purposes (e.g., animations)

        Debug.Log("Checking Results...");

        // Check each line for wins
        CheckMatch();
    }
    private void CheckMatch()
    {
        // Define winning lines based on the grid indices
        List<int[]> selectedLines = new List<int[]>
        {
            new int[] { 0, 1, 2, 3, 4 },    // Top row
            new int[] { 5, 6, 7, 8, 9 },    // Middle row
            new int[] { 10, 11, 12, 13, 14 }, // Bottom row
            new int[] { 0, 6, 12, 8, 4 },  // Diagonal top-left to bottom-right
            new int[] { 10, 6, 2, 8, 14 }  // Diagonal bottom-left to top-right
        };

        foreach (int[] line in selectedLines)
        {
            List<Slot_Symbol> lineSymbols = new List<Slot_Symbol>();

            foreach (int index in line)
            {
                Slot_Symbol symbolScript = currentSymbols[index].GetComponent<Slot_Symbol>();//low --mid -- high -- wild -- jackpot
                if (symbolScript != null)
                {
                    lineSymbols.Add(symbolScript);
                }
            }

            // Check if the line is a winning line
            if (lineSymbols.Count > 0 &&
                (lineSymbols[0].isWild || lineSymbols[0].isJackpot || Slot_Symbol.IsWinningLine(lineSymbols)))
            {
                Debug.Log("You win! Line: " + string.Join(",", line));

                // Add your reward logic here
            }
        }
    }

}
