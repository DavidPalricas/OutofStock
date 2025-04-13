using UnityEngine;

/// <summary>
/// The GrabSubtask class is responsible for representing a subtask that requires the player to grab an item.
/// </summary>
public class GrabSubtask : Subtask
{
    /// <summary>
    /// The item property is responsible for storing the item that the player needs to grab to complete the subtask.
    /// </summary>
    [SerializeField]
    private Item item;

    /// <summary>
    /// he Update method is called every frame (Unity Callback).
    /// In this method, whe check if the item has been grabbed by the player, if it is the sub task is completed and the script is disabled.
    /// </summary>
    private void Update()
    {
        if (item.Grabbed)
        {
           SubtaskCompleted();
            enabled = false;
        }
    }
}
