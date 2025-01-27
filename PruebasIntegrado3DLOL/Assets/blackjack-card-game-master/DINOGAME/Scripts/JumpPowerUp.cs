using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPowerUp : MonoBehaviour
{
    [SerializeField] private float jumpBoost = 8f; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Dinosaur playerScript = other.GetComponent<Dinosaur>();
            if (playerScript != null)
            {
                playerScript.ApplyJumpBoost(jumpBoost);
            }
            Destroy(gameObject);
        }
    }
}
