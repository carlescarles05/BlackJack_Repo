using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class Player_Clock : MonoBehaviour
{
    public int StartYears; // Años iniciales con los que empieza el temporizador
    private int totalYears; // Años totales restantes
    private TextMeshProUGUI timer; // Componente de UI que muestra el tiempo en pantalla
    private bool isTimerActive = true; // Estado del temporizador, si está activo o no
    private Coroutine timerCoroutine; // Almacena la referencia a la corrutina del temporizador

    public GuessTheCard gameManager; // Referencia al script "GuessTheCard" para manejar el Game Over


    void Start()
    {
        timer = GetComponent<TextMeshProUGUI>(); // Intenta obtener el componente de texto de la UI
        if (timer == null)
        {
            Debug.LogError("No Text component found on this GameObject.");
            return;
        }

        gameManager = FindObjectOfType<GuessTheCard>(); // Encuentra el script GuessTheCard en la escena
        if (gameManager == null)
        {
            Debug.Log("Game Manger (GuessTheCard) is missing");
        }

        totalYears = StartYears; // Inicializa el contador con los años de inicio
        UpdateTimer_UI_TXT(); // Actualiza la interfaz con los años restantes

        if (totalYears > 0)
        {
            timerCoroutine = StartCoroutine(TimerCountdown()); // Inicia la cuenta regresiva
        }
        else
        {
            OnTimeOut(); // Si el contador empieza en 0, se activa el "Game Over"
        }
    }

    void UpdateTimer_UI_TXT()
    {
        timer.text = $"{totalYears} Year(s) Remaining"; // Actualiza el texto de la UI con los años restantes
    }

    public void AddYears(int years)
    {
        totalYears = Mathf.Max(0, totalYears + years); // Asegura que no sea menor a 0
        UpdateTimer_UI_TXT(); // Refresca la UI

        if (totalYears <= 0 && isTimerActive) // Si llega a 0 y aún estaba activo...
        {
            isTimerActive = false;
            OnTimeOut(); // Activa el "Game Over"
        }
    }

    public void ResetClock()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine); // Detiene la cuenta regresiva actual
        }

        totalYears = StartYears; // Reinicia los años al valor inicial
        UpdateTimer_UI_TXT(); // Refresca la UI

        if (totalYears > 0)
        {
            isTimerActive = true;
            timerCoroutine = StartCoroutine(TimerCountdown()); // Reinicia la cuenta regresiva
        }
        else
        {
            OnTimeOut(); // Si el valor inicial ya es 0, activa "Game Over"
        }
    }

    private IEnumerator TimerCountdown()
    {
        while (isTimerActive && totalYears > 0) // Mientras el temporizador esté activo y haya años restantes...
        {
            yield return new WaitForSeconds(4f); // Espera 4 segundos (1 "año" en el juego)
            totalYears--; // Reduce los años
            UpdateTimer_UI_TXT(); // Actualiza la UI

            if (totalYears <= 0) // Si el tiempo llega a 0...
            {
                totalYears = 0;
                isTimerActive = false;
                OnTimeOut(); // Se activa el "Game Over"
                yield break; // Detiene la corrutina
            }
        }
    }

    public void OnTimeOut()
    {
        isTimerActive = false; // Detiene el temporizador

        if (timerCoroutine != null)
        {
            StopCoroutine(TimerCountdown()); // Detiene la cuenta regresiva si sigue activa
            timerCoroutine = null; // Elimina la referencia
        }

        if (gameManager != null)
        {
            gameManager.LoadGameOverScene(); // Llama al método que maneja el "Game Over"
        }
        else
        {
            Debug.LogError("Game Manager is not assigned to Player_Clock!");
        }
    }
}