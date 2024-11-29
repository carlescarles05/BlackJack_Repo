using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : MonoBehaviour
{
    public string Name;    // Nombre de la carta
    public int Value;      // Valor de la carta
    public string Suit;    // Palo de la carta
    public Sprite Image;   // Imagen de la carta
}
