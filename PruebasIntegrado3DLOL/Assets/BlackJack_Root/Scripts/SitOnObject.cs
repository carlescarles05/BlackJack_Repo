using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class SitOnObject : MonoBehaviour
{

    public Transform seatPoint;        // El punto donde el jugador debe ir
    public Canvas canvasObject;        // Referencia al Canvas
    public KeyCode interactKey = KeyCode.E;  // Tecla para interactuar con la silla
    public float transitionSpeed = 2f; // Velocidad de la transición al sentarse

    public GameObject smokeObject;    // Humo 1
    public GameObject smokeObject2;   // Humo 2
    public GameObject cardObject;     // Carta 1
    public GameObject cardObject2;    // Carta 2

    private bool isSitting = false;    // ¿Está el jugador sentado?
    private bool isTransitioning = false; // ¿Está en proceso de transición?
    private Transform playerTransform;
    private PlayerMovement playerMovement;
    private CharacterController characterController;

    // Nueva parte para las cartas
    private int playerTotal = 0;
    private int enemyTotal = 0;
    public TextMeshProUGUI playerTotalText;  // Canvas Text para el jugador
    public TextMeshProUGUI enemyTotalText;   // Canvas Text para el enemigo

    // Valores posibles para las cartas
    int[] cardValues = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10 };

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        playerMovement = playerTransform.GetComponent<PlayerMovement>();
        characterController = playerTransform.GetComponent<CharacterController>();

        // Desactivar elementos al inicio
        if (canvasObject != null)
        {
            canvasObject.gameObject.SetActive(false);
        }

        if (cardObject != null)
        {
            cardObject.SetActive(false);
        }

        if (smokeObject != null)
        {
            smokeObject.SetActive(false);
        }

        if (cardObject2 != null)
        {
            cardObject2.SetActive(false);
        }

        if (smokeObject2 != null)
        {
            smokeObject2.SetActive(false);
        }
    }

    void Update()
    {
        if (!isSitting && !isTransitioning)
        {
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

    IEnumerator SitDownSmooth(Transform chair)
    {
        if (seatPoint == null)
        {
            Debug.LogError("No se ha asignado un SeatPoint en el Inspector.");
            yield break;
        }

        isTransitioning = true;

        // Desactivar el movimiento del jugador
        if (playerMovement != null) playerMovement.enabled = false;
        if (characterController != null) characterController.enabled = false;

        // Mover al jugador gradualmente al SeatPoint
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
        isTransitioning = false;

        // Activar el primer conjunto (humo 1 y carta 1)
        yield return ActivateEffect(smokeObject, cardObject);

        // Activar el segundo conjunto (humo 2 y carta 2)
        yield return ActivateEffect(smokeObject2, cardObject2);

        // Activar el Canvas
        if (canvasObject != null)
        {
            canvasObject.gameObject.SetActive(true);
        }

        // Generar las cartas aleatorias para el jugador y enemigo
        GenerateCards();

        Debug.Log("El jugador ahora está sentado.");
    }

    // Método genérico para manejar humo y carta
    IEnumerator ActivateEffect(GameObject smoke, GameObject card)
    {
        if (smoke != null)
        {
            smoke.SetActive(true);  // Activar humo
            ParticleOnce smokeParticleScript = smoke.GetComponent<ParticleOnce>();
            if (smokeParticleScript != null)
            {
                smokeParticleScript.PlayOnce(); // Reproducir el sistema de partículas
            }
        }

        // Esperar un segundo antes de mostrar la carta
        yield return new WaitForSeconds(1f);

        if (card != null)
        {
            card.SetActive(true);  // Activar carta
        }

        // Desactivar el humo después de que aparezca la carta
        if (smoke != null)
        {
            DeactivateParticleSystem(smoke);
        }
    }

    // Método que desactiva el Particle System
    void DeactivateParticleSystem(GameObject smoke)
    {
        ParticleSystem ps = smoke.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Stop();
            ps.Clear();
            smoke.SetActive(false);
        }
    }

    // Método para generar cartas aleatorias
    void GenerateCards()
    {
        // Generar carta para el jugador
        int playerCard1 = cardValues[Random.Range(0, cardValues.Length)];
        int playerCard2 = cardValues[Random.Range(0, cardValues.Length)];
        playerTotal = playerCard1 + playerCard2;
        playerTotalText.text = playerTotal + "/21";

        // Generar carta para el enemigo
        int enemyCard1 = cardValues[Random.Range(0, cardValues.Length)];
        int enemyCard2 = cardValues[Random.Range(0, cardValues.Length)];
        enemyTotal = enemyCard1 + enemyCard2;
        enemyTotalText.text = enemyTotal + "/21";
    }
}
