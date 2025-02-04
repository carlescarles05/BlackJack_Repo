using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BJManager : MonoBehaviour
{
    public EnemyAI enemyAI; // Referencia al script EnemyAI
    public int playerTotal = 0; // Puntos totales del jugador
    public List<GameObject> playerCards = new List<GameObject>(); // Cartas del jugador
    public List<GameObject> enemyCards = new List<GameObject>(); // Cartas del enemigo
    public Transform playerCardSpawnPoint; // Punto donde aparecen las cartas del jugador
    public Transform enemyCardSpawnPoint; // Punto donde aparecen las cartas del enemigo
    public GameObject cardPrefab; // Prefab de las cartas
    public int cardOffset = 30; // Espaciado entre cartas visibles
    public TextMeshProUGUI playerTotalText; // Referencia al texto del canvas
    public TextMeshProUGUI roundsTotalText; 
    public Button hitButton; // Botón "Robar carta"
    public Button standButton; // Botón "Plantarse"
    public Button standUpButton;
    public BJManager bjManager; // Asegúrate de tener esta variable pública en el script
    public Cronometro cronometro;
    public Cronometro cronometroEnemy;

    private bool isGameOver = false;
    private const int maxPoints = 21; // Puntaje máximo (21)
    public int roundCount = 1; // Contador de rondas
    private const int maxRounds = 9; // Número máximo de rondas
    bool blockDobleEnd = false;
    public bool enemyStand = false;
    public bool playerStand = false;

    //[SerializeField] private EnemyAI EnemyAI;  // Si prefieres mantener la variable privada

    public DeckManager deckManager;

    private static BJManager instance;

    public static BJManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("BJManager is null!");
            }
            return instance;
        }
    }

    // Verifica si StartGame se llama al principio
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Debug.Log("Iniciando juego...");
        deckManager = FindObjectOfType<DeckManager>();
        StartGame(); // Asegúrate de que se llame correctamente.
    }

    public enum Turn
    {
        Player,
        Enemy
    }

    public Turn currentTurn = Turn.Player;
    private void Start()
    {
        // Bloquear el cursor al centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
        // Hacer el cursor invisible
        Cursor.visible = false;
        if (enemyAI == null)
        {
            enemyAI = FindObjectOfType<EnemyAI>();
            if (enemyAI == null)
            {
                Debug.LogError("EnemyAI no encontrado. Por favor, asigna el script EnemyAI en el Inspector.");
            }
        }

        hitButton.onClick.AddListener(PlayerHit);
        standButton.onClick.AddListener(PlayerStand);
        //standButton.onClick.AddListener();

        UpdatePlayerTotalUI();
        StartGame(); // Inicializar el juego
    }

    public void PlayerHit()
    {
        if (currentTurn != Turn.Player || blockDobleEnd) return;

        // Generar y agregar nueva carta
        int cardValue = GenerateCard();
        deckManager.DrawCard();
        playerTotal += cardValue; // Sumar el valor de la carta
        UpdatePlayerTotalUI();

        // Verificar si el jugador se pasó de 21
        if (playerTotal > maxPoints)
        {
            Debug.Log("¡Te pasaste de 21!");
            EndGame(false); // Finaliza el juego si el jugador se pasa de 21
            return;           
        }

        // Cambiar turno al enemigo
        EndTurn();
    }

    public void PlayerStand()
    {
        if (currentTurn != Turn.Player || isGameOver) return;

        Debug.Log("Jugador se planta.");

        if (enemyStand == true) 
        { 
            EndGame(null);            
        }
        else
        {
            playerStand = true;
            EndTurn();
            ToggleButtons(false);
        }
    }

    public int GenerateCard()
    {
        int cardValue = Random.Range(1, deckManager.cardMaterials.Count); // Genera un valor entre 1 y el tamaño de cardMaterials
        deckManager.cardV = cardValue; 

        // Instanciar carta en la posición correspondiente
        GameObject card = Instantiate(cardPrefab, playerCardSpawnPoint);
        card.transform.localPosition += new Vector3(cardOffset * playerCards.Count, 0, 0);
        card.transform.localRotation = Quaternion.Euler(0, 0, 0); // Ajustar rotación a 90 grados si es necesario
                                                                  // cardRenderer.material = deckManager.                                                          

        // Cambiar el material de la carta
        Renderer cardRenderer = card.GetComponent<Renderer>();
        if (cardRenderer != null)
        {
            Debug.Log(cardValue + " Valor actual de la carta");
            cardRenderer.material = deckManager.cardMaterials[cardValue - 1]; // Usar cardMaterials del DeckManager
        }

        playerCards.Add(deckManager.cardPrefab);

        Debug.Log("Carta generada: " + cardValue);
        return cardValue;
    }

    private void HandleRounds()
    {
        if (isGameOver) return; // Detener las rondas si el juego ha terminado.

        // Control del turno actual
        switch (currentTurn)
        {
            case Turn.Player:
                Debug.Log("Es el turno del jugador.");
                ToggleButtons(true);
                break; 

            case Turn.Enemy:
                Debug.Log("Es el turno del enemigo.");
                ToggleButtons(false);
                StartCoroutine(EnemyTurnRoutine());
                break;
        }
    }

    public void EndTurn()
    {
        if (currentTurn == Turn.Player)
        {
            Debug.Log("El jugador termina su turno.");
            currentTurn = Turn.Enemy; // Cambiar turno al enemigo.
        }
        else if (currentTurn == Turn.Enemy)
        {
            Debug.Log("El enemigo termina su turno.");
            currentTurn = Turn.Player; // Cambiar turno al jugador.
        }

        HandleRounds(); // Llamar al método de manejo de rondas.

    }

    private void EndGameRoundLimit()
    {
        Debug.Log("¡Se han jugado 8 rondas! El juego ha terminado.");

        // Desactivar los botones
        ToggleButtons(false);

        // Mostrar mensaje de fin de juego
        Debug.Log("Fin del juego. Han transcurrido 8 rondas.");
    }

    public void EndGame(bool? playerWins)
    {
        isGameOver = true;
        ToggleButtons(false);
        enemyAI.enemyStand = true;
        UpdateEnemyTotalUI();
        
        if (playerWins != null) 
        {
            if (playerWins == true)
            {
                Debug.Log("¡Has ganado!");
                bool isEnd = cronometroEnemy.SubtractYears(200);
                if (isEnd) gameDead();
            }
            else if (playerWins == false)
            {
                Debug.Log("Has perdido.");
                bool isEnd = cronometro.SubtractYears(200);
                if (isEnd) gameDead();
            }
        }
        else
        {
            if ((21 - playerTotal) > (21 - enemyAI.enemyTotal))
            {
                Debug.Log("LOSE");
                bool isEnd = cronometro.SubtractYears(200);
                if (isEnd)gameDead();
            }
            else if ((21 - playerTotal) < (21 - enemyAI.enemyTotal))
            {
                Debug.Log("WIN");
                bool isEnd = cronometroEnemy.SubtractYears(200);
                if (isEnd) gameDead();
            }
            else
            {
                Debug.Log("EMPATE LOKO");
                bool isEnd = cronometro.SubtractYears(200);
                isEnd = cronometroEnemy.SubtractYears(200);
                if (isEnd) gameDead();
            }
        }

        deckManager.ResetInstance();

        roundCount++;        
        // Esperamos un poco antes de reiniciar el juego para dar tiempo a que el jugador vea el resultado.
        StartCoroutine(WaitForEndOfGame());
        UpdateRoundsText();
    }

    public void gameDead()
    {
        StopAllCoroutines();
        if (cronometroEnemy.currentYear <= 0)
        {
            SceneManager.LoadScene(3);
        }
        else
        {
            SceneManager.LoadScene(4);
        }
    }

    private IEnumerator WaitForEndOfGame()
    {
        blockDobleEnd = true;
        // Esperar 2 segundos para que el jugador vea el mensaje
        yield return new WaitForSeconds(2f);

        // Llamar a StartGame para reiniciar el juego
        StartGame();
        isGameOver = false; // Resetear el estado del juego

        blockDobleEnd = false;
        Debug.Log("Comenzando una nueva ronda...");
    }

    public IEnumerator EnemyTurnRoutine()
    {

        // Debug.Log(21 - playerTotal < 21 - enemyAI.enemyTotal || 21 - enemyAI.enemyTotal >= 10);       

        yield return new WaitForSeconds(1f);

        if (enemyStand)
        {
            yield return null;
        }

        if ((21 - enemyAI.enemyTotal) >= 5)
        {
            Debug.Log("Turno del enemigo comenzado.");
            int cardValue = Random.Range(1, deckManager.cardMaterials.Count); // Genera un valor entre 1 y el tamaño de cardMaterials;
            deckManager.cardV = cardValue;
            deckManager.DrawCardEnemy();
            enemyAI.enemyAddValueToTotal(cardValue);
            Debug.Log($"Valor de la carta: {cardValue}, Total enemigo: {enemyAI.enemyTotal}");

            // Instanciar la carta visualmente
            GameObject card = Instantiate(cardPrefab, enemyCardSpawnPoint);
            card.transform.localPosition += new Vector3(cardOffset * enemyCards.Count, 0, 0);
            enemyCards.Add(deckManager.cardPrefab);
            UpdateEnemyTotalUI();


            Debug.Log($"El enemigo pidió una carta: {cardValue}. Total del enemigo: {enemyAI.enemyTotal}");

            // Verificar si el enemigo se pasó de 21
            if (enemyAI.enemyTotal > 21)
            {
                EndGame(true); // El jugador gana
            }
            EndTurn(); // Cambiar al turno del jugador
            if (playerStand)
            {
                EndTurn();
            }
            yield break;
        }
        else
        {

            if (21 - playerTotal < 21 - enemyAI.enemyTotal || Random.Range(1, 11) <= 3)
            {
                Debug.Log("Turno del enemigo comenzado.");

                // Verificar si el enemigo se pasó de 21
                if (enemyAI.enemyTotal > 21)
                {
                    EndGame(true); // El jugador gana
                }
                if (playerStand == true)
                {
                    EndGame(null);
                }
                else
                {
                    EndTurn();
                }
                yield break;
            }
            else
            {
                if (playerStand == true)
                {
                    EndGame(null);
                }
                else
                {
                    enemyStand = true;
                    EnemyStand();
                }     
            }
        }
    }

    private void UpdateEnemyTotalUI()
    {
        if (enemyAI != null && enemyAI.enemyTotalText != null)
        {
            enemyAI.UpdateEnemyTotalUI();
        }
    }

    private void UpdatePlayerTotalUI()
    {
        if (playerTotalText != null)
        {
            playerTotalText.text = playerTotal.ToString() + "/21";
        }
    }

    // Método para eliminar los prefabs generados
    public void ClearGeneratedPrefabs()
    {
        if (cardPrefab != null)
        {
            Destroy(cardPrefab);
            cardPrefab = null; // Opcional, si necesitas liberar la referencia
        }
    }

    public void StartGame()
    {
        // Reiniciar totales
        playerTotal = 0;
        enemyAI.ResetValues();
        deckManager.cardsAlreadyDrawn = 0;
        deckManager.cardsAlreadyDrawnEnemy = 0;

        playerCards.Clear();
        enemyCards.Clear();

        Debug.Log("Juego iniciado: Totales reiniciados. playerTotal = 0, enemyTotal = 0");

        // Actualizar las UI
        UpdatePlayerTotalUI();
        enemyAI.UpdateEnemyTotalUI();

        // Aseguramos que el turno del jugador comienza
        currentTurn = Turn.Player;
        Debug.Log("Turno inicial: Jugador.");

        // Activar los botones al reiniciar la ronda
        ToggleButtons(true);

        playerStand = false;
        enemyStand = false;

    }

    public void AddTime(int years)
    {
        cronometro.AddYears(years);
    }

    public void SubstractTime(int years)
    {
        cronometro.SubtractYears(years);
    }

        public void EnemyHit()
    {
        // Asegurarse de que es el turno del enemigo
        if (currentTurn != Turn.Enemy) return;

        if (21 - playerTotal < 21 - enemyAI.enemyTotal || 21 - enemyAI.enemyTotal >= 10)
        {
            // Generar una carta para el enemigo
            int cardValue = GenerateCard();
            Debug.Log($"Enemigo recibe carta: {cardValue}");

            // Sumar el valor de la carta al total del enemigo (usando enemyAI.enemyTotal)
            enemyAI.enemyTotal += cardValue; // Usamos enemyAI.enemyTotal

            Debug.Log($"Nuevo total del enemigo después de la carta: {enemyAI.enemyTotal}");

            // Instanciar la carta visualmente
            GameObject card = Instantiate(cardPrefab, enemyCardSpawnPoint);
            card.transform.localPosition += new Vector3(cardOffset * enemyCards.Count, 0, 0);
            enemyCards.Add(cardPrefab);

            // Actualizar la UI del enemigo
            UpdateEnemyTotalUI();

            // Verificar si el enemigo se pasa de 21
            if (enemyAI.enemyTotal > maxPoints)
            {
                Debug.Log("El enemigo se pasó de 21. ¡Has ganado!");
                bjManager.EndGame(true); // Notificar al BJManager que el jugador ganó
            }
        }
    }

    public void EnemyStand()
    {
        if (currentTurn != Turn.Enemy || isGameOver) return;

        Debug.Log("Enemy se planta.");

        // Finalizar el turno del enemigo
        EndTurn();
    }

    public void UpdateRoundsText()
    {
        if(roundsTotalText != null)
        {
            if(roundCount<=8)roundsTotalText.text = "ROUND  " + roundCount + "/8";
            Debug.Log("Se suma una ronda");
        }
        else 
        {
            roundsTotalText.text = "FIN DEL JUEGO";
        }
    }

    public void ToggleButtons(bool mode)
    {
        hitButton.interactable = mode;
        standButton.interactable = mode;
        standUpButton.interactable = mode;
        
    }
} 


