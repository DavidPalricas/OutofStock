using UnityEngine;
using UnityEngine.UI;



public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    private Slider volumeSlider, sensitivitySlider, fovSlider;
    private AudioManager audioManager;

    private void Awake()
    {
        fovSlider.value = PlayerPrefs.GetFloat("FOV", 90f);
        sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", 5f);
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 7f);

        audioManager = FindFirstObjectByType<AudioManager>();
    }

    public void ChangeFOV(float fov)
    {
        PlayerPrefs.SetFloat("FOV", fov);

        Debug.Log("FOV changed to: " + fov);
    }

    public void ChangeSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);

        Debug.Log("Sensitivity changed to: " + sensitivity);
    }

    public void ChangeVolume(float volume)
    {
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();

        Debug.Log("Volume changed to: " + volume);

        // 🔊 Update FMOD VCA volume
        audioManager?.SetVolume(volume);
    }

}
