using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DinoManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private float initialScrollSpeed;

    private int score;
    private float timer;
    private float scrollSpeed;
    public static DinoManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        UpdateScore();
        UpdateSpeed();
    }

    public void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    private void UpdateScore()
    {
        int scorePerSeconds = 10;

        timer += Time.deltaTime;
        score = (int)(timer * scorePerSeconds);
        scoreText.text = string.Format("{0:00000}", score);
    }

    public float GetScrollSpeed()
    {
        return scrollSpeed;
    }

    private void UpdateSpeed()
    {
        float speedDivider = 10f;
        scrollSpeed = initialScrollSpeed + timer / speedDivider;
    }

}
