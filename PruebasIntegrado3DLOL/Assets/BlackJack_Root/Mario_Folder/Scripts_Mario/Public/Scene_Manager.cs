using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Scene_Manager : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string sceneName1;
    [SerializeField] private string sceneName2;
    [SerializeField] private string sceneName3;

    // Start is called before the first frame update
    public void LoadScene1() 
    {
        if (!string.IsNullOrEmpty(sceneName1))
        {
            SceneManager.LoadScene(sceneName1,LoadSceneMode.Single);
            
        }
        else
        {
            Debug.Log("Invalid Scene Name");
        }
    }
    public void LoadScene2()
    {
        if (!string.IsNullOrEmpty(sceneName2))
        {
            SceneManager.LoadScene(sceneName2, LoadSceneMode.Single);
        }
        else 
        {
            Debug.Log("Invalid Scene Name");
        }
    }
    private void LoadMENU() 
    {
    if (string.IsNullOrEmpty(sceneName3))
        {
            SceneManager.LoadScene(sceneName3,LoadSceneMode.Single);
        }
    
    else
    {
     Debug.Log("Invalid SceneName");
    }
}

}
