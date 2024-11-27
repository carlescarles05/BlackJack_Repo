using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    public float minWaitTime = 1f;  // Tiempo mínimo antes de cambiar de destino
    public float maxWaitTime = 3f;  // Tiempo máximo antes de cambiar de destino
    public float movementRange = 5f; // Rango de movimiento desde la posición actual

    private NavMeshAgent agent;  // Referencia al NavMeshAgent
    private Vector3 startPosition;  // Posición inicial del NPC

    void Start()
    {
        // Obtener el NavMeshAgent del NPC
        agent = GetComponent<NavMeshAgent>();
        startPosition = transform.position;

        // Elegir el primer destino
        ChooseNewDestination();
    }

    void Update()
    {
        // Verifica si el NPC llegó al destino
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // Espera antes de elegir un nuevo destino
            Invoke(nameof(ChooseNewDestination), Random.Range(minWaitTime, maxWaitTime));
        }
    }

    void ChooseNewDestination()
    {
        // Generar un destino aleatorio dentro del rango
        Vector3 randomDirection = new Vector3(
            Random.Range(-movementRange, movementRange),
            0f,
            Random.Range(-movementRange, movementRange)
        );

        Vector3 newDestination = startPosition + randomDirection;

        // Establecer el destino en el NavMeshAgent
        if (NavMesh.SamplePosition(newDestination, out NavMeshHit hit, movementRange, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}
