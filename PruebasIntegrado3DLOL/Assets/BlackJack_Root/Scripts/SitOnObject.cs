using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SitOnObject : MonoBehaviour
{

    /*public Transform seatPoint;         // El punto donde el jugador debe ir
    public Canvas canvasObject;         // Referencia al Canvas
    public GameObject objectToShow;     // Objeto que aparece cuando el jugador se sienta
    public ParticleSystem summonEffect; // Sistema de partículas para el humo
    public KeyCode interactKey = KeyCode.E; // Tecla para interactuar con la silla
    public float transitionSpeed = 2f;  // Velocidad de la transición al sentarse

    private bool isSitting = false;     // ¿Está el jugador sentado?
    private bool isTransitioning = false; // ¿Está en proceso de transición?
    private Transform playerTransform;
    private PlayerMovement playerMovement;
    private CharacterController characterController;

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        playerMovement = playerTransform.GetComponent<PlayerMovement>();
        characterController = playerTransform.GetComponent<CharacterController>();

        // Asegúrate de que el Canvas y el objeto estén desactivados al inicio
        if (canvasObject != null)
        {
            canvasObject.gameObject.SetActive(false);
        }

        if (objectToShow != null)
        {
            objectToShow.SetActive(false);
        }

        if (summonEffect != null)
        {
            summonEffect.Stop(); // Asegúrate de que el sistema de partículas no esté activo al inicio
        }
    }

    void Update()
    {
        if (!isSitting && !isTransitioning)
        {
            // Detectar la cercanía del jugador con la silla
            Collider[] nearbyObjects = Physics.OverlapSphere(playerTransform.position, 2f);
            foreach (Collider obj in nearbyObjects)
            {
                if (obj.CompareTag("Chair"))
                {
                    Debug.Log("Silla detectada. Presiona 'E' para sentarte.");
                    if (Input.GetKeyDown(interactKey))
                    {
                        StartCoroutine(SitDownSmooth(obj.transform));
                        break;
                    }
                }
            }
        }
    }


    System.Collections.IEnumerator SitDownSmooth(Transform chair)
    {
        if (seatPoint == null)
        {
            Debug.LogError("No se ha asignado un SeatPoint en el Inspector.");
            yield break;
        }

        isTransitioning = true;

        // Desactivar el movimiento del jugador
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
        if (characterController != null)
        {
            characterController.enabled = false;
        }

        // Mover al jugador gradualmente al SeatPoint
        float elapsedTime = 0f;
        Vector3 startPosition = playerTransform.position;

        while (elapsedTime < 1f)
        {
            playerTransform.position = Vector3.Lerp(startPosition, seatPoint.position, elapsedTime);
            elapsedTime += Time.deltaTime * transitionSpeed;

            yield return null; // Esperar al siguiente frame
        }

        playerTransform.position = seatPoint.position; // Asegurarse de que esté exactamente en el punto final
        isSitting = true;
        isTransitioning = false;

        // Activar el Canvas
        if (canvasObject != null)
        {
            canvasObject.gameObject.SetActive(true);
        }

        // Activar el efecto de partículas
        if (summonEffect != null)
        {
            summonEffect.Play(); // Iniciar el efecto de partículas
            Debug.Log("Efecto de partículas iniciado.");
            yield return new WaitForSeconds(summonEffect.main.duration); // Esperar a que termine
        }

        // Mostrar el objeto después del efecto
        if (objectToShow != null)
        {
            Debug.Log("Efecto de partículas terminado. Activando objeto.");
            objectToShow.SetActive(true);
        }



        Debug.Log("El jugador ahora está sentado, y el objeto ha sido invocado.");

    }*/

    public Transform seatPoint;        // El punto donde el jugador debe ir
    public Canvas canvasObject;        // Referencia al Canvas
    public KeyCode interactKey = KeyCode.E;  // Tecla para interactuar con la silla
    public float transitionSpeed = 2f; // Velocidad de la transición al sentarse
    public GameObject smokeObject;    // El objeto que contiene el ParticleSystem (humo)
    public GameObject smokeObject2;    // El objeto que contiene el ParticleSystem (humo)
    public GameObject cardObject;    // El objeto de la carta que aparecerá
    public GameObject cardObject2;    // El objeto de la carta que aparecerá

    private bool isSitting = false;    // ¿Está el jugador sentado?
    private bool isTransitioning = false; // ¿Está en proceso de transición?
    private Transform playerTransform;
    private PlayerMovement playerMovement;
    private CharacterController characterController;

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        playerMovement = playerTransform.GetComponent<PlayerMovement>();
        characterController = playerTransform.GetComponent<CharacterController>();

        // Asegúrate de que el Canvas y la carta estén desactivados al inicio
        if (canvasObject != null)
        {
            canvasObject.gameObject.SetActive(false);
        }

        if (cardObject != null)
        {
            cardObject.SetActive(false); // Desactivar la carta al principio
        }

        if (smokeObject != null)
        {
            smokeObject.SetActive(false); // Desactivar el humo al principio
        }
    }

    void Update()
    {
        if (!isSitting && !isTransitioning)
        {
            // Detectar la cercanía del jugador con la silla
            Collider[] nearbyObjects = Physics.OverlapSphere(playerTransform.position, 2f);
            foreach (Collider obj in nearbyObjects)
            {
                if (obj.CompareTag("Chair"))
                {
                    Debug.Log("Silla detectada. Presiona 'E' para sentarte.");
                    if (Input.GetKeyDown(interactKey))
                    {
                        StartCoroutine(SitDownSmooth(obj.transform));
                        break;
                    }
                }
            }
        }
    }

    System.Collections.IEnumerator SitDownSmooth(Transform chair)
    {
        if (seatPoint == null)
        {
            Debug.LogError("No se ha asignado un SeatPoint en el Inspector.");
            yield break;
        }

        isTransitioning = true;

        // Desactivar el movimiento del jugador
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
        if (characterController != null)
        {
            characterController.enabled = false;
        }

        // Mover al jugador gradualmente al SeatPoint
        float elapsedTime = 0f;
        Vector3 startPosition = playerTransform.position;

        while (elapsedTime < 1f)
        {
            playerTransform.position = Vector3.Lerp(startPosition, seatPoint.position, elapsedTime);
            elapsedTime += Time.deltaTime * transitionSpeed;

            yield return null; // Esperar al siguiente frame
        }

        playerTransform.position = seatPoint.position; // Asegurarse de que esté exactamente en el punto final
        isSitting = true;
        isTransitioning = false;

        // Activar el Particle System de humo
        if (smokeObject != null)
        {
            smokeObject.SetActive(true);  // Activar el objeto de humo
            ParticleOnce smokeParticleScript = smokeObject.GetComponent<ParticleOnce>();
            if (smokeParticleScript != null)
            {
                smokeParticleScript.PlayOnce();  // Reproduce el Particle System una sola vez
            }
        }

        // Esperar un poco antes de activar la carta (para que el humo termine de reproducirse)
        yield return new WaitForSeconds(1f); // Ajusta el tiempo si es necesario para que el humo se vea

        // Activar la carta después del humo
        if (cardObject != null)
        {
            cardObject.SetActive(true);  // Activar la carta después de un retraso
        }

        // Desactivar el Particle System de humo después de que la carta aparezca
        if (smokeObject != null)
        {
            DeactivateParticleSystem();
        }

        // Activar el Canvas
        if (canvasObject != null)
        {
            canvasObject.gameObject.SetActive(true);
        }

        Debug.Log("El jugador ahora está sentado.");
    }

    // Método que desactiva el Particle System después de que la carta aparezca
    void DeactivateParticleSystem()
    {
        if (smokeObject != null)
        {
            ParticleSystem ps = smokeObject.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Stop();  // Detener el Particle System
                ps.Clear(); // Limpiar las partículas
                smokeObject.SetActive(false); // Desactivar el objeto con el Particle System
            }
        }
    }
}
