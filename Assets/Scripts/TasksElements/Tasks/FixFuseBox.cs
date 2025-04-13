using UnityEngine;

/// <summary>
/// The FixFuseBox class is responsible for representing a task that requires the player to fix a fuse box.
/// </summary>
public class FixFuseBox : Task
{
    /// <summary>
    /// The fuseBox property is responsible for storing a reference to the fuse box game object that the player needs to fix.
    /// </summary>
    [SerializeField]
    private GameObject fuseBox;

    /// <summary>
    /// The OnEnable method is called when the script is enabled. (Unity Callback).
    /// In this method we are adding the subtask to fix the fuse box and initialize its properties.
    /// </summary>
    private void OnEnable()
    {
        subtasks = new GameObject[] { fuseBox };

        InteractSubtask subTask = subtasks[0].GetComponent<InteractSubtask>();

        subTask.enabled = true;
        subTask.AddObservers(new IObserver[] { this });
        subTask.MoveIconToSubtask();
        subTask.CanBeReseted = true;
    }

    /// <summary>
    /// The Update method is called every frame (Unity Callback).
    /// In this method we are checking if all subtasks are completed.
    /// If it is the TaskCompleted method is called to handle its completion and the script is disabled.
    /// </summary>
    private void Update()
    {
        if (completedSubtasks >= subtasks.Length)
        {
            TaskCompleted();
            enabled = false;
        }
    }
}
