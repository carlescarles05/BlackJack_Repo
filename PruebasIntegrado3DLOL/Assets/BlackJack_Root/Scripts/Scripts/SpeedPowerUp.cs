using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : MonoBehaviour
{
    [SerializeField] private float speedBoostAmount = 5f; 
    [SerializeField] private float powerUpDuration = 5f; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Aplica el aumento de velocidad al jugador
            Dinosaur playerScript = other.GetComponent<Dinosaur>();
            if (playerScript != null)
            {
                playerScript.ApplySpeedBoost(speedBoostAmount, powerUpDuration);
            }
            Destroy(gameObject);
        }
    }
}
