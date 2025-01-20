using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cronometro : MonoBehaviour
{
    /*public int startYear = 2000;
    public int endYear = 0;
    public float countdownInterval = 1f;
    public TextMeshProUGUI yearText;

    private int currentYear;

    public void StartCountdown(System.Action onComplete)
    {
        currentYear = startYear;
        StartCoroutine(CountdownCoroutine(onComplete));
    }

    private IEnumerator CountdownCoroutine(System.Action onComplete)
    {
        while (currentYear >= endYear)
        {
            yearText.text = currentYear.ToString();
            yield return new WaitForSeconds(countdownInterval);
            currentYear--;
        }

        yearText.text = "¡Llegaste al año " + endYear + "!";
        yield return new WaitForSeconds(2f);

        onComplete?.Invoke(); // Llamar al callback cuando termine la cuenta regresiva
    }

    // Método para sumar años al contador
    public void AddYears(int years)
    {
        currentYear += years;
        if (currentYear > startYear)
        {
            currentYear = startYear; // Limitar el máximo valor
        }
        yearText.text = currentYear.ToString(); // Actualizar el texto del contador
    }*/
    public int startYear = 2000;
    public int endYear = 0;
    public float countdownInterval = 1f;
    public TextMeshProUGUI yearText;

    private int currentYear;

    // Variables para el sonido
    public AudioSource audioSource; // Fuente de audio
    public AudioClip tickSound;     // Sonido de cada segundo

    public void StartCountdown(System.Action onComplete)
    {
        currentYear = startYear;
        StartCoroutine(CountdownCoroutine(onComplete));
    }

    private IEnumerator CountdownCoroutine(System.Action onComplete)
    {
        while (currentYear >= endYear)
        {
            yearText.text = currentYear.ToString();

            // Reproducir sonido
            if (audioSource != null && tickSound != null)
            {
                audioSource.PlayOneShot(tickSound);
            }

            yield return new WaitForSeconds(countdownInterval);
            currentYear--;
        }

        yearText.text = "¡Llegaste al año " + endYear + "!";
        yield return new WaitForSeconds(2f);

        onComplete?.Invoke(); // Llamar al callback cuando termine la cuenta regresiva
    }

    // Método para sumar años al contador
    public void AddYears(int years)
    {
        currentYear += years;
        if (currentYear > startYear)
        {
            currentYear = startYear; // Limitar el máximo valor
        }
        yearText.text = currentYear.ToString(); // Actualizar el texto del contador
    }

    // Método para restar años al contador
    public void SubtractYears(int years)
    {
        currentYear -= years;
        if (currentYear < endYear)
        {
            currentYear = endYear; // Limitar al valor mínimo (endYear)
        }
        yearText.text = currentYear.ToString(); // Actualizar el texto del contador
    }

    public void SubtractYearsEnemy(int years)
    {
        currentYear -= years;
        if (currentYear < endYear)
        {
            currentYear = endYear; // Limitar al valor mínimo (endYear)
        }
        yearText.text = currentYear.ToString(); // Actualizar el texto del contador
    }


}
