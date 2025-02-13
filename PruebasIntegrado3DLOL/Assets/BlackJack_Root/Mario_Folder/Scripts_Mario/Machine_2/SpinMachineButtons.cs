using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SpinMachineButtons : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    public GameObject gameAbout;
    [SerializeField]
    public GameObject gameControls;

    [Header("SCRIPTS")]
    private Scene_Manager sceneManager;
    
    [Header("GAME INPUT")]
    public GuessCardInputActions inputActions;

    // Start is called before the first frame update
    private void Start()
    {
        sceneManager = FindObjectOfType<Scene_Manager>();
        if (sceneManager != null)
        {
            Debug.Log("SpinMachineButons:Scene Manager script not found");
        }
        else { Debug.LogError("SMB: SCENEMANAGER SCRIPT NOT FOUND"); }
        gameAbout.gameObject.SetActive(true);
    }
    public void ShowMainScreen()//in gamabout controlss screen
    {
        
        ActiveControls();
      
    }
    public void gameScreen()//canvas 
    { 
    
    }
    public void PlayerScene()//in game scene
    {
        if (sceneManager != null)
        {
            SFXManagerSMtwo.Instance.DisableEnvironmentAudio();
            sceneManager.GoBackToMainScreen();
        }
        else 
        
        {
            Debug.LogWarning("SpinMachine call: NO scenemaneger,BUTTONS");
        }
    }

    public void ActiveControls()
    {
        StartCoroutine(DelayedControlsActivation());
    }
    IEnumerator DelayedControlsActivation()
    {
        gameControls.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        gameControls.gameObject.SetActive(false);
        gameAbout.SetActive(false);

        if (SFXManagerSMtwo.Instance != null)
        {
            SFXManagerSMtwo.Instance.EnableEnvironmentAudio();
        }
        else { Debug.LogError("SpinMachbuttons:sfxSM2 NOT FOUND:BUTTONS"); }
    }
}
