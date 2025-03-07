using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    public float minWaitTime;  // Tiempo m�nimo antes de cambiar de destino
    public float maxWaitTime;  // Tiempo m�ximo antes de cambiar de destino
    public float movementRange; // Rango de movimiento desde la posici�n actual
    public float rotationSpeed = 5f; // Velocidad de rotaci�n, configurable desde el Inspector

    bool choosingDestination;

    private NavMeshAgent agent;  // Referencia al NavMeshAgent
    private Vector3 startPosition;  // Posici�n inicial del NPC
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

        // Verifica si el NPC lleg� al destino
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && choosingDestination == false)
        {
            // Espera antes de elegir un nuevo destino
            Debug.Log("Eligiendo nuevo destino");
            choosingDestination = true;
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
            // Establecer la posici�n de destino en el NavMeshAgent
            agent.SetDestination(hit.position);

            // Hacer que el NPC mire hacia el punto de destino
            LookAtDestination(hit.position);
        }
        choosingDestination = false;
    }

    void LookAtDestination(Vector3 destination)
    {
        /*// Crear una posici�n destino que tenga la misma altura que el NPC
        Vector3 lookAtPosition = new Vector3(destination.x, transform.position.y, destination.z);

        // Usar LookAt para girar directamente hacia el destino
        transform.LookAt(lookAtPosition);*/
        // Crear una posici�n destino que tenga la misma altura que el NPC
        Vector3 lookAtPosition = new Vector3(destination.x, transform.position.y, destination.z);

        // Calcular la rotaci�n objetivo
        Quaternion targetRotation = Quaternion.LookRotation(lookAtPosition - transform.position);

        // Interpolar suavemente hacia la rotaci�n objetivo
        transform.rotation = Quaternion.Slerp(
            transform.rotation,        // Rotaci�n actual
            targetRotation,            // Rotaci�n objetivo
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
