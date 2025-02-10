using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManagerSMtwo : MonoBehaviour
{
    public static SFXManagerSMtwo Instance;
    [Header("Audio Source for sound Effects")]
    private AudioSource sfxSource;

    [Header("GuessTheCard Sound Effects")]
    public AudioClip spin;
    //public AudioClip machardSound;
    public AudioClip earnTime;
    public AudioClip jackPot;
    public AudioClip noCredit;
    public AudioClip cashed;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
    }
    // Play a sound effect by passing a specific clip.

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }


    //play the "Select"sound
    public void Spin()
    {
        PlaySFX(spin);
        sfxSource.loop= true;
        sfxSource.Play();
    }
    public void StopSpinSound()
    {
        sfxSource.Stop();
    }

    public void EarnTime()
    {
        PlaySFX(earnTime);
    }
    public void Jackpot()
    {
        PlaySFX(jackPot);
    }
    public void NoCredit()
    {
        PlaySFX(noCredit);
    }
    public void Cashed()
    {
        PlaySFX(cashed);
    }
}
