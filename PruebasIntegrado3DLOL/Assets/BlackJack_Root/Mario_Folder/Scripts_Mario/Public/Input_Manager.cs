using UnityEngine;
using UnityEngine.InputSystem;

public class Input_Manager : MonoBehaviour
{
    public static Input_Manager instance;
  //  public GameInputActions InputActions { get; private set; }
    public float speed = 5f;
    public float lookSensitivity = 3f;

    private Vector2 moveInput;
    private Vector2 lookInput;

    public static Input_Manager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("Input_Manager is NULL! Ensure it is in the scene.");
            }
            return instance;
        }
    }

  /*  private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        InputActions = new GameInputActions();
        InputActions.Enable();

        InputActions.XboxControl.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        InputActions.XboxControl.Move.canceled += ctx => moveInput = Vector2.zero;

        InputActions.XboxControl.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        InputActions.XboxControl.Look.canceled += ctx => lookInput = Vector2.zero;

        Debug.Log("Input_Manager successfully initialized.");
    }*/

    public Vector2 GetMoveInput()
    {
        return moveInput;
    }

    public Vector2 GetLookInput()
    {
        return lookInput;
    }
}
