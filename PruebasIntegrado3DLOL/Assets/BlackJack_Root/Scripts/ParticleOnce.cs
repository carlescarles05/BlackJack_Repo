using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleOnce : MonoBehaviour
{
    private ParticleSystem ps;

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
        ps = GetComponentInChildren<ParticleSystem>(); // Busca en los hijos
                                                       // O usa GetComponentInParent si está en un padre.
        if (ps == null)
        {
            Debug.LogError("No se encontró un ParticleSystem en el GameObject o en sus hijos/padres.");
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
