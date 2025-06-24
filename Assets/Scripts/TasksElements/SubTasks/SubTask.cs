using UnityEngine;

/// <summary>
/// The SubTask class is responsible for representing a subtask in the game.
/// It implements the ISubject interface to notify its observers (task) when the subtask is completed.
/// </summary>
public class Subtask : MonoBehaviour, ISubject
{   /// <summary>
    /// The subtaskIconPrefab property is responsible for storing the prefab of the icon tho show where the subtask is located.
    /// </summary>
    [SerializeField]
    private GameObject subtaskIconPrefab;

    /// <summary>
    /// The observers property is responsible for storing the observers of the subTask.
    /// For now, there is only one observer (the task where the subtask is associated).
    /// </summary>
    private IObserver[] observers;

    /// <summary>
    /// The subtaskIcon property stores a reference to the subtask icon game object.
    /// </summary>
    private GameObject subtaskIcon = null;

    /// <summary>
    /// The MoveIconToSubtask method is responsible for instanting a subtask icon in front of the subtask game object.
    /// </summary>
    public void MoveIconToSubtask()
    {
        const float ICON_DISTANCE = 1.5f;

        Vector3 subTaskIconPos = gameObject.transform.position + new Vector3(0f, ICON_DISTANCE, 0f);

        subtaskIcon = Instantiate(subtaskIconPrefab, subTaskIconPos, Quaternion.identity);
    }

    /// <summary>
    /// The SubtaskCompleted method is responsible for handling the completion of the subtask.
    /// When a subtask is completed their observers are notified and removed, its icon is destroyed and its script is disabled.
    /// </summary>
    protected void SubtaskCompleted()
    {
        NotifyObservers();
        RemoveObservers();

        Destroy(subtaskIcon);

        enabled = false;
    }

    /// <summary>
    /// The AddObserver method is responsible for adding observers to the subtask (ISubject interface method).
    /// </summary>
    /// <param name="observers">The observers (only one the task where the subtask is associated).</param>
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
            observer.OnNotify(data);
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
