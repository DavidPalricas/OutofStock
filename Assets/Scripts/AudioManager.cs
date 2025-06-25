using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine.SceneManagement;



public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;
    private VCA masterVCA;


    [Header("---------- Snapshots ----------")]

    public EventReference pauseSnapshot;


    [Header("---------- Events ----------")]


    public EventReference doorchimeEvent; //customer enters the store
    public EventReference paymentSFXEvent; //customer pays before leaving

    public EventReference customerAttackedEvent; //customer is hit with item
    public EventReference KarenComplaintEvent; //Karen complaints
    public EventReference karendefeatEvent; //Karen is defeated

    public EventReference alarmEvent; //thief alarm
    private EventInstance alarmInstance;

    public EventReference throwEvent; //player throws item
    public EventReference knockEvent; //player is knocked down by karen
    public EventReference taskdoneEvent; //task is completed
    public EventReference winEvent; //player wins level
    public EventReference loseEvent; //player loses level




    [Header("---------- Music / Ambience ----------")]

    public EventReference supermarketAmbience;
    private EventInstance ambienceInstance;

    public EventReference mainMusicEvent;
    private EventInstance musicInstance;

    public EventReference mainMenuMusic;


    private string currentScene;


    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject); // Kill duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Subscribe to scene load callback
        SceneManager.sceneLoaded += OnSceneLoaded;


        // Get reference to the VCA exposed as "MasterVolume"
        masterVCA = RuntimeManager.GetVCA("vca:/MasterVolume");

        // Read saved volume from PlayerPrefs, defaulting to 7
        float savedVolume = PlayerPrefs.GetFloat("Volume", 7f);

        // Convert 0–10 scale to FMOD's 0.0–1.0 scale
        float normalizedVolume = Mathf.InverseLerp(0f, 10f, savedVolume);

        // Apply volume to FMOD
        masterVCA.setVolume(normalizedVolume);


        // Play background music on load
        musicInstance = RuntimeManager.CreateInstance(mainMusicEvent);
        musicInstance.start();


        ambienceInstance = RuntimeManager.CreateInstance(supermarketAmbience);
        ambienceInstance.start();

    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.name;

        // Stop current music and ambience (if playing)
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicInstance.release();

        ambienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        ambienceInstance.release();

        if (currentScene.ToLower().Contains("menu"))
        {
            musicInstance = RuntimeManager.CreateInstance(mainMenuMusic);
            musicInstance.start();
        }
        else
        {
            musicInstance = RuntimeManager.CreateInstance(mainMusicEvent);
            musicInstance.start();

            ambienceInstance = RuntimeManager.CreateInstance(supermarketAmbience);
            ambienceInstance.start();
        }


        if (pauseSnapshotInstance.isValid())
        {
            pauseSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            pauseSnapshotInstance.release();
        }
    }

    public void SetVolume(float value)
    {
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();

        float normalized = Mathf.InverseLerp(0f, 10f, value);
        Debug.Log($"Setting FMOD VCA volume to: {normalized}");
        masterVCA.setVolume(normalized);
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


    public void PlayImpactSFX(Vector3 position, float materialParamValue)
    {
        Debug.Log("Playing impact sound with MaterialType param: " + materialParamValue);

        var instance = RuntimeManager.CreateInstance(customerAttackedEvent);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        instance.setParameterByName("MaterialType", materialParamValue);
        instance.start();
        instance.release();
    }




    public void PlayPaymentSFX(Vector3 position)
    {
        RuntimeManager.PlayOneShot(paymentSFXEvent, position);
    }


    public void PlayKarenComplaint(Vector3 position)
    {
        RuntimeManager.PlayOneShot(KarenComplaintEvent, position);
    }


    public void PlayThrowSound(Vector3 position)
    {
        RuntimeManager.PlayOneShot(throwEvent, position);
    }


    public void PlayAlarmSound(Vector3 position)
    {
        if (alarmInstance.isValid())
            return; // Already playing

        alarmInstance = RuntimeManager.CreateInstance(alarmEvent);
        alarmInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        alarmInstance.start();
    }

    public void StopAlarmSound()
    {
        if (alarmInstance.isValid())
        {
            alarmInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            alarmInstance.release();
        }
    }

    public void PlayChimeSound(Vector3 position)
    {
        RuntimeManager.PlayOneShot(doorchimeEvent, position);
    }

    public void PlayKarenDefeated(Vector3 position)
    {
        RuntimeManager.PlayOneShot(karendefeatEvent, position);
    }

    public void PlayPlayerKnockedOut(Vector3 position)
    {
        RuntimeManager.PlayOneShot(knockEvent, position);
    }

    public void PlayWin(Vector3 position)
    {
        RuntimeManager.PlayOneShot(winEvent, position);
    }

    public void PlayLose(Vector3 position)
    {
        RuntimeManager.PlayOneShot(loseEvent, position);
    }

        public void PlayTaskComplete(Vector3 position)
    {
        RuntimeManager.PlayOneShot(taskdoneEvent, position);
    }


}
