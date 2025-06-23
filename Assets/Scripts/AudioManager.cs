using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;



public class AudioManager : MonoBehaviour
{

    public AudioClip taskDoneSFX;
    public AudioClip thiefAlertSFX, karenDeafeatSFX;




    [Header("---------- Snapshots ----------")]

    public EventReference pauseSnapshot;


    [Header("---------- Events ----------")]

    public EventReference customerAttackedEvent; //customer is hit with item
    public EventReference paymentSFXEvent; //customer pays before leaving
    public EventReference KarenComplaintEvent; //Karen complaints



    [Header("---------- Music ----------")]

    public EventReference mainMusicEvent;

    private EventInstance musicInstance;

    private void Awake()
    {
        // Play background music on load
        musicInstance = RuntimeManager.CreateInstance(mainMusicEvent);
        musicInstance.start();
        // No release — keep it around

        DontDestroyOnLoad(gameObject);
    }

        public void PlaySFX(AudioClip clip)
    {
        Debug.LogWarning("PlaySFX called, but AudioClip-based playback is deprecated. Switch to FMOD events.");
    }


    private EventInstance pauseSnapshotInstance;

    public void ActivatePauseSnapshot()
    {
        pauseSnapshotInstance = RuntimeManager.CreateInstance(pauseSnapshot);
        pauseSnapshotInstance.start();
    }

    public void DeactivatePauseSnapshot()
    {
        pauseSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        pauseSnapshotInstance.release();
    }


    public void PlayCustomerAttackedSFX(Vector3 position)
    {
        RuntimeManager.PlayOneShot(customerAttackedEvent, position);
        Debug.DrawRay(position, Vector3.up * 2f, Color.red, 2f);
    }



    public void PlayPaymentSFX(Vector3 position)
    {
        RuntimeManager.PlayOneShot(paymentSFXEvent, position);
    }
    

    public void PlayKarenComplaint(Vector3 position)
{
    RuntimeManager.PlayOneShot(KarenComplaintEvent, position);
}



}
