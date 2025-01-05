using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject MainMenuPanel;       // Panel for the menu to display
    public GameObject interactionUI;      // UI that shows interaction prompt
    public float raycastDistance = 5f;    // Distance to check for interactable objects
    public LayerMask interactableLayer;   // Layer for interactable objects

    private Transform currentTarget;      // Current target object in view
    private bool isInteractionEnabled = false;

    // Input System
    [SerializeField] private InputActionAsset gameInputActions;  // Reference to InputActionAsset
    private InputAction interactAction;

    private void Awake()
    {
        if (gameInputActions == null)
        {
            Debug.LogError("InputActionAsset is not assigned in the inspector.");
            return;
        }

        // Find the "XboxControl" action map and "Interact" action
        var actionMap = gameInputActions.FindActionMap("XboxControl");
        if (actionMap == null)
        {
            Debug.LogError("ActionMap 'XboxControl' not found in InputActionAsset.");
            return;
        }

        interactAction = actionMap.FindAction("Interact");
        if (interactAction == null)
        {
            Debug.LogError("Action 'Interact' not found in 'XboxControl' action map.");
            return;
        }

        interactAction.performed += OnInteractPerformed;  // Subscribe to interaction event
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
    {
        // Cast a ray from the center of the screen
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, interactableLayer))
        {
            if (currentTarget == null || hit.transform != currentTarget)
            {
                currentTarget = hit.transform;  // Update the current target
                ShowInteractionUI();           // Show the interaction UI
            }
        }
        else
        {
            if (currentTarget != null)
            {
                HideInteractionUI();           // Hide the interaction UI
                currentTarget = null;          // Clear the current target
            }
        }
    }

    private void ShowInteractionUI()
    {
        interactionUI.SetActive(true);  // Show the interaction UI
        isInteractionEnabled = true;   // Interaction is enabled
    }

    private void HideInteractionUI()
    {
        interactionUI.SetActive(false); // Hide the interaction UI
        isInteractionEnabled = false;  // Interaction is disabled
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (currentTarget != null)
        {
            Debug.Log("Interact action triggered.");
            PerformInteraction();
        }
    }

    public void PerformInteraction()
    {
        Debug.Log("Interaction performed!");
        if (MainMenuPanel != null)
        {
            MainMenuPanel.SetActive(true); // Show the menu panel
        }
    }
}
