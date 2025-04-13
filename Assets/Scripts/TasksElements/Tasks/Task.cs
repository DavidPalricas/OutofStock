using UnityEngine;

/// <summary>
/// The Task class is responsible for representing a task in the game.
/// It implements the IObserver interface to be notified when a subtask (subject) is completed 
/// and implements the ISubject interface to notify its observers (task manager) when the task is completed.
/// </summary>
public class Task : MonoBehaviour, ISubject, IObserver
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
    /// It notifies the observers, resets the subtask count and disables its script.
    /// </summary>
    protected void TaskCompleted()
    {
        NotifyObservers();
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
    /// The AddObserver method is responsible for adding observers to the subtask (ISubject interface method).
    /// </summary>
    /// <param name="observers">The observers (only one the task manager).</param>
    public void AddObservers(IObserver[] observers)
    {
        this.observers = observers;
    }

    /// <summary>
    /// The NotifyObservers method is responsible for notifying the subtask observers (ISubject interface method).
    /// </summary>
    /// <param name="data">Any argument to be sent to the observer,in this case no argument is specified (null) .</param>
    public void NotifyObservers(object data = null)
    {
        foreach (IObserver observer in observers)
        {
            observer.OnNotify(Number);
        }
    }

    /// <summary>
    /// The RemoveObservers method is responsible for removing the observers from the subject (ISubject interface method).
    /// </summary>
    public void RemoveObservers()
    {
        observers = null;
    }
}
