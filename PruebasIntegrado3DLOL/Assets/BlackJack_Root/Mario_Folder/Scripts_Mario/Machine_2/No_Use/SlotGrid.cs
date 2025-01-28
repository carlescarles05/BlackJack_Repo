using UnityEngine;
using UnityEngine.UI; // Only if you're using UI elements

public class SlotGrid : MonoBehaviour
{
    public GameObject symbolPrefab; // Assign the prefab for your symbol (UI Image or 3D Object)
    public int rows = 3;            // Number of rows in the grid
    public int columns = 5;         // Number of columns in the grid

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Instantiate a symbol prefab for each grid cell
                GameObject symbol = Instantiate(symbolPrefab, transform);
                symbol.name = $"Symbol_{row}_{col}";

                // Optional: Set the symbol's position (if not using Grid Layout Group)
                RectTransform rect = symbol.GetComponent<RectTransform>();
                if (rect != null)
                {
                    rect.anchoredPosition = new Vector2(col * 100, -row * 100); // Adjust based on cell size
                }
            }
        }
    }
}