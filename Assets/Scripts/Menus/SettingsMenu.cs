using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    private Slider volumeSlider, sensitivitySlider, fovSlider;

    private void Awake()
    {
        fovSlider.value = PlayerPrefs.GetFloat("FOV", 90f);
        sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", 5f);
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 7f);
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

        Debug.Log("Volume changed to: " + volume);
    }
}
