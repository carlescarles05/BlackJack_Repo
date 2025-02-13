using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class MenuPause : MonoBehaviour
{
    /*[SerializeField] private GameObject botonPausa;
    [SerializeField] private GameObject menuPausa;

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
    }*/
    [SerializeField] private GameObject botonPausa;
    [SerializeField] private GameObject menuPausa;

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
    }
}
