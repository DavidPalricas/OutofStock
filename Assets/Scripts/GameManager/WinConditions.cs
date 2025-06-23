using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private int customersSent = 0, tasksCompleted = 0, maxCustomersSent, tasksToComplete;

    /// <summary>
    /// The customerHittedTextsUI attribute is used to store a reference to the TextMeshProUGUI component that displays the number of customers hitted in the UI.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI customerSentTextsUI, customersToSentUI;

    /// <summary>
    /// The customerHittedPanel attribute is used to store a reference to the GameObject that displays the number of customers hitted in the UI.
    /// </summary>
    [SerializeField]
    private GameObject customerSentPanel;

    /// <summary>
    /// The Awake Method is called when the script instance is being loaded (Unity Callback).
    /// In this method, the maxCustomersHitted and tasksToComplete attributes are initialized with the values stored in PlayerPrefs.
    /// </summary>
    private void Awake()
    {
        maxCustomersSent = PlayerPrefs.GetInt("CustomersToSend");
        tasksToComplete = PlayerPrefs.GetInt("NumberOfTasks");

        customerSentTextsUI.text = $"0";
        customersToSentUI.text = $"{maxCustomersSent}";
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
    /// <remarks>
    /// This method also updates the number of customers hitted in the UI and checks if the player has reached the maximum number of customers hitted.
    /// If the customer hitted reached the maximum number of customers hitted, it waits 3 seconds and hides the UI element, after that doesn't update the UI anymore.
    /// </remarks>
    private void CustomerSent()
    {   
        if(customersSent >= maxCustomersSent)
        {
            return;
        }

        customersSent++;
        customerSentTextsUI.text = customersSent.ToString();
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
        return customersSent >= maxCustomersSent && tasksCompleted >= tasksToComplete;
    }

    /// <summary>
    /// The ListenToEvents method is used to listen to one or more events.
    /// It subscribes to the "CustomerAttacked" and "TaskCompleted" events of the EventManager.
    /// </summary>
    public void ListenToEvents()
    {
        EventManager eventManager = EventManager.GetInstance();

        eventManager.CustomerSent += CustomerSent;
        eventManager.TaskCompleted += TaskCompleted;
    }
}

