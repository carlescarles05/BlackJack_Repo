using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAudioManager : MonoBehaviour
{
    [Header("Background Music")]
    [SerializeField] private AudioClip backgroundMusic;
    
    private AudioSource audioSource;

    private void Awake()
    {
        //Create and configure AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        //Play the background music
        audioSource.Play();
    }
    private void OnDestroy()
    {
        //Stop the music when the scene is destroyed
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}
