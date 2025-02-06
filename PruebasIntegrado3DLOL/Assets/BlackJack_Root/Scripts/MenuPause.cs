using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class MenuPause : MonoBehaviour
{
    [SerializeField] private GameObject menuPausa;
    public KeyCode interactKey = KeyCode.Escape; // Tecla de interacción
    private Cronometro cronometro;

    [Header("Volumen")]
    [SerializeField] private Slider volumenSlider; // Slider de volumen
    private const string VolumeKey = "GameVolume"; // Clave para guardar el volumen en PlayerPrefs

    private bool juegoPausado = false; // Estado del juego

    private void Start()
    {
        cronometro = FindObjectOfType<Cronometro>();

        // Cargar el volumen guardado o establecerlo por defecto
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f); // Volumen al 100% por defecto
        AudioListener.volume = savedVolume;
        if (volumenSlider != null)
        {
            volumenSlider.value = savedVolume;
            volumenSlider.onValueChanged.AddListener(ActualizarVolumen);
        }

        menuPausa.SetActive(false); // Asegurar que el menú está oculto al inicio
    }

    private void Update()
    {
        // Detectar si se presiona Escape para pausar o reanudar
        if (Input.GetKeyDown(interactKey))
        {
            if (juegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        juegoPausado = true;
        menuPausa.SetActive(true);
        cronometro.countdownInterval = 0;
    }

    public void Reanudar()
    {
        Time.timeScale = 1f;
        juegoPausado = false;
        menuPausa.SetActive(false);
        cronometro.countdownInterval = 1;
    }

    public void Cerrar()
    {
        Debug.Log("Cerrando juego");
        Application.Quit();
    }

    private void ActualizarVolumen(float nuevoVolumen)
    {
        AudioListener.volume = nuevoVolumen;
        PlayerPrefs.SetFloat(VolumeKey, nuevoVolumen);
    }
}
