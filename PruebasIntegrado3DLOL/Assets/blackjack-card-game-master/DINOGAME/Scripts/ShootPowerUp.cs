using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPowerUp : MonoBehaviour
{
    [SerializeField] private float boostAmount = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyEffectsToPlayer(other.gameObject);
            Destroy(gameObject);
        }
    }

    private void ApplyEffectsToPlayer(GameObject player)
    {
        Dinosaur playerScript = player.GetComponent<Dinosaur>();
        if (playerScript != null)
        {
            //playerScript.ApplySpeedBoost(boostAmount, boostDuration);
            playerScript.EnableShooting();
        }
    }
}
