using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public List<Material> cardMaterials; // Materiales para representar los valores
    public Transform[] cardSpawnPosition = new Transform[7]; // Array de 7 posiciones para las cartas
    public Transform[] cardSpawnPositionEnemy = new Transform[7]; // Array de 7 posiciones para las cartas
    public GameObject cardPrefab;        // Prefab de la carta para mostrar en la escena
    public int cardsAlreadyDrawn;
    public int cardsAlreadyDrawnEnemy;
    public int cardV;



    private void Start()
    {
        GenerateDeck();
    }

    public void GenerateDeck()
    {
        // Crear cartas con valores del 1 al 7
        for (int i = 1; i <= 11; i++)
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

    /*public void DrawCard()
    {
        if (deck.Count == 0)
        {
            Debug.LogWarning("El mazo está vacío.");
            return;
        }

        if (cardsAlreadyDrawn >= cardSpawnPosition.Length)
        {
            Debug.LogWarning("No hay más posiciones disponibles para las cartas.");
            return;
        }

        if (cardSpawnPosition[cardsAlreadyDrawn] == null)
        {
            Debug.LogError($"La posición {cardsAlreadyDrawn} no está asignada en el Inspector.");
            return;
        }

        // Obtener la carta
        Card drawnCard = deck[0];
        deck.RemoveAt(0); // Eliminar la carta del mazo

        // Instanciar el prefab en la posición correspondiente
        GameObject cardInstance = Instantiate(cardPrefab, cardSpawnPosition[cardsAlreadyDrawn].position, Quaternion.identity);

        // Aplicar el material correspondiente al valor de la carta
        MeshRenderer renderer = cardInstance.GetComponent<MeshRenderer>();
        if (renderer != null && drawnCard.value >= 1 && drawnCard.value <= cardMaterials.Count)
        {
            renderer.material = cardMaterials[cardV - 1]; // Asignar el material correcto
        }
        else
        {
            Debug.LogWarning($"Material no encontrado para el valor {drawnCard.value}.");
        }

        // Incrementar el contador
        cardsAlreadyDrawn++;

        // Añadir el valor de la carta como texto
        TMPro.TextMeshPro text = cardInstance.GetComponentInChildren<TMPro.TextMeshPro>();
        if (text != null)
        {
            text.text = drawnCard.value.ToString();
        }
    }*/

    public void DrawCard()
    {
        if (deck.Count == 0)
        {
            Debug.LogWarning("El mazo está vacío.");
            return;
        }

        if (cardsAlreadyDrawn >= cardSpawnPosition.Length)
        {
            Debug.LogWarning("No hay más posiciones disponibles para las cartas.");
            return;
        }

        if (cardSpawnPosition[cardsAlreadyDrawn] == null)
        {
            Debug.LogError($"La posición {cardsAlreadyDrawn} no está asignada en el Inspector.");
            return;
        }

        // Obtener la carta
        Card drawnCard = deck[0];
        //deck.RemoveAt(0); // Eliminar la carta del mazo

        // Instanciar el prefab en la posición correspondiente
        GameObject cardInstance = Instantiate(cardPrefab, cardSpawnPosition[cardsAlreadyDrawn].position, Quaternion.identity);

        // Aplicar el material correspondiente al valor de la carta
        MeshRenderer renderer = cardInstance.GetComponent<MeshRenderer>();
        if (renderer != null && drawnCard.value >= 1 && drawnCard.value <= cardMaterials.Count)
        {
            renderer.material = cardMaterials[cardV - 1]; // Asignar el material correcto
        }
        else
        {
            Debug.LogWarning($"Material no encontrado para el valor {drawnCard.value}.");
        }

        // Incrementar el contador
        cardsAlreadyDrawn++;

        // Añadir el valor de la carta como texto
        TMPro.TextMeshPro text = cardInstance.GetComponentInChildren<TMPro.TextMeshPro>();
        if (text != null)
        {
            text.text = drawnCard.value.ToString();
        }
    }

    public void DrawCardEnemy()
    {
        if (deck.Count == 0)
        {
            Debug.LogWarning("El mazo está vacío.");
            return;
        }

        if (cardsAlreadyDrawnEnemy >= cardSpawnPositionEnemy.Length)
        {
            Debug.LogWarning("No hay más posiciones disponibles para las cartas.");
            return;
        }

        if (cardSpawnPositionEnemy[cardsAlreadyDrawnEnemy] == null)
        {
            Debug.LogError($"La posición {cardsAlreadyDrawnEnemy} no está asignada en el Inspector.");
            return;
        }

        // Obtener la carta
        Card drawnCard = deck[0];
        //deck.RemoveAt(0); // Eliminar la carta del mazo

        // Instanciar el prefab en la posición correspondiente
        GameObject cardInstance = Instantiate(cardPrefab, cardSpawnPositionEnemy[cardsAlreadyDrawnEnemy].position, Quaternion.identity);

        // Aplicar el material correspondiente al valor de la carta
        MeshRenderer renderer = cardInstance.GetComponent<MeshRenderer>();
        if (renderer != null && drawnCard.value >= 1 && drawnCard.value <= cardMaterials.Count)
        {
            renderer.material = cardMaterials[cardV - 1]; // Asignar el material correcto
        }
        else
        {
            Debug.LogWarning($"Material no encontrado para el valor {drawnCard.value}.");
        }

        // Incrementar el contador
        cardsAlreadyDrawnEnemy++;

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