using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string sceneName1 ;  // Main Menu
    [SerializeField] private string sceneName2 = "01_Machine#1"; // Machine Game Scene
    [SerializeField] private string sceneName3 = "02_GuessTheCardGame"; // Card Game Scene

    public void LoadScene1() // Load Main Menu
    {
        if (!string.IsNullOrEmpty(sceneName1))
        {
            SceneManager.LoadScene(sceneName1, LoadSceneMode.Single);
        }
        else
        {
            Debug.LogError("Invalid Scene Name");
        }
    }

    public void LoadScene2() // Load Machine Game
    {
        if (!string.IsNullOrEmpty(sceneName2))
        {
            SceneManager.LoadScene(sceneName2, LoadSceneMode.Single);
        }
        else
        {
            Debug.LogError("Invalid Scene Name");
        }
    }

    public void LoadScene3() // Load Card Game
    {
        if (!string.IsNullOrEmpty(sceneName3))
        {
            SceneManager.LoadScene(sceneName3, LoadSceneMode.Single);
        }
        else
        {
            Debug.LogError("Invalid Scene Name");
        }
    }

    public void GoBackToMainScreen()
    {
        LoadScene1(); // Go back to "00_Scenario"
    }
}
