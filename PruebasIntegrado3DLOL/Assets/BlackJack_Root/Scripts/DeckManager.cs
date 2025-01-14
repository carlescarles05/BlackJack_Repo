using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public List<Material> cardMaterials; // Materiales para representar los valores
    public Transform[] cardSpawnPosition = new Transform[7]; // Array de 7 posiciones para las cartas
    public GameObject cardPrefab;        // Prefab de la carta para mostrar en la escena
    //public Transform cardSpawnPoint;     // Punto donde se instanciará la carta
    public int cardsAlreadyDrawn;


    private void Start()
    {
        GenerateDeck();
    }

    public void GenerateDeck()
    {
        // Crear cartas con valores del 1 al 7
        for (int i = 1; i <= 7; i++)
        {
            Card newCard = new Card(i, cardMaterials[i - 1]); // Asignar material según el valor
            deck.Add(newCard);
        }

        ShuffleDeck(); // Barajar el mazo
    }

    private void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = Random.Range(0, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    public void DrawCard()
    {
        if (deck.Count == 0)
        {
            Debug.LogWarning("El mazo está vacío.");
            return;
        }

        if (cardsAlreadyDrawn >= cardSpawnPosition.Length)
        {
            Debug.LogWarning("cardsAlreadyDrawn está fuera de rango.");
            return;
        }

        Card drawnCard = deck[0];
        deck.RemoveAt(0); // Eliminar la carta del mazo

        // Instanciar el prefab y aplicar el material correspondiente
        GameObject cardInstance = Instantiate(cardPrefab, cardSpawnPosition[cardsAlreadyDrawn].position, Quaternion.identity);
        MeshRenderer renderer = cardInstance.GetComponent<MeshRenderer>();
        renderer.material = drawnCard.material;
        Debug.Log("Has robado una carta");

        cardsAlreadyDrawn++;

        if (cardsAlreadyDrawn > 7) cardsAlreadyDrawn = 0;

        // Añadir el valor de la carta como texto
        TMPro.TextMeshPro text = cardInstance.GetComponentInChildren<TMPro.TextMeshPro>();
        if (text != null)
        {
            text.text = drawnCard.value.ToString();
        }
    }
}

public class Card
{
    public int value;
    public Material material;

    public Card(int value, Material material)
    {
        this.value = value;
        this.material = material;
    }
}
