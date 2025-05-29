
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    [SerializeField]
    private AudioSource musicSource, SFXSource;

    [Header("---------- Audio Clips Game -------------")]
    public AudioClip taskDoneSFX, paymentSFX;

    [Header("---------- Audio Clips Game -------------")]
    public AudioClip customerAttackedSFX, thiefAlertSFX, karenDeafeatSFX;

    [Header("---------- Music -------------")]
    public AudioClip mainMusic;

    public List<AudioClip> karenComplainingSFX;

    /// <summary>
    /// The Awake method is called when the script instance is being loaded (Unity Callback).
    /// For now it sets to not be destroyed on load.
    /// </summary>
    private void Awake()
    {
        PlayMusic(mainMusic);
        DontDestroyOnLoad(gameObject);
    }

  
    // M�todo para tocar a m�sica (se necess�rio)
    private void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.volume = 0.5f;
        musicSource.Play();
    }

    public void HandlePlayStopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Pause();
        }
        else if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    // M�todo para tocar o efeito sonoro
    public void PlaySFX(AudioClip clip)
    {
        if (clip == customerAttackedSFX)
        {
            SFXSource.pitch = Utils.RandomFloat(1.0f, 1.5f);
        }

        SFXSource.PlayOneShot(clip);
    }
}
