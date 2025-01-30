using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;  // El jugador
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;



    // Método para fijar la cámara en un ángulo específico (usado cuando el jugador se siente)
    public void SetCameraAngle(Vector3 angle)
    {
        transform.rotation = Quaternion.Euler(angle);
    }

    // Método para restaurar el control normal de la cámara
    public void ResetCameraAngle()
    {
        
    }
}
