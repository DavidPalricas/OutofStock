using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;
using FMOD.Studio;
using NUnit.Framework;


/// <summary>
/// The PauseMenu class is responsible for managing the player activing or deactivating the pause menu.
/// </summary>
public class PauseMenu : MonoBehaviour
{


    /// <summary>
    /// The pauseActionReference attribute is used to reference the pause action from the InputActionAsset.
    /// </summary>
    [SerializeField]
    private InputActionReference pauseActionReference;

    /// <summary>
    /// The pauseMenu attribute is used to reference the pause menu GameObject.
    /// </summary>
    [SerializeField]
    private GameObject pauseMenu;

    /// <summary>
    /// The isPaused atribute is used to check if the game is paused or not.
    /// </summary>
    private bool isPaused = false;

    /// <summary>
    /// The firstPersonController attribute is used to reference the FirstPersonController (Player Controller) component.
    /// </summary>
    private FirstPersonController firstPersonController;


    /// <summary>
    ///The Awake Method is called when the script instance is being loaded (Unity callback).
    ///In this method the firstPersonController atributed is initialized.
    /// </summary>
    private void Awake()
    {
        firstPersonController = GetComponent<FirstPersonController>();
    }

    /// <summary>
    /// The Update method is called once per frame (Unity callback).
    /// In this method the pauseActionReference is checked to see if it was triggered and if so, the PauseResume method is called, to handle the 
    /// �
    /// </summary>
    private void Update()
    {
        if (pauseActionReference.action.triggered)
        {
            PauseResume();
        }
    }

    /// <summary>
    /// The PauseResume method is responsible for pausing or resuming the game, and activating or deactivating the cursor visibility and lock state.
    /// </summary>

    // Note : This method is public because is called from a button in the UI Pause Menu.




    public void PauseResume()
    {
        // Tries to pause the game but the game is already paused.
        if (Time.timeScale == 0 && !isPaused)
        {
            return;
        }


        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
        HandlingInput(isPaused);

        Utils.IsGamePaused = isPaused;

        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;

        AudioManager audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        if (isPaused)
        {
            audioManager.ActivatePauseSnapshot();
        }
        else
        {
            audioManager.DeactivatePauseSnapshot();
        }
    }

    /// <summary>
    /// The HandlingInput method is responsible for handling the input.
    /// This method activates or deactivates the player input depending on the game pause status, if the game is paused the player input is disabled otherwise it is enabled.
    /// The only input action that is not disabled when the game is paused is the pause/unpause input.
    /// </summary>
    private void HandlingInput(bool isPaused)
    {
        firstPersonController.enabled = !isPaused;
    }
    
    
}
