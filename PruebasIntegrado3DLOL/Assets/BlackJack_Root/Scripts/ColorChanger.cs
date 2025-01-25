using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColorChanger : MonoBehaviour
{
    public TMP_Text text; // Referencia al texto
    public float changeSpeed = 1f; // Velocidad del cambio de color
    private Color currentColor;
    private Color targetColor;

    // Lista de colores predefinidos
    private Color[] colors = new Color[]
    {
        Color.blue, // Azul
        new Color(0.5f, 0f, 0.5f), // Morado (RGB: 128, 0, 128)
        Color.yellow, // Amarillo
        Color.red // Rojo
    };

    void Start()
    {
        if (text == null)
        {
            text = GetComponent<TMP_Text>();
        }
        currentColor = text.color; // Color inicial del texto
        targetColor = GetRandomColor(); // Generar el primer color objetivo
    }

    void Update()
    {
        // Interpola del color actual al color objetivo
        currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * changeSpeed);
        text.color = currentColor;

        // Si el color actual está muy cerca del color objetivo, genera un nuevo color objetivo
        if (Vector4.Distance(currentColor, targetColor) < 0.1f)
        {
            targetColor = GetRandomColor();
        }
    }

    // Obtiene un color aleatorio de la lista de colores predefinidos
    private Color GetRandomColor()
    {
        return colors[Random.Range(0, colors.Length)];
    }
}
