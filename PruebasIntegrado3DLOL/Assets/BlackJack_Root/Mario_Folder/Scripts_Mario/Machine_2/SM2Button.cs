using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SM2Button : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string sceneName1;  // Main Menu
                                                 // Start is called before the first frame update
    public GameObject SpinningMachineCanvas;
  
    public void LoadScene1() // Load Main Menu
    {
        if (!string.IsNullOrEmpty(sceneName1))
        {
            SceneManager.LoadScene(sceneName1, LoadSceneMode.Single);
            SpinningMachineCanvas.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Invalid Scene Name");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
