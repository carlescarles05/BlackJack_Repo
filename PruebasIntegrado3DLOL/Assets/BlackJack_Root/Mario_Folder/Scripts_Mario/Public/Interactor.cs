using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Interactor : MonoBehaviour
{
    public Camera playerCamera;
  //  public GameObject MainMenuPanel;       // Panel for the menu to display
    public GameObject SM1interactionUI;
    public GameObject SM2interactionUI;
    // UI that shows interaction prompt
    public float raycastDistance = 5f;    // Distance to check for interactable objects
    public LayerMask interactableLayer;   // Layer for interactable objects

    private Transform currentTarget;      // Current target object in view. TRANSFORM
    private bool isInteractionEnabled = false;
    public Scene_Manager sceneManager;
    [SerializeField] private InputActionAsset gameInputActions;  // Reference to InputActionAsset
    private InputAction interactAction;

    private void Awake()
    {
        sceneManager = FindObjectOfType<Scene_Manager>();
        if (sceneManager == null)
        {
            Debug.LogError("Scene_Manager script not found in the scene.");
        }
        if (gameInputActions == null)
        {
            Debug.LogError("InputActionAsset is not assigned in the inspector.");
            return;
        }

        // Find the "XboxControl" action map 
        var actionMap = gameInputActions.FindActionMap("XboxControl");
        if (actionMap == null)
        {
            Debug.LogError("ActionMap 'XboxControl' not found in InputActionAsset.");
            return;
        }
        // and "Interact" action
        interactAction = actionMap.FindAction("Interact");
        if (interactAction == null)
        {
            Debug.LogError("Action 'Interact' not found in 'XboxControl' action map.");
            return;
        }

        interactAction.performed += OnInteractPerformed;  // Subscribe to input interaction event
    }

    private void OnEnable()
    {
        interactAction?.Enable(); // Enable the interact action
    }

    private void OnDisable()
    {
        interactAction?.Disable(); // Disable the interact action
        interactAction.performed -= OnInteractPerformed; // Unsubscribe
    }

    private void Update()
    {//
        // Cast a ray from the center of the screen
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)); //Set
       
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, interactableLayer))
        {


            if (hit.transform.CompareTag("Machine1")) //Then
            {

                if (currentTarget != hit.transform)
                {

                    ShowInteractionUI(hit.transform);
                    currentTarget = hit.transform;  // Update the current target
                }
               
            }
           
            ///////////////
            else if (hit.transform.CompareTag("Machine2"))
            {
                if (currentTarget != hit.transform)
                {
                    ShowInteractionUI(hit.transform);
                    currentTarget = hit.transform;
                }
            }
            else
            {
                HideInteractionUI();  //no tag matched,hide UI
                currentTarget = null;
            }
        }
        else
        {
            HideInteractionUI();
            currentTarget = null;
        }
        ////////////
    }

    private void ShowInteractionUI(Transform interactedObject)
    {
        if (interactedObject.CompareTag("Machine1"))
        {
            Debug.Log("UI 1 displayed");
            SM1interactionUI.SetActive(true);
            SM2interactionUI.SetActive(false);
        }

        else if (interactedObject.CompareTag("Machine2"))
        {
            Debug.Log("UI 2 displayed");
            SM1interactionUI.SetActive(false);
            SM2interactionUI.SetActive(true);
        }
        ////////////////////
    }

    private void HideInteractionUI()
    {
        SM1interactionUI.SetActive(false); // Hide the interaction UI
        SM2interactionUI.SetActive(false);
        isInteractionEnabled = false;  // Interaction is disabled
    }
//
    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (currentTarget != null)
        {
            Debug.Log("Interact action triggered.");
            if (currentTarget.CompareTag("Machine1"))
            {
                ActivateMachine1();
            }
            if (currentTarget.CompareTag("Machine2")) 
            {
                ActivateMachine2();
            }
        }//
    }
    //
    private void ActivateMachine1()
    {
        Debug.Log("Scene#1,Machine1 loaded");
        sceneManager.LoadScene2();

    }
    private void ActivateMachine2() 
    {
        Debug.Log("Scene#2,Machine2 loaded");
        sceneManager.LoadScene3();

    }//
}
