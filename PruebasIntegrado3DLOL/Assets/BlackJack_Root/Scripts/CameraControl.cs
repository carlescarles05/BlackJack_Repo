using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;  // El jugador
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;

    void Update()
    {
        if (player != null)
        {
            // Aqu� va el c�digo normal para mover la c�mara
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            player.Rotate(Vector3.up * mouseX);
        }
    }

    // M�todo para fijar la c�mara en un �ngulo espec�fico (usado cuando el jugador se siente)
    public void SetCameraAngle(Vector3 angle)
    {
        transform.rotation = Quaternion.Euler(angle);
    }

    // M�todo para restaurar el control normal de la c�mara
    public void ResetCameraAngle()
    {
        
    }
}
