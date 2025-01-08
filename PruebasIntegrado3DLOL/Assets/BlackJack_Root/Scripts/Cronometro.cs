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

    public void StartCountdown(System.Action onComplete)
    {
        StartCoroutine(CountdownCoroutine(onComplete));
    }

    private IEnumerator CountdownCoroutine(System.Action onComplete)
    {
        int currentYear = startYear;
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
}
