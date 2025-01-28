using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartDinoGame : MonoBehaviour
{
    [SerializeField] private Button startButton;
    public bool gameStarted = false;

    void Start()
    {
        startButton.onClick.AddListener(StartMiniGame);
    }

    void Update()
    {
        if (!gameStarted) return; // No hacer nada hasta que el juego empiece
    }

    public void StartMiniGame()
    {
        gameStarted = true;
        Debug.Log("Minijuego iniciado");
    }
}
