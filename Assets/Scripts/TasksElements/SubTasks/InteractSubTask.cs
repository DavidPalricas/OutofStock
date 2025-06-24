using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// The InteractSubtask class is responsible for representing a subtask that requires the player to interact with an object.
/// </summary>
public class InteractSubtask : Subtask
{
    /// <summary>
    /// The subtaskInput property is responsible for storing the input action reference that the player needs to use to interact with the object.
    /// </summary>
    [SerializeField]
    private InputActionReference subtaskInput;


    [SerializeField]
    private GameObject sliderContainer;

    [SerializeField]
    private Slider slider;

    /// <summary>
    /// The crosshair property is responsible for storing a reference to the player's crosshair UI element.
    /// </summary>
    [SerializeField]
    private RectTransform crosshair;

    /// <summary>
    /// The subTaskTime and currentSubtaskTime properties are responsible for storing
    /// the time of the subtask and the current time of the subtask respectively.
    /// </summary>
    private float subTaskTime, currentSubtaskTime;

    /// <summary>
    /// The CanBeVanish property is responsible for storing a value that indicates whether the subtask game object 
    /// can be desactivated when the subtask is completed.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance can be vanish; otherwise, <c>false</c>.
    /// </value>
    public bool CanBeVanish { get; set; } = false;

    /// <summary>
    /// The CanBeReseted property is responsible for storing a value that indicates whether the subtask time can be reseted,
    /// if the player stops doing the subtask.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance can be reseted; otherwise, <c>false</c>.
    /// </value>
    public bool CanBeReseted { get; set; } = false;

    /// <summary>
    /// The OnEnable method is called when the script is enabled. (Unity Callback).
    /// In this method, we set the subtask time to a random value between 1 and 5 seconds and initialize the current subtask time;
    /// </summary>
    private void OnEnable()
    {
        subTaskTime = 5f;
        currentSubtaskTime = 0f;
        slider.value = 0f;
    }

    /// <summary>
    /// The Update method is called every frame (Unity Callback).
    /// </summary>
    /// <remarks>
    /// In this method whe check if the player is doing the subtask, if it is the case the subtask progress is updated, 
    /// otherwise, checks if the subtask can be reseted, if it is the case the current subtask time is reseted.
    /// </remarks>
    private void Update()
    {
        if (IsPlayerDoingSubtask())
        {
            sliderContainer.SetActive(true);
            UpdateSubTaskProgress();
        }
        else
        {
            if (CanBeReseted)
            {
                currentSubtaskTime = 0f;
                slider.value = 0f;
            }

            Debug.Log("Player is not doing the subtask");

            sliderContainer.SetActive(false);
        }
     
    }

    /// <summary>
    /// The IsPlayerDoingSubtask method is responsible for checking if the player is doing the subtask.
    /// </summary>
    /// <remarks>
    /// To check if the player is pressing the correct input action, if not the method stops imediately (returns false).
    /// Otherwise a raycast is casted from the player's crosshair to check if the player is looking at the subtask game object.
    /// </remarks>
    /// <returns>
    ///   <c>true</c> if is player doing sub task; otherwise, <c>false</c>.
    /// </returns>
    private bool IsPlayerDoingSubtask()
    {
        if (!subtaskInput.action.IsPressed())
        {
            return false;
        }

        Ray ray = Utils.CastRayFromUI(crosshair);

        const float RAYCASTDISTANCE = 2f;

        return Physics.Raycast(ray, out RaycastHit hit, RAYCASTDISTANCE, LayerMask.GetMask("Default")) && hit.collider.gameObject == gameObject;
    }

    /// <summary>
    /// The UpdateSubTaskProgress method is responsible for updating the subtask progress.
    /// </summary>
    /// <remarks>
    /// If the subtask's time checks to its max value (subtask completed), 
    /// the SubtaskCompleted method is called to handle its completions and the script is disabled, it also
    /// checks if the game object can be desactivated, if it is the case the game object is desactivated.
    /// </remarks>
    private void UpdateSubTaskProgress()
    {
        currentSubtaskTime += Time.deltaTime;

        // Because the slider value is between 0 and 10;
        slider.value = slider.value = Mathf.Round((currentSubtaskTime / subTaskTime) * 10f);

        Debug.Log($"Current Subtask Time: {currentSubtaskTime / subTaskTime}");

        if (currentSubtaskTime >= subTaskTime)
        {
            SubtaskCompleted();

            sliderContainer.SetActive(false);

            // Checks if the game object can be desactivated, if it is the case the game object is desactivated.
            if (CanBeVanish)
            {
                gameObject.SetActive(false);
            }

            enabled = false;
        }
    }
}
