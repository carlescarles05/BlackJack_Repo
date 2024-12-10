using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCInteractio : MonoBehaviour
{
    public GameObject interactionIndicator; // El indicador (cubo) que se activa/desactiva
    public GameObject dialoguePanel; // Panel de di�logo de la UI
    public TextMeshProUGUI dialogueText; // Texto del di�logo en el panel
    public NPCMovement Move;
    public string[] dialogueLines; // L�neas de di�logo para el NPC
    private int currentLineIndex = 0; // �ndice de la l�nea actual en el di�logo

    private bool playerInRange = false; // Para saber si el jugador est� en rango
    private bool isDialogueActive = false; // Para saber si un di�logo est� activo

    private void Start()
    {
        // Asegurarnos de que el indicador y el panel de di�logo est�n apagados al inicio
        if (interactionIndicator != null) interactionIndicator.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        // Si el jugador est� en rango y presiona "E"
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isDialogueActive)
            {
                StartDialogue();
            }
            else
            {
                AdvanceDialogue();
            }
        }
    }

    private void StartDialogue()
    {
        if (dialogueLines.Length > 0)
        {
            isDialogueActive = true;
            currentLineIndex = 0; // Empezar desde la primera l�nea
            dialoguePanel.SetActive(true); // Mostrar el panel de di�logo

            // Desactivar el movimiento del NPC
            if (Move != null)
            {
                Debug.Log("Desactivando movimiento del NPC.");
                Move.SetMovement(false); // Pausar movimiento
            }
            else
            {
                Debug.LogError("El componente NPCMovement no est� asignado.");
            }

            // Iniciar la animaci�n del texto
            StartCoroutine(TypeSentence(dialogueLines[currentLineIndex]));
        }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = ""; // Limpiar el texto actual

        foreach (char letter in sentence)
        {
            dialogueText.text += letter; // Agregar letra por letra
            yield return new WaitForSeconds(0.05f); // Espera antes de mostrar la siguiente letra
        }
    }

    private void AdvanceDialogue()
    {
        currentLineIndex++;
        if (currentLineIndex < dialogueLines.Length)
        {
            StartCoroutine(TypeSentence(dialogueLines[currentLineIndex])); // Mostrar la siguiente l�nea
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false); // Ocultar el panel de di�logo

        // Reactivar el movimiento del NPC
        if (Move != null)
        {
            Debug.Log("Reactivando movimiento del NPC.");
            Move.SetMovement(true); // Reactivar el movimiento
        }
        else
        {
            Debug.LogError("El componente NPCMovement no est� asignado.");
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

            // Si el jugador sale del rango, cerrar el di�logo y reactivar el movimiento
            if (isDialogueActive)
            {
                EndDialogue();
            }
        }
    }

}
