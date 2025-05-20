using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{

    private bool isPaused = false;

    [SerializeField]
    private InputActionReference pauseActionReference;

    [SerializeField]
    private GameObject pauseMenu;

    private FirstPersonController firstPersonController;

    private void Awake()
    {
        firstPersonController = GetComponent<FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (pauseActionReference.action.triggered)
        {

            PauseResume();

        }
        
    }

    private void PauseResume()
    {

        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
        HandlingInput(isPaused);
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
