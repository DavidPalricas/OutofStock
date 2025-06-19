using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;



public class AudioManager : MonoBehaviour
{

public AudioClip taskDoneSFX, paymentSFX;
public AudioClip thiefAlertSFX, karenDeafeatSFX;
public List<AudioClip> karenComplainingSFX;
    public AudioClip mainMusic;



    [EventRef]
    public EventReference customerAttackedEvent;

public void PlayCustomerAttackedSFX(Vector3 position)
{
    RuntimeManager.PlayOneShot(customerAttackedEvent, position);
}


    public void PlaySFX(AudioClip clip)
    {
        Debug.LogWarning("PlaySFX called, but AudioClip-based playback is deprecated. Switch to FMOD events.");
    }


    [Header("---------- FMOD Events ----------")]
    [EventRef]
    public EventReference punchHitEvent; // This is your test sound (e.g. punch)

    [Header("---------- Music ----------")]
    [EventRef]
    public EventReference mainMusicEvent;

    private EventInstance musicInstance;

    private void Awake()
    {
        // Play background music on load
        musicInstance = RuntimeManager.CreateInstance(mainMusicEvent);
        musicInstance.start();
        musicInstance.release(); // Let FMOD manage the memory cleanup

        DontDestroyOnLoad(gameObject);
    }

    public void HandlePlayStopMusic()
    {
        PLAYBACK_STATE playbackState;
        musicInstance.getPlaybackState(out playbackState);

        if (playbackState == PLAYBACK_STATE.PLAYING)
        {
            musicInstance.setPaused(true);
        }
        else
        {
            musicInstance.setPaused(false);
        }
    }

    public void PlayPunchHitSFX(Vector3 position)
    {
        RuntimeManager.PlayOneShot(punchHitEvent, position);
    }
}
