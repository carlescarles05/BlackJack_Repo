using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;  // El jugador
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;



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
