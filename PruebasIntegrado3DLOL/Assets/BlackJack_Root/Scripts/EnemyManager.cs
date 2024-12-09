using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public TextMeshProUGUI enemyTotalText;  // Referencia al texto del canvas del enemigo
    public GameObject enemyCard1;          // Objeto de la primera carta
    public GameObject enemyCard2;          // Objeto de la segunda carta
    public GameObject enemySmoke1;         // Partícula para la primera carta
    public GameObject enemySmoke2;         // Partícula para la segunda carta
    public int[] cardValues = { 1, 2, 3, 4, 5, 6 }; // Valores posibles de las cartas

    public int enemyTotal;                // Total de las cartas del enemigo

    public void GenerateEnemyCards()
    {
        // Generar valores aleatorios para las cartas del enemigo
        int card1Value = cardValues[Random.Range(0, cardValues.Length)];
        int card2Value = cardValues[Random.Range(0, cardValues.Length)];
        enemyTotal = card1Value + card2Value;

        // Actualizar el texto del canvas
        /*if (enemyTotalText != null)
        {
            enemyTotalText.text = enemyTotal + "/21";
        }*/

        // Activar las cartas y partículas
        StartCoroutine(ActivateCardWithParticles(enemySmoke1, enemyCard1));
        StartCoroutine(ActivateCardWithParticles(enemySmoke2, enemyCard2));
    }

    private IEnumerator ActivateCardWithParticles(GameObject smoke, GameObject card)
    {
        if (smoke != null)
        {
            smoke.SetActive(true);  // Activar la partícula de humo
            ParticleOnce smokeScript = smoke.GetComponent<ParticleOnce>();
            if (smokeScript != null)
            {
                smokeScript.PlayOnce();
            }
        }

        // Esperar un segundo antes de mostrar la carta
        yield return new WaitForSeconds(1f);

        if (card != null)
        {
            card.SetActive(true); // Mostrar la carta
        }

        // Desactivar el humo después de mostrar la carta
        if (smoke != null)
        {
            DeactivateParticleSystem(smoke);
        }
    }

    private void DeactivateParticleSystem(GameObject smoke)
    {
        ParticleSystem ps = smoke.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Stop();
            ps.Clear();
            smoke.SetActive(false);
        }
    }

    public int GetEnemyTotal()
    {
        return enemyTotal;
    }

    public int GenerateCard()
    {
        int newCard = cardValues[Random.Range(0, cardValues.Length)];
        return newCard;
    }

    /*public void UpdateEnemyCanvas()
    {
        enemyTotalText.text = enemyTotal + "/21";
    }

    public void ResetEnemy()
    {
        enemyTotal = 0;
        UpdateEnemyCanvas();
    }*/

}
