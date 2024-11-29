using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardCard : MonoBehaviour
{
    public Card card;  // Referencia a la clase Card que contiene los datos de la carta

    public Text cardNameText;  // Texto para mostrar el nombre de la carta
    public Image cardImage;    // Imagen para mostrar la ilustración de la carta

    void Start()
    {
        if (card != null)
        {
            UpdateCardDisplay();
        }
    }

    public void UpdateCardDisplay()
    {
        // Asegúrate de que los elementos de UI estén asignados antes de usarlos
        if (cardNameText != null)
        {
            cardNameText.text = card.Name;  // Establece el nombre en el texto
        }

        if (cardImage != null)
        {
            cardImage.sprite = card.Image;  // Establece la imagen de la carta
        }
    }
}
