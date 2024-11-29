using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [System.Serializable]
    public class GameCard
    {
        public string Name;    // Nombre de la carta
        public int Value;      // Valor de la carta
        public Sprite Image;   // Imagen de la carta
    }
}
