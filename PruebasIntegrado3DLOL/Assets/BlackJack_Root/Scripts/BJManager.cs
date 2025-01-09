using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    public Button hitButton; // Botón "Robar carta"
    public Button standButton; // Botón "Plantarse"
    public bool isPlayerStanding = false;
    public BJManager bjManager; // Asegúrate de tener esta variable pública en el script

    private bool isGameOver = false;
    private const int maxPoints = 21; // Puntaje máximo (21)
    private int enemyTotal;
    private int roundCount = 0; // Contador de rondas
    private const int maxRounds = 8; // Número máximo de rondas

    [SerializeField] private EnemyAI EnemyAI;  // Si prefieres mantener la variable privada

    // Verifica si StartGame se llama al principio
    private void Awake()
    {
        Debug.Log("Iniciando juego...");
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

        UpdatePlayerTotalUI();
        //StartPlayerTurn(); // Asegurarnos de que empiece el turno del jugador

        StartGame(); // Inicializar el juego
    }

    public void PlayerHit()
    {

        // Al final de tu turno, cambia al turno del enemigo
        //EndTurn(); // Ahora pasamos el control al enemigo*/
        if (currentTurn != Turn.Player) return;

        // Generar y agregar nueva carta
        int cardValue = GenerateCard();
        playerTotal += cardValue; // Sumar el valor de la carta
        UpdatePlayerTotalUI();

        // Instanciar carta en la posición correspondiente
        GameObject card = Instantiate(cardPrefab, playerCardSpawnPoint);
        card.transform.localPosition += new Vector3(cardOffset * playerCards.Count, 0, 0);
        playerCards.Add(card);

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

        // Finalizar el turno del jugador
        EndTurn();

    }

    public int GenerateCard()
    {
        int cardValue = Random.Range(1, 11); // Genera un valor entre 1 y 10
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
                hitButton.interactable = true; // Permitir interacción del jugador
                standButton.interactable = true;
                break;

            case Turn.Enemy:
                Debug.Log("Es el turno del enemigo.");
                hitButton.interactable = false; // Desactivar botones del jugador
                standButton.interactable = false;

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

        /*roundCount++; // Incrementar contador de rondas

        Verificar si hemos llegado a la cantidad máxima de rondas
        if (roundCount >= maxRounds)
        {
            EndGameRoundLimit(); // Finalizar el juego si llegamos al límite de rondas
        }
        else
        {
            HandleRounds(); // Llamar al método de manejo de rondas si no hemos llegado al límite
        }*/

    }

    public void EndRound(bool playerWins)
    {
        // Si el jugador gana o pierde, se muestra el resultado
        if (playerWins)
        {
            Debug.Log("¡Has ganado esta ronda!");
        }
        else
        {
            Debug.Log("Has perdido esta ronda.");
        }

        HandleRounds();

        // Incrementar el contador de rondas
        roundCount++;

        // Comprobar si hemos llegado al límite de rondas
        if (roundCount >= maxRounds)
        {
            Debug.Log("¡Se han jugado 8 rondas! El juego ha terminado.");
            EndGameRoundLimit();
        }
        else
        {
            // Reiniciar la partida para la siguiente ronda
            StartGame();
        }
        
    }


    private void EndGameRoundLimit()
    {
        Debug.Log("¡Se han jugado 8 rondas! El juego ha terminado.");

        // Desactivar los botones
        hitButton.interactable = false;
        standButton.interactable = false;

        // Mostrar mensaje de fin de juego
        Debug.Log("Fin del juego. Han transcurrido 8 rondas.");

        // Aquí puedes agregar más lógica, como mostrar un UI de fin de juego si lo deseas.
    }

    public void EndGame(bool? playerWins)
    {
        isGameOver = true;

        hitButton.interactable = false;
        standButton.interactable = false;

        if (playerWins == true)
        {
            Debug.Log("¡Has ganado!");
        }
        else if (playerWins == false)
        {
            Debug.Log("Has perdido.");
        }
        else
        {
            Debug.Log("Empate.");
        }

        // Esperamos un poco antes de reiniciar el juego para dar tiempo a que el jugador vea el resultado.
        StartCoroutine(WaitForEndOfGame());
    }

    private IEnumerator WaitForEndOfGame()
    {
        // Esperar 2 segundos para que el jugador vea el mensaje
        yield return new WaitForSeconds(2f);

        // Llamar a StartGame para reiniciar el juego
        StartGame();
        isGameOver = false; // Resetear el estado del juego

        // Opcional: podrías restablecer la interfaz de usuario u otros elementos aquí si es necesario

        Debug.Log("Comenzando una nueva ronda...");
    }


    public IEnumerator EnemyTurnRoutine()
    {

        Debug.Log("Turno del enemigo comenzado.");

        yield return new WaitForSeconds(1f); // Pausa simulada para el turno del enemigo
        Debug.Log("Eres tonto");
        // Generar carta
        int cardValue = GenerateCard();
        enemyAI.enemyTotal += cardValue;
        Debug.Log($"Valor de la carta: {cardValue}, Total enemigo: {enemyTotal}");

        UpdateEnemyTotalUI();

        // Instanciar la carta visualmente
        GameObject card = Instantiate(cardPrefab, enemyCardSpawnPoint);
        card.transform.localPosition += new Vector3(cardOffset * enemyCards.Count, 0, 0);
        enemyCards.Add(card);

        Debug.Log($"El enemigo pidió una carta: {cardValue}. Total del enemigo: {enemyTotal}");

        // Verificar si el enemigo se pasó de 21
        if (enemyAI.enemyTotal > 21)
        {
            Debug.Log("¡El enemigo se pasó de 21!");
            EndGame(true); // El jugador gana
            
        }

        // Finalizar el turno del enemigo
        Debug.Log("El enemigo termina su turno.");

        EndTurn(); // Cambiar al turno del jugador
        yield break;
    }

    private void UpdateEnemyTotalUI()
    {
        if (enemyAI != null && enemyAI.enemyTotalText != null)
        {
            Debug.Log("Cabron");
            enemyAI.enemyTotalText.text = $"{enemyAI.enemyTotal}/21";
        }
        else
        {
            Debug.LogError("enemyAI o enemyTotalText no está asignado en el Inspector.");
        }

    }

    private void UpdatePlayerTotalUI()
    {
        if (playerTotalText != null)
        {
            playerTotalText.text = playerTotal.ToString() + "/21";
            //playerTotalText.text = $"{playerTotal}/21";
        }
        else
        {
            Debug.LogError("PlayerTotalText no está asignado en el Inspector.");
        }
    }

    public void StartGame()
    {
        // Reiniciar totales
        playerTotal = 0;
        enemyAI.enemyTotal = 0;

        // Limpiar las cartas
        foreach (var card in playerCards)
        {
            Destroy(card);
        }
        playerCards.Clear();

        foreach (var card in enemyAI.enemyCards)
        {
            Destroy(card);
        }
        enemyAI.enemyCards.Clear();

        Debug.Log("Juego iniciado: Totales reiniciados. playerTotal = 0, enemyTotal = 0");

        // Generar cartas iniciales para el jugador
        for (int i = 0; i < 2; i++)
        {
            int cardValue = GenerateCard();
            Debug.Log($"Jugador recibe carta inicial {i + 1}: {cardValue}");
            playerTotal += cardValue;
            GameObject card = Instantiate(cardPrefab, playerCardSpawnPoint);
            card.transform.localPosition += new Vector3(cardOffset * playerCards.Count, 0, 0);
            playerCards.Add(card);
        }
        Debug.Log("Total inicial del jugador: " + playerTotal);

        // Generar cartas iniciales para el enemigo
        for (int i = 0; i < 2; i++)
        {
            int cardValue = GenerateCard();
            Debug.Log($"Enemigo recibe carta inicial {i + 1}: {cardValue}");
            Debug.Log("Valor de enemy total = " + enemyAI.enemyTotal.ToString());
            enemyAI.enemyTotal = enemyAI.enemyTotal + cardValue;
            Debug.Log("Valor de enemy total = " + enemyAI.enemyTotal.ToString());
            GameObject card = Instantiate(cardPrefab, enemyCardSpawnPoint);
            card.transform.localPosition += new Vector3(cardOffset * enemyCards.Count, 0, 0);
            enemyCards.Add(card);
        }
        Debug.Log("Total inicial del enemigo: " + enemyAI.enemyTotal);

        // Actualizar las UI
        UpdatePlayerTotalUI();
        enemyAI.UpdateEnemyTotalUI();

        // Aseguramos que el turno del jugador comienza
        currentTurn = Turn.Player;
        Debug.Log("Turno inicial: Jugador.");

        // Activar los botones al reiniciar la ronda
        hitButton.interactable = true;
        standButton.interactable = true;
    }

    public void EnemyHit()
    {
        // Asegurarse de que es el turno del enemigo
        if (currentTurn != Turn.Enemy) return;

        // Generar una carta para el enemigo
        int cardValue = GenerateCard();
        Debug.Log($"Enemigo recibe carta: {cardValue}");

        // Sumar el valor de la carta al total del enemigo (usando enemyAI.enemyTotal)
        enemyAI.enemyTotal += cardValue; // Usamos enemyAI.enemyTotal

        Debug.Log($"Nuevo total del enemigo después de la carta: {enemyAI.enemyTotal}");

        // Instanciar la carta visualmente
        GameObject card = Instantiate(cardPrefab, enemyCardSpawnPoint);
        card.transform.localPosition += new Vector3(cardOffset * enemyCards.Count, 0, 0);
        enemyCards.Add(card);

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
