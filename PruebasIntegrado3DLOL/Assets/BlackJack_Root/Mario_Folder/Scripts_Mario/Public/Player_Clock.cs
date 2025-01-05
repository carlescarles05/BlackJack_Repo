using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_Clock : MonoBehaviour
{
    public int StartMinutes = 1; // Minutes set in the Inspector
    public int StartSeconds = 30; // Seconds set in the Inspector
    private int totalTime;

    public TextMeshProUGUI timer; // El componente TextMeshProUGUI para mostrar el tiempo

    void Start()
    {
        // Calcular el tiempo total en segundos a partir de los valores del Inspector
        totalTime = (StartMinutes * 60) + StartSeconds;
        UpdateTimer_UI_TXT();
    }

    /// <summary>
    /// Actualiza el texto del temporizador en la UI con el tiempo actual.
    /// </summary>
    void UpdateTimer_UI_TXT()
    {
        int minutes = totalTime / 60;
        int seconds = totalTime % 60;
        timer.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }

    /// <summary>
    /// Añade o resta tiempo (en segundos) según la lógica externa.
    /// </summary>
    /// <param name="seconds">Tiempo en segundos a añadir (usar valores negativos para restar).</param>
    public void AddTime(int seconds)
    {
        totalTime = Mathf.Max(0, totalTime + seconds); // Asegurar que el tiempo total no sea menor a cero
        UpdateTimer_UI_TXT(); // Actualizar la UI
    }

    /// <summary>
    /// Restaura el reloj al tiempo inicial configurado en el Inspector.
    /// </summary>
    public void ResetClock()
    {
        totalTime = (StartMinutes * 60) + StartSeconds; // Reiniciar el tiempo total
        UpdateTimer_UI_TXT(); // Actualizar la UI
    }
}
