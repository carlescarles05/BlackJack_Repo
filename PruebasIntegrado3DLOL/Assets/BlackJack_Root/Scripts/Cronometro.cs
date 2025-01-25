using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cronometro : MonoBehaviour
{
    public int startYear = 2000;
    public int endYear = 0;
    public float countdownInterval = 1f;
    public TextMeshProUGUI yearText;

    public bool isCorutineActive = false;
    public bool isPause = false;

    public int currentYear;

    // Variables para el sonido
    public AudioSource audioSource; // Fuente de audio
    public AudioClip tickSound;     // Sonido de cada segundo

    public void InteractiveCountdown(System.Action onComplete, int newStartYear = 0)
    { 
        isCorutineActive = true;
        if (newStartYear != 0)
        {
           currentYear = newStartYear;
        }
        else
        {
           currentYear = startYear;
        }      
        StartCoroutine(CountdownCoroutine(onComplete));         
    }

    public void toggleCountdown()
    {
        isPause = !isPause;
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
            if (isPause == false) currentYear--;
        }

        yearText.text = "¡Llegaste al año " + endYear + "!";
        yield return new WaitForSeconds(2f);

        onComplete?.Invoke(); // Llamar al callback cuando termine la cuenta regresiva
        BJManager.Instance.gameDead();
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
    public bool SubtractYears(int years)
    {
        if (currentYear - years <= 0) {
          currentYear = 0;
            yearText.text = currentYear.ToString();
            return true; 
        }     
        currentYear -= years;
        if (currentYear < endYear)
        {
            currentYear = endYear; // Limitar al valor mínimo (endYear)
        }
        yearText.text = currentYear.ToString(); // Actualizar el texto del contador
        return false;
    }

}
