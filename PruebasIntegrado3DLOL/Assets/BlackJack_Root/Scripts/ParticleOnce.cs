using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleOnce : MonoBehaviour
{
    private ParticleSystem ps;

    void Start()
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
    }
}
