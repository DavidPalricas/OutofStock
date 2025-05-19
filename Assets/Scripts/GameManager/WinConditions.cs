using UnityEngine;

/// <summary>
/// The WindConditions class is responsible for checking if the player has won its shift in the game.
/// It implements the IEventListener interface to listen to events dispatched by the EventManager (CustomerAttacked and TaskCompleted events).
/// </summary>
public class WinConditions : MonoBehaviour, IEventListener
{
    /// <summary>
    /// The customersHitted, tasksCompleted, maxCustomersHitted and tasksToComplete attributes are used to store the number of customers hitted,
    /// the number of tasks completed, the maximum number of customers hitted and the number of tasks to complete, respectively.
    /// </summary>
    private int customersHitted = 0, tasksCompleted = 0, maxCustomersHitted, tasksToComplete;

    /// <summary>
    /// The Awake Method is called when the script instance is being loaded (Unity Callback).
    /// In this method, the maxCustomersHitted and tasksToComplete attributes are initialized with the values stored in PlayerPrefs.
    /// </summary>
    private void Awake()
    {
        maxCustomersHitted = PlayerPrefs.GetInt("CustomersToSend");
        tasksToComplete = PlayerPrefs.GetInt("NumberOfTasks");
    }

    /// <summary>
    /// The Start method is called on the frame when a script is enabled just before any of the Update methods are called the first time (Unity Callback).
    /// In this method, the ListenToEvents method is called to subscribe to the events of the EventManager.
    /// </summary>
    private void Start()
    {   
      ListenToEvents();
    }

    /// <summary>
    /// The CustomerHitted method is called when the "CustomerHitted" event is dispatched 
    /// and increments the customersHitted attribute.
    /// </summary>
    private void CustomerHitted()
    {
        customersHitted++;
    }

    /// <summary>
    /// The TaskCompleted method is called when the "TaskCompleted" event is dispatched and increments the tasksCompleted attribute.
    /// </summary>
    private void TaskCompleted()
    {
        tasksCompleted++;
    }

    /// <summary>
    /// The PlayerWon method checks if the player has won the level (its shift) by checking if the number of customers hitted is greater than or equal to
    /// the number of customers it needs to send to its uncle market and if the number of tasks completed is greater than or equal to the number of tasks that it needs to complete.
    /// </summary>
    /// <returns>
    ///  <c>true</c> if the player won; otherwise, <c>false</c>.
    /// </returns>
    public bool PlayerWon()
    {
        return customersHitted >= maxCustomersHitted && tasksCompleted >= tasksToComplete;
    }

    /// <summary>
    /// The ListenToEvents method is used to listen to one or more events.
    /// It subscribes to the "CustomerAttacked" and "TaskCompleted" events of the EventManager.
    /// </summary>
    public void ListenToEvents()
    {
        EventManager eventManager = EventManager.GetInstance();

        eventManager.CustomerAttacked += CustomerHitted;
        eventManager.TaskCompleted += TaskCompleted;
    }
}

