using UnityEngine;

/// <summary>
/// The Task class is responsible for representing a task in the game.
/// It implements the IObserver interface to be notified when a subtask (subject) is completed  and
/// the IEventDispatcher interface to notify the to event manager when the task is completed.
/// </summary>
public class Task : MonoBehaviour, IObserver, IEventDispatcher
{
    /// <summary>
    /// The observers property is responsible for storing a reference to the observers of the task.
    /// In this case, there is only one observer, which is the task manager.
    /// </summary>
    protected IObserver[] observers;

    /// <summary>
    /// The completedSubtasks property is responsible for storing the number of completed subtasks.
    /// </summary>
    protected int completedSubtasks = 0;

    /// <summary>
    /// The subtasks property is responsible for storing a reference to the subtasks of the task.
    /// </summary>
    protected GameObject[] subtasks;

    /// <summary>
    /// The Number property is responsible for storing the number of the task.
    /// </summary>
    /// <value>
    /// The number of the task.
    /// </value>
    public int Number { get; set; }

    /// <summary>
    /// The TaskCompleted method is responsible for handling the logic when the task is completed.
    /// It dispatches the task completed event by calling the DispatchEvents method, and resets the completed Subtasks.
    /// </summary>
    protected void TaskCompleted()
    {
        DispatchEvents();
        completedSubtasks = 0;
        enabled = false;
    }

    /// <summary>
    /// The OnNotify (IObserver method) method is responsible for updating the observer (this game object), when a subject notifies it.
    /// In this method, the number of completed subtasks is incremented.
    /// </summary>
    /// <param name="data">Any argument to be sent to the observer, in this case no argument is specified (null)</param>
    public void OnNotify(object data = null)
    {
        completedSubtasks++;
    }

    /// <summary>
    /// The DispatchEvents method is used to dispatch one or more events.
    /// It notifies the event manager when a task is completed.
    /// </summary>
    public void DispatchEvents()
    {
        EventManager.GetInstance().OnTaskCompleted(Number);
    }
}
