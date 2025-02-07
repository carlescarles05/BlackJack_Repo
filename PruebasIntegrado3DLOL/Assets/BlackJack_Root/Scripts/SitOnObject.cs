using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

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
    private bool triggerDesactivado = false;
    private Transform playerTransform;
    private PlayerMovement playerMovement;
    private CharacterController characterController;
    public AdivinaLaCarta adivinaLaCarta;
    public GameObject LuciusEnemy;
    public Transform puntoA;
    public Transform puntoB;

    public Canvas timerCanvas; // Canvas del temporizador
    public Cronometro cronometro; // Referencia al script del temporizador
    public Canvas timerCanvasEnemy; // Temporizador enemigo
    public Cronometro cronometroEnemigo; // Cronómetro enemigo

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        playerMovement = playerTransform.GetComponent<PlayerMovement>();
        characterController = playerTransform.GetComponent<CharacterController>();

        if (canvasObject != null) canvasObject.gameObject.SetActive(false);
        if (timerCanvas != null) timerCanvas.gameObject.SetActive(false);
        if (enemyCanvasObject != null) enemyCanvasObject.gameObject.SetActive(false);
        if (timerCanvasEnemy != null) timerCanvasEnemy.gameObject.SetActive(false);

        if (cronometro != null) cronometro.gameObject.SetActive(false);
        if (cronometroEnemigo != null) cronometroEnemigo.gameObject.SetActive(false);

    }

    void Update()
    {
        if (!isSitting && !isTransitioning)
        {
            Collider[] nearbyObjects = Physics.OverlapSphere(playerTransform.position, 2f);
            foreach (Collider obj in nearbyObjects)
            {
                if (obj != null && obj.CompareTag("Chair") && adivinaLaCarta.jugadorCerca)
                {
                    adivinaLaCarta.text.gameObject.SetActive(true);

                    if (Input.GetKeyDown(interactKey))
                    {
                        adivinaLaCarta.canPlay = true;
                        adivinaLaCarta.text.gameObject.SetActive(false);
                        StartCoroutine(SitDownSmooth());
                        //Animacion del centro lucius para tepearse
                        StartCoroutine(TeletransporteLucius());
                        break;
                    }
                    
                }

            }

            
        }
        HandleCursorState();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggerDesactivado)
        {
            adivinaLaCarta.jugadorCerca = true; // Detecta que el jugador está cerca
            //interaction.SetActive(true);
            adivinaLaCarta.text.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            adivinaLaCarta.jugadorCerca = false; // El jugador sale del área
            //interaction.SetActive(false);
            adivinaLaCarta.text.gameObject.SetActive(false);
        }
    }

    private IEnumerator TeletransporteLucius()
    {
        yield return new WaitForSeconds(3f);
        LuciusEnemy.transform.position = puntoB.position;
    }

    public void StandUp()
    {
        StartCoroutine(SitDownSmooth());
    }

    private void HandleCursorState()
    {
        if (isSitting)
        {
            // Mostrar el cursor y desbloquearlo
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }
        else
        {
            // Ocultar el cursor y bloquearlo
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }
    }

    IEnumerator SitDownSmooth()
    {
        isTransitioning = true;
        if (isSitting)
        {
            // Desactivar movimiento y rotación del jugador
            if (playerMovement != null) playerMovement.canMove = true;
            if (characterController != null) characterController.enabled = true;
            if (playerMovement != null) playerMovement.isSitting = false;
            

            // Desactivar el Rigidbody para evitar movimiento por física
            Rigidbody playerRigidbody = playerTransform.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                playerRigidbody.isKinematic = false; // Desactivar la física
                playerRigidbody.detectCollisions = true; // Desactivar las colisiones
            }

            // Fijar la rotación del jugador para que no gire durante el movimiento
            playerTransform.rotation = Quaternion.Euler(0f, playerTransform.rotation.eulerAngles.y, 0f); // Mantener solo la rotación en Y

            // Mover al jugador suavemente hacia la silla
            float elapsedTime = 0f;
            Vector3 startPosition = playerTransform.position;

            while (elapsedTime < 1f)
            {
                playerTransform.position = Vector3.Lerp(startPosition, seatPoint.position, elapsedTime);
                elapsedTime += Time.deltaTime * transitionSpeed;
                yield return null;
            }

            playerTransform.position = seatPoint.position;

            isSitting = false;
        }
        else
        {
            // Desactivar movimiento y rotación del jugador
            if (playerMovement != null) playerMovement.canMove = false;
            if (characterController != null) characterController.enabled = false;
            if (playerMovement != null) playerMovement.isSitting = true;

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

            while (elapsedTime < 1f)
            {
                playerTransform.position = Vector3.Lerp(startPosition, seatPoint.position, elapsedTime);
                elapsedTime += Time.deltaTime * transitionSpeed;
                yield return null;
            }

            playerTransform.position = seatPoint.position;

            isSitting = true;
        }



        isTransitioning = false;
        Debug.Log(cronometro.isCorutineActive);
        if (cronometro.isCorutineActive == false)
        {
         if (canvasObject != null)
         {
             yield return StartCoroutine(FadeCanvas(canvasObject.gameObject, true, 1f)); // Aparece el Canvas del jugador
         }

         if (timerCanvas != null)
         {
             yield return StartCoroutine(FadeCanvas(timerCanvas.gameObject, true, 1f)); // Aparece el Canvas del cronómetro
             if (cronometro != null)
             {
                 cronometro.gameObject.SetActive(true); // Activar el cronómetro del jugador
                 cronometro.InteractiveCountdown(() =>
                 {
                     StartCoroutine(FadeCanvas(timerCanvas.gameObject, false, 1f)); // Ocultar el Canvas cuando termine el cronómetro
                 });
             }
         }

         if (enemyCanvasObject != null)
         {
             yield return StartCoroutine(FadeCanvas(enemyCanvasObject.gameObject, true, 1f)); // Aparece el Canvas del enemigo
         }

         if (timerCanvasEnemy != null)
         {
             yield return StartCoroutine(FadeCanvas(timerCanvasEnemy.gameObject, true, 1f)); // Aparece el Canvas del cronómetro enemigo
             if (cronometroEnemigo != null)
             {
                 cronometroEnemigo.gameObject.SetActive(true); // Activar el cronómetro del enemigo
                 cronometroEnemigo.InteractiveCountdown(() =>
                 {
                     StartCoroutine(FadeCanvas(timerCanvasEnemy.gameObject, false, 1f)); // Ocultar el Canvas enemigo cuando termine el cronómetro
                 }, 2000);
             }
         }
     }
     else
     {
         cronometroEnemigo.toggleCountdown();
         BJManager.Instance.ToggleButtons(isSitting);
     }

 }

 IEnumerator FadeCanvas(GameObject canvasObject, bool fadeIn, float duration)
 {
     // Asegurarse de que el Canvas tenga un CanvasGroup
     CanvasGroup canvasGroup = canvasObject.GetComponent<CanvasGroup>();
     if (canvasGroup == null)
     {
         canvasGroup = canvasObject.AddComponent<CanvasGroup>();
     }

     float startAlpha = fadeIn ? 0f : 1f;
     float endAlpha = fadeIn ? 1f : 0f;
     float elapsedTime = 0f;

     // Activar el Canvas si se está haciendo fadeIn
     if (fadeIn) canvasObject.SetActive(true);

     while (elapsedTime < duration)
     {
         elapsedTime += Time.deltaTime;
         float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
         canvasGroup.alpha = alpha;
         yield return null;
     }

     canvasGroup.alpha = endAlpha;

     // Desactivar el Canvas si se está haciendo fadeOut
     if (!fadeIn) canvasObject.SetActive(false);
 }
}

