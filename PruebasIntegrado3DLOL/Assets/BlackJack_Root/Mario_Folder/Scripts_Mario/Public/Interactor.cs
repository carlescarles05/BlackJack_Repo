using UnityEngine;

public class Interactor : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject MainMenuPanel;
    public GameObject interactionUI;
    public float raycastDistance = 5f;
    public LayerMask interactableLayer;

    private Transform currentTarget;
    private bool isInteractionEnabled = false;

    private void Update()
    {
        if (Input_Manager.Instance == null)
        {
            Debug.LogError("Input_Manager is NULL. Interaction functionality is disabled.");
            return;
        }

        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, interactableLayer))
        {
            if (currentTarget == null || hit.transform != currentTarget)
            {
                currentTarget = hit.transform;
                ShowInteractionUI();
            }
        }
        else
        {
            if (currentTarget != null)
            {
                HideInteractionUI();
                currentTarget = null;
            }
        }
    }

    private void ShowInteractionUI()
    {
        interactionUI.SetActive(true);
        if (!isInteractionEnabled)
        {
            EnableInteractAction();
        }
    }

    private void HideInteractionUI()
    {
        if (isInteractionEnabled)
        {
            DisableInteractAction();
        }
        interactionUI.SetActive(false);
    }

    private void EnableInteractAction()
    {
        if (Input_Manager.Instance != null)
        {
            Input_Manager.Instance.InputActions.XboxControl.Interact.performed += OnInteractPerformed;
            Input_Manager.Instance.InputActions.XboxControl.Interact.Enable();
            isInteractionEnabled = true;
            Debug.Log("Interact action enabled.");
        }
    }

    private void DisableInteractAction()
    {
        if (Input_Manager.Instance != null)
        {
            Input_Manager.Instance.InputActions.XboxControl.Interact.performed -= OnInteractPerformed;
            Input_Manager.Instance.InputActions.XboxControl.Interact.Disable();
            isInteractionEnabled = false;
            Debug.Log("Interact action disabled.");
        }
    }

    private void OnInteractPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Debug.Log("Interact action triggered.");
        PerformInteraction();
    }

    public void PerformInteraction()
    {
        Debug.Log("Interaction performed!");
        if (MainMenuPanel != null)
        {
            MainMenuPanel.SetActive(true);
        }
    }
}
