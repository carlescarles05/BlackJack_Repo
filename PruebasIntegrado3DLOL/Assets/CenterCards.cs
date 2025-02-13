using UnityEngine;

public class CenterCards : MonoBehaviour
{
    public Transform cardContainer; // Assign the parent GameObject for your cards

    void Start()
    {
        CenterCardContainer();
    }

    void CenterCardContainer()
    {
        // Screen width and height in world units based on camera
        float screenHalfHeight = Camera.main.orthographicSize;
        float screenHalfWidth = screenHalfHeight * Screen.width / Screen.height;

        // Center the card container
        cardContainer.position = new Vector3(0, 0, 0);

        // Optional: Scale cards to fit within the visible area
        ScaleCardsToFit(screenHalfWidth);
    }

    void ScaleCardsToFit(float screenHalfWidth)
    {
        float targetWidth = screenHalfWidth * 2f; // Full screen width
        float cardSpacing = 1.2f; // Adjust spacing between cards
        int cardCount = cardContainer.childCount;

        // Calculate scale
        float scale = targetWidth / (cardCount * cardSpacing);
        foreach (Transform card in cardContainer)
        {
            card.localScale = Vector3.one * scale; // Scale the cards evenly
        }
    }
}
