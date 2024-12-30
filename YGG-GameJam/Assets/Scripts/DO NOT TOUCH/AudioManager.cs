using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Music Properties")]
    public AudioSource musicSource;
    public AudioClip[] musicClips;
    [Header("Sound Properties")]
    public AudioSource upSfxSource; // unaffected pitch
    public AudioSource apSfxSource; // affected pitch
    public AudioClip[] soundEffects;

    public void Awake()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if (currentScene == 1)
        {
            PlayMainMenuMusic();
        }
        else
        {
            musicSource.Stop();
        }

        upSfxSource.pitch = 1;
    }
    // MUSIC PROPERTIES =================================
    public void PlayMainMenuMusic()
    {
        musicSource.Stop();
        musicSource.clip = musicClips[0];
        musicSource.loop = true;
        musicSource.volume = 0.5f;
        musicSource.Play();
        
    }
    public void PlayInGameMusic()
    {
        musicSource.Stop();
        musicSource.clip = musicClips[1];
        musicSource.loop = true;
        musicSource.volume = 0.3f;
        musicSource.Play();
    }
    // SFX PROPERTIES =================================
    public void PlayButtonClickSound()
    {
        apSfxSource.pitch = Random.Range(0.7f, 1);
        apSfxSource.PlayOneShot(soundEffects[0]);
    }
    public void PlayRouletteSound()
    {
        upSfxSource.PlayOneShot(soundEffects[1]);
        upSfxSource.volume = 0.5f;
    }
    public void PlayCountdownSound()
    {
        upSfxSource.PlayOneShot(soundEffects[2]);
        upSfxSource.volume = 0.25f;
    }
    public void PlayGoSound()
    {
        upSfxSource.PlayOneShot(soundEffects[3]);
        upSfxSource.volume = 0.25f;
    }
    public void PlayWinSound()
    {
        upSfxSource.PlayOneShot(soundEffects[4]);
        upSfxSource.volume = 0.3f;
    }
    public void PlayTieSound()
    {
        upSfxSource.PlayOneShot(soundEffects[5]);
        upSfxSource.volume = 0.3f;
    }

    public void PlayWalkSound()
    {
        apSfxSource.pitch = Random.Range(0.8f, 1);
        apSfxSource.PlayOneShot(soundEffects[6]);
    }
    public void PlayGrabSound()
    {
        upSfxSource.PlayOneShot(soundEffects[7]);
        upSfxSource.volume = 0.3f;
    }
    public void PlayThrowSound()
    {
        upSfxSource.PlayOneShot(soundEffects[8]);
        upSfxSource.volume = 0.3f;
    }
    public void PlayPushSound()
    {
        upSfxSource.PlayOneShot(soundEffects[9]);
        upSfxSource.volume = 0.3f;
    }
    public void PlayDashSound()
    {
        upSfxSource.PlayOneShot(soundEffects[10]);
    }
    public void PlayRespawnSound() 
    {
        upSfxSource.PlayOneShot(soundEffects[11]);
        upSfxSource.volume = 0.25f;
    }
    public void PlayAddedItemSound()
    {
        upSfxSource.PlayOneShot(soundEffects[12]);
        upSfxSource.volume = 0.3f;
    }
    public void PlayCompletedItemSound()
    {
        upSfxSource.PlayOneShot(soundEffects[13]);
        upSfxSource.volume = 0.2f;
    }
    public void PlayPlayerDeathSound()
    {
        upSfxSource.PlayOneShot(soundEffects[14]);
        upSfxSource.volume = 0.5f;
    }
}
