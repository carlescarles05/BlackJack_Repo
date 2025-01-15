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
    public TextMeshProUGUI playerTotalText;  // Referencia al texto del jugador

    public List<GameObject> PlayerCards = new List<GameObject>(); // Lista para las cartas del jugador
    public bool IsPlayerTurn = true; // Controla si es el turno del jugador

    public GameObject smokeObject;
    public GameObject smokeObject2;
    public GameObject cardObject;      // Carta 1 del jugador
    public GameObject cardObject2;     // Carta 2 del jugador

    // Cartas y canvas del enemigo
    public GameObject enemyCardObject1;
    public GameObject enemyCardObject2;
    public TextMeshProUGUI enemyTotalText; // Total de cartas del enemigo
    public EnemyManager enemyManager; // Arrastra el GameObject con el EnemyManager en el Inspector

    private bool isSitting = false;
    private bool isTransitioning = false;
    private Transform playerTransform;
    private PlayerMovement playerMovement;
    private CharacterController characterController;

    public int playerTotal = 0;
    public int enemyTotal = 0;
    int[] cardValues = new int[] { 1, 2, 3, 4, 5 };

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

        if (cardObject != null) cardObject.SetActive(false);
        if (cardObject2 != null) cardObject2.SetActive(false);
        if (smokeObject != null) smokeObject.SetActive(false);
        if (smokeObject2 != null) smokeObject2.SetActive(false);
        if (enemyCardObject1 != null) enemyCardObject1.SetActive(false);
        if (enemyCardObject2 != null) enemyCardObject2.SetActive(false);
    }

    public int PlayerTotal
    {
        get { return playerTotal; }
        set { playerTotal = value; }
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

        // Activar humo y cartas del jugador
        yield return ActivateEffect(smokeObject, cardObject);
        yield return ActivateEffect(smokeObject2, cardObject2);

        // Activar Canvas del jugador
        if (canvasObject != null)
        {
            canvasObject.gameObject.SetActive(true);
        }

        // Generar cartas del jugador
        GenerateCards();

        // Activar cartas y Canvas del enemigo después de un retraso
        yield return new WaitForSeconds(2f);  // Esperar 2 segundos antes de mostrar las cartas del enemigo

        // Generar cartas del enemigo
        GenerateEnemyCards();

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

    IEnumerator ActivateEffect(GameObject smoke, GameObject card)
    {
        if (smoke != null)
        {
            smoke.SetActive(true);
            ParticleOnce smokeParticleScript = smoke.GetComponent<ParticleOnce>();
            if (smokeParticleScript != null)
            {
                smokeParticleScript.PlayOnce();
            }
        }

        yield return new WaitForSeconds(1f);

        if (card != null)
        {
            card.SetActive(true);
        }

        if (smoke != null)
        {
            DeactivateParticleSystem(smoke);
        }
    }

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

    void GenerateCards()
    {
        
        // Generar dos cartas al azar para el jugador
        int playerCard1 = cardValues[Random.Range(0, cardValues.Length)];
        int playerCard2 = cardValues[Random.Range(0, cardValues.Length)];

        // Reinicia el total del jugador antes de sumar las nuevas cartas
        playerTotal = 0;

        // Suma ambas cartas al total
        playerTotal += playerCard1 + playerCard2;

        // Actualiza el texto del total en el canvas
        /*if (playerTotalText != null)
        {
            playerTotalText.text = playerTotal + "/21";
        }*/
    }

    public void GenerateEnemyCards()
    {
        int enemyCard1 = cardValues[Random.Range(0, cardValues.Length)];
        int enemyCard2 = cardValues[Random.Range(0, cardValues.Length)];
        enemyTotal = enemyCard1 + enemyCard2;
        //enemyTotalText.text = enemyTotal + "/21";

        // Mostrar las cartas del enemigo
        if (enemyCardObject1 != null) enemyCardObject1.SetActive(true);
        if (enemyCardObject2 != null) enemyCardObject2.SetActive(true);
    }

    public int GenerateCard()
    {
        int newCard = cardValues[Random.Range(0, cardValues.Length)];

        // Sumar la nueva carta al total del jugador
        playerTotal += newCard;

        // Actualizar el texto del total en el canvas
        UpdatePlayerCanvas();

        return newCard; // Devuelve la nueva carta si es necesario usarla
    }

    public void UpdatePlayerCanvas()
    {
        playerTotalText.text = playerTotal + "/21";
    }

    public void ResetPlayer()
    {
        playerTotal = 0;
        UpdatePlayerCanvas();
    }


}
