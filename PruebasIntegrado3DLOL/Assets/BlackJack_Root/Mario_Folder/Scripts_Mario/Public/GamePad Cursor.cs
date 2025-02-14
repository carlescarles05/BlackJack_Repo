using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem.LowLevel;
using System.Collections;
using System.Runtime.InteropServices;

public class GamepadCursor : MonoBehaviour
{
   
    //better with API
    public float mouseSpeed = 500f; // Adjust mouse sensitivity
    private Vector2 mouseDelta;
    private GuessCardInputActions inputActions;
    [DllImport("user32.dll")]
    private static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);

    private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
    private const uint MOUSEEVENTF_LEFTUP = 0x04;

    void Awake()
    {
        inputActions = new GuessCardInputActions();
        inputActions.GuessTheCardGame.PointerMovement.performed += ctx => mouseDelta = ctx.ReadValue<Vector2>();
        inputActions.GuessTheCardGame.PointerMovement.canceled += ctx => mouseDelta = Vector2.zero;
        inputActions.GuessTheCardGame.PointerClick.performed += ctx => Click();
    }

    void OnEnable() => inputActions.GuessTheCardGame.Enable();
    void OnDisable() => inputActions.GuessTheCardGame.Disable();

    void Update()
    {
        if (mouseDelta != Vector2.zero)
        {
            // Move the system mouse
            Vector3 currentMousePosition = Mouse.current.position.ReadValue();
            Vector3 newMousePosition = currentMousePosition + (Vector3)(mouseDelta * mouseSpeed * Time.deltaTime);

            // Clamp within screen bounds
            newMousePosition.x = Mathf.Clamp(newMousePosition.x, 0, Screen.width);
            newMousePosition.y = Mathf.Clamp(newMousePosition.y, 0, Screen.height);

            // Set the mouse position
            Mouse.current.WarpCursorPosition(newMousePosition);
        }
    }
    void Click()
    {
        // Simulate a real Windows mouse click
        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
    }
        IEnumerator ReleaseMouseClick()
    {
        yield return new WaitForSeconds(0.05f); // Small delay before releasing the click

        if (Mouse.current != null)
        {
            InputSystem.QueueStateEvent(Mouse.current, new MouseState { buttons = 0 });
            InputSystem.Update();
        }
    }



}

