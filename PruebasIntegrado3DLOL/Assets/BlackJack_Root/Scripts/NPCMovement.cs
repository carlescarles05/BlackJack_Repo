using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float moveSpeed = 3f;             // Velocidad de movimiento del NPC
    public float minWaitTime = 1f;          // Tiempo mínimo antes de cambiar de destino
    public float maxWaitTime = 3f;          // Tiempo máximo antes de cambiar de destino
    public float movementRange = 5f;        // Rango alrededor del NPC para moverse

    private Vector3 targetPosition;         // Posición objetivo del NPC
    private bool isMoving = false;          // ¿Está el NPC en movimiento?

    void Start()
    {
        ChooseNewDestination();
    }

    void Update()
    {
        if (isMoving)
        {
            // Mover al NPC hacia el objetivo
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Si el NPC ha alcanzado su destino, deja de moverse y espera
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
                Invoke(nameof(ChooseNewDestination), Random.Range(minWaitTime, maxWaitTime));
            }
        }
    }

    void ChooseNewDestination()
    {
        // Elegir una nueva posición dentro del rango definido
        Vector3 randomDirection = new Vector3(
            Random.Range(-movementRange, movementRange),
            0f, // Mantener el NPC en el mismo plano
            Random.Range(-movementRange, movementRange)
        );

        targetPosition = transform.position + randomDirection;
        targetPosition.y = transform.position.y; // Asegurarse de que la altura sea la misma

        isMoving = true;
    }
}
