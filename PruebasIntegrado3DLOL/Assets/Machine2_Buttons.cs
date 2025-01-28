using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Machine2_Buttons : MonoBehaviour
{
    public GameObject one_Register;
    public GameObject Game1PANEL;
    public GameObject NotYetSet;
    public Scene_Manager sceneManager;
    // Start is called before the first frame update
    void Start()
    {
        one_Register.SetActive(true);
    }
    public void XClose()
    {
        one_Register.SetActive(false);
        Game1PANEL.SetActive(false);
        sceneManager.LoadScene2();
        //00_Scenario
    }
    public void SwitchToRegisterPANEL()
    {
        Debug.Log("Switch to register panel called");
        Game1PANEL.SetActive(false);
        one_Register.SetActive(true);
    }
    public void GameLoad()
    {
        Debug.Log("Switch to GameScreen");
        Game1PANEL.SetActive(true);
        one_Register.SetActive(false);
    }
}
