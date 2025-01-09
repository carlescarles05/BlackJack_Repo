using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BJManager;

public class AumentarVida : MonoBehaviour
{
    public int[] yearOptions = { 200, 500, 1000 };  // Opciones de años a sumar
    public Cronometro cronometro;  // Referencia al script Cronometro

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                int randomYears = yearOptions[Random.Range(0, yearOptions.Length)];
                cronometro.AddYears(randomYears);  // Sumar los años al cronómetro
                Destroy(gameObject);  // Destruir el power-up después de usarlo
            }
        }
    }

}
