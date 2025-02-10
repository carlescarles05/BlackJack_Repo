using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;
    [Header("Audio Source for sound Effects")]
    [SerializeField] private AudioSource backgroundmusic;
    private AudioSource sfxSource;
    
    [Header("GuessTheCard Sound Effects")]
    public AudioClip cardHover;
    public AudioClip machardSound;
    public AudioClip earnTime;
    public AudioClip win;
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
        //
        DisableEnvironmentAudio();
    }
    // Play a sound effect by passing a specific clip.
    public void EnableEnvironmentAudio()
    {
        backgroundmusic.gameObject.SetActive(true);
    }
    public void DisableEnvironmentAudio()
    {
        backgroundmusic.gameObject.SetActive(false);
    }
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }


    //play the "Select"sound
    public void CardHoverSound()
    {
        PlaySFX(cardHover);
    }
    public void MachineCardSound()
    {
        PlaySFX(machardSound);
    }

    public void EarnTime()
    {
        PlaySFX(earnTime);
    }
    public void Win()
    {
        PlaySFX(win);
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
