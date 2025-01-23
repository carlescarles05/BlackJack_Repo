using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    public float minWaitTime = 1f;  // Tiempo mínimo antes de cambiar de destino
    public float maxWaitTime = 3f;  // Tiempo máximo antes de cambiar de destino
    public float movementRange = 5f; // Rango de movimiento desde la posición actual
    public float rotationSpeed = 5f; // Velocidad de rotación, configurable desde el Inspector

    private NavMeshAgent agent;  // Referencia al NavMeshAgent
    private Vector3 startPosition;  // Posición inicial del NPC
    public bool canMove = true;

    void Start()
    {
        // Obtener el NavMeshAgent del NPC
        agent = GetComponent<NavMeshAgent>();
        startPosition = transform.position;

        // Elegir el primer destino
        if (canMove) ChooseNewDestination();
    }

    void Update()
    {
        if (!canMove)
        {
            if (agent.enabled)
            {
                agent.isStopped = true; // Detener al agente
                agent.velocity = Vector3.zero; // Asegurarse de que no tenga velocidad residual
            }
            return;
        }

        // Reactivar el movimiento si el agente estaba detenido
        if (agent.isStopped)
        {
            agent.isStopped = false;
        }

        // Verifica si el NPC llegó al destino
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // Espera antes de elegir un nuevo destino
            Invoke(nameof(ChooseNewDestination), Random.Range(minWaitTime, maxWaitTime));
        }
    }

    void ChooseNewDestination()
    {
        /*if (!canMove) return;

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
        }*/
        if (!canMove) return;

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
            // Establecer la posición de destino en el NavMeshAgent
            agent.SetDestination(hit.position);

            // Hacer que el NPC mire hacia el punto de destino
            LookAtDestination(hit.position);
        }
    }

    void LookAtDestination(Vector3 destination)
    {
        /*// Crear una posición destino que tenga la misma altura que el NPC
        Vector3 lookAtPosition = new Vector3(destination.x, transform.position.y, destination.z);

        // Usar LookAt para girar directamente hacia el destino
        transform.LookAt(lookAtPosition);*/
        // Crear una posición destino que tenga la misma altura que el NPC
        Vector3 lookAtPosition = new Vector3(destination.x, transform.position.y, destination.z);

        // Calcular la rotación objetivo
        Quaternion targetRotation = Quaternion.LookRotation(lookAtPosition - transform.position);

        // Interpolar suavemente hacia la rotación objetivo
        transform.rotation = Quaternion.Slerp(
            transform.rotation,        // Rotación actual
            targetRotation,            // Rotación objetivo
            Time.deltaTime * rotationSpeed // Suavidad controlada por rotationSpeed
        );
    }

    public void SetMovement(bool state)
    {
        canMove = state;

        if (!state)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
        else
        {
            if (!agent.enabled)
            {
                agent.enabled = true; // Reactivar el agente si estaba desactivado
            }

            agent.isStopped = false;
            ChooseNewDestination(); // Forzar una nueva ruta al reactivar el movimiento
        }
    }

}
