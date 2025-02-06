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
    private Transform playerTransform;
    private PlayerMovement playerMovement;
    private CharacterController characterController;
    public AdivinaLaCarta adivinaLaCarta;

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
                if (obj != null && obj.CompareTag("Chair"))
                {
                    if (Input.GetKeyDown(interactKey))
                    {
                        adivinaLaCarta.canPlay = true;
                        StartCoroutine(SitDownSmooth());
                        break;
                    }
                }
            }

            
        }
        HandleCursorState();
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
    /*public Transform seatPoint;
    public Canvas canvasObject;        // Canvas del jugador
    public Canvas enemyCanvasObject;   // Canvas del enemigo
    public float transitionSpeed = 2f;

    public List<GameObject> PlayerCards = new List<GameObject>(); // Lista para las cartas del jugador
    private bool isSitting = false;
    private bool isTransitioning = false;
    private Transform playerTransform;
    private PlayerMovement playerMovement;
    private CharacterController characterController;

    public Canvas timerCanvas; // Canvas del temporizador
    public Cronometro cronometro; // Referencia al script del temporizador
    public Canvas timerCanvasEnemy; // Temporizador enemigo
    public Cronometro cronometroEnemigo; // Cronómetro enemigo

    private GameInputActions playerInput; // Input Actions instance
    private InputAction interactAction;    // Reference to the "Interact" action

    void Awake()
    {
        playerInput = new GameInputActions(); // Initialize the Input Actions
    }

    void OnEnable()
    {
        // Enable the Input Actions
        playerInput.XboxControl.Enable();
        interactAction = playerInput.XboxControl.SitOnObject; // Assign the "SitOnObject" action
        interactAction.performed += OnInteract;              // Subscribe to the performed event
    }

    void OnDisable()
    {
        // Disable the Input Actions
        playerInput.XboxControl.Disable();
        interactAction.performed -= OnInteract; // Unsubscribe from the performed event
    }

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
        HandleCursorState();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (!isSitting && !isTransitioning)
        {
            Collider[] nearbyObjects = Physics.OverlapSphere(playerTransform.position, 2f);
            foreach (Collider obj in nearbyObjects)
            {
                if (obj != null && obj.CompareTag("Chair"))
                {
                    StartCoroutine(SitDownSmooth());
                    break;
                }
            }
        }
    }

    public void StandUp()
    {
        StartCoroutine(SitDownSmooth());
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
            // Standing up logic
            if (playerMovement != null) playerMovement.canMove = true;
            if (characterController != null) characterController.enabled = true;
            if (playerMovement != null) playerMovement.isSitting = false;

            // Disable player physics
            Rigidbody playerRigidbody = playerTransform.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                playerRigidbody.isKinematic = false; // Enable physics
                playerRigidbody.detectCollisions = true; // Enable collisions
            }

            // Move the player back to the standing position
            float elapsedTime = 0f;
            Vector3 startPosition = playerTransform.position;

            while (elapsedTime < 1f)
            {
                playerTransform.position = Vector3.Lerp(startPosition, seatPoint.position, elapsedTime);
                elapsedTime += Time.deltaTime * transitionSpeed;
                yield return null;
            }

            playerTransform.position = seatPoint.position;

            // Disable the canvases when standing up
            if (canvasObject != null) canvasObject.gameObject.SetActive(false);
            if (timerCanvas != null) timerCanvas.gameObject.SetActive(false);
            if (enemyCanvasObject != null) enemyCanvasObject.gameObject.SetActive(false);
            if (timerCanvasEnemy != null) timerCanvasEnemy.gameObject.SetActive(false);

            isSitting = false;
        }
        else
        {
            if (playerMovement != null) playerMovement.canMove = false;
            if (characterController != null) characterController.enabled = false;
            if (playerMovement != null) playerMovement.isSitting = true;

            // Disable player physics
            Rigidbody playerRigidbody = playerTransform.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                playerRigidbody.isKinematic = true; // Disable physics
                playerRigidbody.detectCollisions = false; // Disable collisions
            }

            // Move the player smoothly into the seat
            float elapsedTime = 0f;
            Vector3 startPosition = playerTransform.position;

            while (elapsedTime < 1f)
            {
                playerTransform.position = Vector3.Lerp(startPosition, seatPoint.position, elapsedTime);
                elapsedTime += Time.deltaTime * transitionSpeed;
                yield return null;
            }

            playerTransform.position = seatPoint.position;

            if (canvasObject != null) canvasObject.gameObject.SetActive(true);
            if (timerCanvas != null) timerCanvas.gameObject.SetActive(true);
            if (enemyCanvasObject != null) enemyCanvasObject.gameObject.SetActive(true);
            if (timerCanvasEnemy != null) timerCanvasEnemy.gameObject.SetActive(true);

            // Start the countdown timers
            if (cronometro != null)
            {
                cronometro.gameObject.SetActive(true);
                cronometro.InteractiveCountdown(() =>
                {
                    StartCoroutine(FadeCanvas(timerCanvas.gameObject, false, 1f)); // Fade out when finished
                });
            }

            if (cronometroEnemigo != null)
            {
                cronometroEnemigo.gameObject.SetActive(true);
                cronometroEnemigo.InteractiveCountdown(() =>
                {
                    StartCoroutine(FadeCanvas(timerCanvasEnemy.gameObject, false, 1f)); // Fade out when finished
                }, 8000);
            }

            isSitting = true;
        }

        isTransitioning = false;
    }

    /* IEnumerator SitDownSmooth()
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
     }*/
}

