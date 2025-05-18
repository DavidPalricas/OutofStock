using UnityEngine;

/// <summary>
/// The WindConditions class is responsible for checking if the player has won its shift in the game.
/// </summary>
public class WinConditions : MonoBehaviour
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
    /// In this method this class is added as a listener to the "CustomerAttacked" and "TaskCompleted" events of the EventManager.
    /// </summary>
    private void Start()
    {   
        EventManager eventManager = EventManager.GetInstance();

        eventManager.CustomerAttacked += CustomerHitted;
        eventManager.TaskCompleted += TaskCompleted;
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

    public bool PlayerWon()
    {
        return customersHitted >= maxCustomersHitted && tasksCompleted >= tasksToComplete;
    }
}

