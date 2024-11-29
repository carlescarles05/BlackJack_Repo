using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class Card : MonoBehaviour
    {
        public string Name;   // Nombre de la carta (e.g., "2 de Corazones")
        public int Value;     // Valor de la carta (2-10, 10 para figuras, 1 o 11 para As)
        public string Suit;   // Palo de la carta (e.g., Corazones, Picas)
        public Sprite Image;  // Imagen visual de la carta
    }

