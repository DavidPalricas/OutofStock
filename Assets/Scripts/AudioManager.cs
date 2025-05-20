
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    [SerializeField]
    private AudioSource musicSource, SFXSource;

    [Header("---------- Audio Clips Game -------------")]
    public AudioClip taskDoneSFX, paymentSFX;

    [Header("---------- Audio Clips Game -------------")]
    public AudioClip customerAttackedSFX, thiefAlertSFX, karenComplainingSFX, karenDeafeatSFX;

    // private float musicPauseTime = 0f;

    /// <summary>
    /// The Awake method is called when the script instance is being loaded (Unity Callback).
    /// For now it sets to not be destroyed on load.
    /// </summary>
    private void Awake()
    {
        // PlayMusic(ambient);
        DontDestroyOnLoad(gameObject);
    }

    // Método para tocar o efeito sonoro
    public void PlaySFX(AudioClip clip)
    {
        /*  Attack Sounds
        if (clip == playerAttack || clip == spiderAttack || clip == skeletonAttack || clip == bossAttack)
        {
            SFXSource.pitch = Utils.RandomFloat(1.0f, 1.5f);
        }
        */

        SFXSource.PlayOneShot(clip);
    }

    // Método para tocar a música (se necessário)
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.volume = 0.5f;
        musicSource.Play();
    }

    public void HandlePlayStopMusic(AudioClip clip)
    {
        if (musicSource.isPlaying && musicSource.clip == clip)
        {
            musicPauseTime = musicSource.time;
            musicSource.Pause();
        }
        else if (!musicSource.isPlaying && musicSource.clip == clip)
        {
            musicSource.clip = clip;
            musicPauseTime = 0f;
            musicSource.Play();
        }
    }
}
