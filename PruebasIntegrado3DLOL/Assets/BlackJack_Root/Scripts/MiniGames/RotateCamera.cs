using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{

    [SerializeField] private float rotationDuration = 7f;
    [SerializeField] private GameObject cam;
    private Quaternion originalRotation;
    private bool isRotating = false;

    private void Start()
    {
        cam = GameObject.Find("Main Camera");
        originalRotation = cam.transform.rotation;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isRotating)
        {
            StartCoroutine(RotateCameraInstantly());
        }
    }

    IEnumerator RotateCameraInstantly()
    {
        isRotating = true;
        Quaternion endRotation = Quaternion.Euler(0, 0, 180) * originalRotation;
        cam.transform.rotation = endRotation;

        // Wait for a certain duration before returning to original rotation
        yield return new WaitForSeconds(rotationDuration);

        // Rotate back to original rotation
        cam.transform.rotation = originalRotation;
        isRotating = false;
    }

}
