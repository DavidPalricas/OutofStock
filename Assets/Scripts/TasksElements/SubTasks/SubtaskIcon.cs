using UnityEngine;

/// <summary>
/// The SubtaskIcon class is responsible for handling the icon of the subtask logic.
/// </summary>
public class SubtaskIcon : MonoBehaviour
{
    /// <summary>
    /// The shoIcon property is responsible for storing a reference to the MeshRenderer component to show or hide the subtask icon.
    /// </summary>
    private MeshRenderer showIcon;

    /// <summary>
    /// The Awake method is called when the script instance is being loaded. (Unity Callback).
    /// In this method, we initialize the showIcon property;
    /// </summary>
    private void Awake()
    {
        showIcon = GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// The Update method is called every frame (Unity Callback).
    /// In this metod, a small animation is applied to the icon, rotating it around the Y axis.
    /// </summary>
    private void Update()
    {
        transform.Rotate(0, 50 * Time.deltaTime, 0);
    }

    /// <summary>
    /// The OnTriggerEnter method is called when another game object enters the trigger collider attached to this object. (Unity Callback).
    /// In this method we are checking if the collider belongs to the player, if it is, the subtask icon is hidden.
    /// </summary>
    /// <param name="collider">The collider of a game object.</param>
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
           showIcon.enabled = false;
        }
    }

    /// <summary>
    /// The OnTriggerExit method is called when another game object exits the trigger collider attached to this object. (Unity Callback).
    /// In this method we are checking if the collider belongs to the player, if it is, the subtask icon is shown.
    /// </summary>
    /// <param name="collider">The collider of a game object.</param>
    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            showIcon.enabled = true;
        }
    }
}
