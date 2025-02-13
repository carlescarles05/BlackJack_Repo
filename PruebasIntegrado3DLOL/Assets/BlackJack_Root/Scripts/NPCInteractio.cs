using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;
using UnityEngine.Animations;

public class NPCInteractio : MonoBehaviour
{
    [SerializeField] GameObject NPCView;
    private Transform npcViewTransform; // Transform del NPCView

    public GameObject interactionIndicator; // El indicador (cubo) que se activa/desactiva
    public GameObject dialoguePanel; // Panel de diálogo de la UI
    public TextMeshProUGUI dialogueText; // Texto del diálogo en el panel
    public NPCMovement Move;
    public string[] dialogueLines; // Líneas de diálogo para el NPC
    private int currentLineIndex = 0; // Índice de la línea actual en el diálogo

    private bool playerInRange = false; // Para saber si el jugador está en rango
    private bool isDialogueActive = false; // Para saber si un diálogo está activo

    private bool isTyping = false; // Indica si el texto se está escribiendo

    public MonoBehaviour playerMovementScript; // El script de movimiento del jugador

    // Nuevas referencias para rotación
    public Transform npcTransform; // El transform del NPC que debe girar (cuerpo o cabeza)
    public Transform playerTransform; // Transform del jugador
    public float rotationSpeed = 5f; // Velocidad de rotación del NPC
    public Transform detect;

    private void Start()
    {
        if (NPCView == null)
        {
            NPCView = GameObject.FindGameObjectWithTag("NPCView"); // Asegúrate de asignar el tag "NPCView"
        }

        if (NPCView != null)
        {
            npcViewTransform = NPCView.transform;
            Debug.Log("NPCView asignado automáticamente por tag: " + NPCView.name);
        }
        else
        {
            Debug.LogError("NPCView no encontrado por tag. Asegúrate de que el objeto tenga el tag 'NPCView'.");
        }

        if (interactionIndicator != null) interactionIndicator.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        // Si el jugador está en rango, presiona "E" y no está escribiendo
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isTyping)
        {
            if (!isDialogueActive) 
            {
                StartDialogue();
                detect.LookAt(playerTransform);
                interactionIndicator.SetActive(false);
            }
            else
            {
                AdvanceDialogue();
            }
            // Girar hacia el jugador durante el diálogo
            if (isDialogueActive && npcTransform != null && playerTransform != null)
            {
                RotateToFaceNPCView();
            }
        }
    }

    private void StartDialogue()
    {
        if (dialogueLines.Length > 0)
        {
            isDialogueActive = true;
            currentLineIndex = 0;
            dialoguePanel.SetActive(true);

            // Desactivar movimiento del jugador
            if (playerMovementScript != null)
            {
                playerMovementScript.enabled = false;
            }

            // Desactivar el movimiento del NPC
            if (Move != null)
            {
                Move.SetMovement(false);
            }

            StartCoroutine(TypeSentence(dialogueLines[currentLineIndex]));
        }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true; // Comienza a escribir
        dialogueText.text = ""; // Limpiar el texto actual

        foreach (char letter in sentence)
        {
            dialogueText.text += letter; // Agregar letra por letra
            yield return new WaitForSeconds(0.02f); // Espera antes de mostrar la siguiente letra
        }

        isTyping = false; // Termina de escribir
    }

    private void AdvanceDialogue()
    {
        currentLineIndex++;
        if (currentLineIndex < dialogueLines.Length)
        {
            StartCoroutine(TypeSentence(dialogueLines[currentLineIndex])); // Mostrar la siguiente línea
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);

        // Reactivar movimiento del jugador
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }

        // Reactivar el movimiento del NPC
        if (Move != null)
        {
            Move.SetMovement(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que entra es el jugador
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactionIndicator != null)
            {
                interactionIndicator.SetActive(true); // Activar el indicador
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Verificar si el objeto que sale es el jugador
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactionIndicator != null)
            {
                interactionIndicator.SetActive(false); // Desactivar el indicador
            }

            // Si el jugador sale del rango, cerrar el diálogo y reactivar el movimiento
            if (isDialogueActive)
            {
                EndDialogue();
            }
        }
    }
    private void RotateToFaceNPCView()
    {
        // Calcular la dirección hacia el NPCView
        Vector3 directionToNPCView = (npcViewTransform.position - npcTransform.position).normalized;

        // Evitar que el NPC rote hacia arriba o abajo
        directionToNPCView.y = 0f;

        // Calcular la rotación objetivo
        Quaternion targetRotation = Quaternion.LookRotation(directionToNPCView);

        // Rotar el NPC suavemente hacia el NPCView
        npcTransform.rotation = Quaternion.Slerp(npcTransform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime);

    }

}
