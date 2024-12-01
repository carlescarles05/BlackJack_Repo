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
                if (obj.CompareTag("Chair"))
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

        if (playerMovement != null) playerMovement.enabled = false;
        if (characterController != null) characterController.enabled = false;

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
        int playerCard1 = cardValues[Random.Range(0, cardValues.Length)];
        int playerCard2 = cardValues[Random.Range(0, cardValues.Length)];
        playerTotal = playerCard1 + playerCard2;

        if (playerTotalText != null)
        {
            playerTotalText.text = playerTotal + "/21";
        }
    }

    public void GenerateEnemyCards()
    {
        int enemyCard1 = cardValues[Random.Range(0, cardValues.Length)];
        int enemyCard2 = cardValues[Random.Range(0, cardValues.Length)];
        enemyTotal = enemyCard1 + enemyCard2;
        enemyTotalText.text = enemyTotal + "/21";

        // Mostrar las cartas del enemigo
        if (enemyCardObject1 != null) enemyCardObject1.SetActive(true);
        if (enemyCardObject2 != null) enemyCardObject2.SetActive(true);
    }

    public int GenerateCard()
    {
        int newCard = cardValues[Random.Range(0, cardValues.Length)];
        return newCard;
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
