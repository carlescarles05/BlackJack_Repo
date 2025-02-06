using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class MenuPause : MonoBehaviour
{
    /*[SerializeField] private GameObject botonPausa;
    [SerializeField] private GameObject menuPausa;
    public KeyCode interactKey = KeyCode.Escape; // Tecla de interacción

    [Header("Volumen")]
    [SerializeField] private Slider volumenSlider; // Slider de volumen
    private const string VolumeKey = "GameVolume"; // Clave para guardar el volumen en PlayerPrefs

    private void Start()
    {
        // Cargar el volumen guardado o establecerlo por defecto
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f); // Por defecto, volumen al 100%
        AudioListener.volume = savedVolume; // Establecer volumen del juego
        if (volumenSlider != null)
        {
            volumenSlider.value = savedVolume; // Sincronizar slider con el volumen
            volumenSlider.onValueChanged.AddListener(ActualizarVolumen); // Registrar evento
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        botonPausa.SetActive(false);
        menuPausa.SetActive(true);
    }

    public void Reanudar()
    {
        Time.timeScale = 1f;
        botonPausa.SetActive(true);
        menuPausa.SetActive(false);
    }

    public void Cerrar()
    {
        Debug.Log("Cerrando juego");
        Application.Quit();
    }

    private void ActualizarVolumen(float nuevoVolumen)
    {
        AudioListener.volume = nuevoVolumen; // Cambiar el volumen global
        PlayerPrefs.SetFloat(VolumeKey, nuevoVolumen); // Guardar en PlayerPrefs
    }*/
    [SerializeField] private GameObject botonPausa;
    [SerializeField] private GameObject menuPausa;
    public KeyCode interactKey = KeyCode.Escape; // Tecla de interacción

    [Header("Volumen")]
    [SerializeField] private Slider volumenSlider; // Slider de volumen
    private const string VolumeKey = "GameVolume"; // Clave para guardar el volumen en PlayerPrefs

    private bool juegoPausado = false; // Estado del juego

    private void Start()
    {
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
        botonPausa.SetActive(false);
    }

    public void Reanudar()
    {
        Time.timeScale = 1f;
        juegoPausado = false;
        menuPausa.SetActive(false);
        botonPausa.SetActive(true);
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
