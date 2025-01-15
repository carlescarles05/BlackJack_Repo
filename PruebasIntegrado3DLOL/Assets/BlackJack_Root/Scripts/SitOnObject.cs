using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class SitOnObject : MonoBehaviour
{
    public Transform seatPoint;
    public Canvas canvasObject;        // Canvas del jugador
    public Canvas enemyCanvasObject;   // Canvas del enemigo
    public KeyCode interactKey = KeyCode.E;
    public float transitionSpeed = 2f;

    public List<GameObject> PlayerCards = new List<GameObject>(); // Lista para las cartas del jugador
    private bool isSitting = false;
    private bool isTransitioning = false;
    private Transform playerTransform;
    private PlayerMovement playerMovement;
    private CharacterController characterController;

    public Canvas timerCanvas; // Canvas del temporizador
    public Cronometro cronometro; // Referencia al script del temporizador
    public Canvas timerCanvasEnemy; //Temporizador enemigo
    public Cronometro cronometroEnemigo; //Cronometro enemigo

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        playerMovement = playerTransform.GetComponent<PlayerMovement>();
        characterController = playerTransform.GetComponent<CharacterController>();

        if (canvasObject != null)
        {
            canvasObject.gameObject.SetActive(false);
        }

        if (enemyCanvasObject != null)
        {
            enemyCanvasObject.gameObject.SetActive(false);  // Desactivar canvas enemigo inicialmente
        }
    }


    void Update()
    {
        if (!isSitting && !isTransitioning)
        {
            Collider[] nearbyObjects = Physics.OverlapSphere(playerTransform.position, 2f);
            foreach (Collider obj in nearbyObjects)
            {
                if (obj != null && obj.CompareTag("Chair"))
                {
                    if (Input.GetKeyDown(interactKey))
                    {
                        StartCoroutine(SitDownSmooth(obj.transform));
                        break;
                    }
                }
            }
        }
    }

    IEnumerator SitDownSmooth(Transform chair)
    {
        isTransitioning = true;

        // Desactivar movimiento y rotación del jugador
        if (playerMovement != null) playerMovement.enabled = false;
        if (characterController != null) characterController.enabled = false;

        // Desactivar el Rigidbody para evitar movimiento por física
        Rigidbody playerRigidbody = playerTransform.GetComponent<Rigidbody>();
        if (playerRigidbody != null)
        {
            playerRigidbody.isKinematic = true; // Desactivar la física
            playerRigidbody.detectCollisions = false; // Desactivar las colisiones
        }

        // Fijar la rotación del jugador para que no gire durante el movimiento
        playerTransform.rotation = Quaternion.Euler(0f, playerTransform.rotation.eulerAngles.y, 0f); // Mantener solo la rotación en Y

        // Mover al jugador suavemente hacia la silla
        float elapsedTime = 0f;
        Vector3 startPosition = playerTransform.position;

        // Movimiento suave hacia la silla sin salto
        while (elapsedTime < 1f)
        {
            playerTransform.position = Vector3.Lerp(startPosition, seatPoint.position, elapsedTime);
            elapsedTime += Time.deltaTime * transitionSpeed;
            yield return null;
        }

        // Asegurarse de que el jugador está exactamente en la silla al final
        playerTransform.position = seatPoint.position;

        isSitting = true;
        isTransitioning = false;

        // Activar Canvas del jugador
        if (canvasObject != null)
        {
            canvasObject.gameObject.SetActive(true);
        }

        // Activar cartas y Canvas del enemigo después de un retraso
        yield return new WaitForSeconds(2f);  // Esperar 2 segundos antes de mostrar las cartas del enemigo


        // Activar Canvas del enemigo
        if (enemyCanvasObject != null)
        {
            enemyCanvasObject.gameObject.SetActive(true);
        }

        if (timerCanvas != null)
        {
            timerCanvas.gameObject.SetActive(true); // Activar el canvas del temporizador

            if (cronometro != null)
            {
                cronometro.gameObject.SetActive(true); // Activar el GameObject del cronómetro
                cronometro.StartCountdown(() =>
                {
                    timerCanvas.gameObject.SetActive(false); // Ocultar el canvas al terminar la cuenta regresiva
                });
            }
        }

        if (timerCanvasEnemy != null)
        {
            timerCanvasEnemy.gameObject.SetActive(true); // Activar el canvas del segundo temporizador

            if (cronometroEnemigo != null)
            {
                cronometroEnemigo.gameObject.SetActive(true); // Activar el segundo cronómetro
                cronometroEnemigo.StartCountdown(() =>
                {
                    timerCanvasEnemy.gameObject.SetActive(false); // Ocultar el canvas del segundo temporizador al terminar
                });
            }
        }
    }
}
