using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_Slot_Machine : MonoBehaviour
{
    public GameObject[] symbolPrefabs; // Array of symbol prefabs
    public GridLayoutGroup gridLayoutGroup; // Grid layout for displaying symbols
    private List<List<GameObject>> reels = new List<List<GameObject>>(); // Separate list for each of the columns

    private int rows = 3; // Number of rows
    private int columns = 5; // Number of columns

    public Text Winning_Text; // Text component to display the winning message

    [SerializeField] public float initialSpeed = 1500f; // Initial spin speed
    [SerializeField] public float finalSpeed = 100f; // Final spin speed
    [SerializeField] public float spinDuration = 2f; // Spin duration
    [SerializeField] public float stopDelay = 0.5f; // Delay between stopping reels

    private bool isSpinning = false;

    private void Start()
    {
        FillGrid();
    }

    // Populate the grid with symbols
    public void FillGrid()
    {
        reels.Clear();

        // Destroy current symbols in the grid
        foreach (Transform child in gridLayoutGroup.transform)
        {
            Destroy(child.gameObject);
        }

        // Generate new reels and populate them with symbols
        for (int col = 0; col < columns; col++)
        {
            List<GameObject> reel = new List<GameObject>();
            for (int row = 0; row < rows; row++)
            {
                int randomIndex = Random.Range(0, symbolPrefabs.Length); // Choose a random prefab
                GameObject newSymbol = Instantiate(symbolPrefabs[randomIndex], gridLayoutGroup.transform);
                reel.Add(newSymbol); // Keep track for match-checking
            }
            reels.Add(reel);
        }
    }

    // Start spinning the reels
    public void SpinReel()
    {
        if (isSpinning) return;
        StartCoroutine(SpinAnimation());
    }

    private IEnumerator SpinAnimation()
    {
        isSpinning = true;
        float currentSpeed = initialSpeed;

        for (int col = 0; col < columns; col++)
        {
            StartCoroutine(SpinSingleReel(col, spinDuration, currentSpeed, finalSpeed));
            yield return new WaitForSeconds(stopDelay); // Delay before spinning the next reel
        }

        // Wait for all reels to stop spinning
        yield return new WaitForSeconds(spinDuration + stopDelay * (columns - 1));

        StartCoroutine(CheckResults());
        isSpinning = false;
    }

    private IEnumerator SpinSingleReel(int col, float duration, float startSpeed, float endSpeed)
    {
        float elapsedTime = 0f;
        float currentSpeed = startSpeed;

        List<GameObject> reel = reels[col];

        while (elapsedTime < duration)
        {
            currentSpeed = Mathf.Lerp(startSpeed, endSpeed, elapsedTime / duration);

            foreach (GameObject symbol in reel)
            {
                Vector3 position = symbol.transform.localPosition;
                position.y -= currentSpeed * Time.deltaTime;

                if (position.y <= -gridLayoutGroup.cellSize.y)
                {
                    position.y += gridLayoutGroup.cellSize.y * rows;
                }

                symbol.transform.localPosition = position;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Finalize positions after spinning
        foreach (GameObject symbol in reel)
        {
            Destroy(symbol);
        }

        reel.Clear();

        // Repopulate reel with final symbols
        for (int row = 0; row < rows; row++)
        {
            int randomIndex = Random.Range(0, symbolPrefabs.Length);
            GameObject newSymbol = Instantiate(symbolPrefabs[randomIndex], gridLayoutGroup.transform);
            reel.Add(newSymbol);
        }
    }

    private IEnumerator CheckResults()
    {
        yield return new WaitForSeconds(1);

        CheckMatch();
    }

    // Check for winning lines
    private void CheckMatch()
    {
        // Define winning lines for a 3x5 grid (horizontal and diagonals)
        List<int[]> winningLines = new List<int[]>
        {
            new int[] { 0, 1, 2, 3, 4 }, // Top row
            new int[] { 5, 6, 7, 8, 9 }, // Middle row
            new int[] { 10, 11, 12, 13, 14 }, // Bottom row
            new int[] { 0, 6, 12 }, // Diagonal \
            new int[] { 4, 8, 12 } // Diagonal /
        };

        List<GameObject> flattenedGrid = new List<GameObject>();
        foreach (var reel in reels)
        {
            flattenedGrid.AddRange(reel);
        }

        foreach (int[] line in winningLines)
        {
            List<Slot_Symbol> lineSymbols = new List<Slot_Symbol>();

            foreach (int index in line)
            {
                if (index >= flattenedGrid.Count) continue;

                Slot_Symbol symbolScript = flattenedGrid[index].GetComponent<Slot_Symbol>();
                if (symbolScript != null)
                {
                    lineSymbols.Add(symbolScript);
                }
            }

            if (lineSymbols.Count == line.Length && Slot_Symbol.IsWinningLine(lineSymbols))
            {
                Winning_Text.text = "You win! Line: " + string.Join(",", line);
                StartCoroutine(HideWinningTextAfterDelay(3f));
                return;
            }
        }

        Debug.Log("No winning lines.");
    }

    private IEnumerator HideWinningTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Winning_Text.text = "";
    }
}
