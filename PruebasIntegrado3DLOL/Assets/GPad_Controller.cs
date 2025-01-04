using UnityEngine;

public class GPad_Controller : MonoBehaviour
{
    public Camera playerCamera;
    public float moveSpeed = 5f;
    public float rotationSpeed = 3f;

    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController is missing!");
        }
    }

    private void Update()
    {
        Vector2 moveInput = Input_Manager.Instance?.GetMoveInput() ?? Vector2.zero;
        Vector2 lookInput = Input_Manager.Instance?.GetLookInput() ?? Vector2.zero;

        // Move the player
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        characterController.Move(move * moveSpeed * Time.deltaTime);

        // Rotate the player and camera
        transform.Rotate(Vector3.up, lookInput.x * rotationSpeed);
        playerCamera.transform.Rotate(Vector3.left, lookInput.y * rotationSpeed);
    }
}
