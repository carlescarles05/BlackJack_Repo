using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameMachine : MonoBehaviour
{
    public GameObject interactionUI; // UI con el botón de interacción
    public GameObject miniGameScreen; // Pantalla donde se verá el minijuego
    public RenderTexture miniGameRenderTexture; // Render Texture del minijuego
    public Camera miniGameCamera; // Cámara del minijuego
    public Renderer screenRenderer; // Pantalla de la máquina (MeshRenderer)

    private bool isPlayerNearby = false;
    private bool isMiniGameActive = false;

    private void Start()
    {
        interactionUI.SetActive(false); // Ocultar UI de interacción al inicio
        miniGameScreen.SetActive(false); // Ocultar la pantalla del minijuego
        miniGameCamera.targetTexture = miniGameRenderTexture; // Asignar la Render Texture
        screenRenderer.material.mainTexture = null; // Dejar la pantalla vacía al inicio
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            ToggleMiniGame();
        }
    }

    private void ToggleMiniGame()
    {
        isMiniGameActive = !isMiniGameActive;
        miniGameScreen.SetActive(isMiniGameActive);

        if (isMiniGameActive)
        {
            screenRenderer.material.mainTexture = miniGameRenderTexture; // Mostrar el minijuego
        }
        else
        {
            screenRenderer.material.mainTexture = null; // Ocultar el minijuego
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            interactionUI.SetActive(true); // Mostrar UI de interacción
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            interactionUI.SetActive(false); // Ocultar UI de interacción
            miniGameScreen.SetActive(false); // Ocultar el minijuego al salir
            screenRenderer.material.mainTexture = null; // Limpiar la pantalla
        }
    }
}
