using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleOnce : MonoBehaviour
{
    public ParticleSystem ps;

    /*void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Método para reproducir el ParticleSystem solo una vez
    public void PlayOnce()
    {
        if (!ps.isPlaying)
        {
            ps.Play();
        }
    }*/
    void Start()
    {
        // Busca en el mismo GameObject primero
        ps = GetComponent<ParticleSystem>();

        // Si no lo encuentra, busca en los hijos
        if (ps == null)
        {
            ps = GetComponentInChildren<ParticleSystem>();
        }

        // Log de error si sigue sin encontrarlo
        if (ps == null)
        {
            Debug.LogError("No se encontró un ParticleSystem en el GameObject ni en sus hijos.");
        }
    }

    public void PlayOnce()
    {
        if (ps == null)
        {
            Debug.LogError("ParticleSystem no está asignado en el script ParticleOnce.");
            return;
        }

        if (!ps.isPlaying)
        {
            ps.Play();
        }
    }
}
