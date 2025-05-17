using System;
using UnityEngine;

/// <summary>
/// The EventManager class is a singlethon responsible for managing events in the game.
/// </summary>
public class EventManager
{
    /// <summary>
    /// The instance attribute is used to store the instance of the EventManager class.
    /// </summary>
    private static EventManager instance = null;

    /// <summary>
    /// The CustomerHitted and TaskCompleted atributes, are events used to notify when a customer is hitted or a task is completed, respectively.
    /// </summary>
    public event Action CustomerHitted, TaskCompleted;

    /// <summary>
    /// The CustomerAttacked attribute is used to store a reference to the customer that was attacked.
    /// </summary>
    /// <value>
    /// The GameObject of the customer that was attacked.
    /// </value>
    public GameObject CustomerAttacked { get; set; } = null;

    /// <summary>
    /// The GetInstance method is used to get the instance of the EventManager class.
    /// It creates a new instance if it doesn't exist yet.
    /// </summary>
    /// <returns>The unique instance of this class</returns>
    public static EventManager GetInstance()
    {
        return instance ??= new EventManager();
    }

    /// <summary>
    /// The OnCustomerHitted method is used to invoke the CustomerHitted event and updating the CustomerAttacked attribute.
    /// </summary>
    /// <param name="customerHitted">The hitted customer's game obejct.</param>
    public void OnCustomerHitted(GameObject customerHitted)
    {
        CustomerAttacked = customerHitted;
        CustomerHitted?.Invoke();
    }

    /// <summary>
    /// The OnTaskCompleted method is used to invoke the TaskCompleted event.
    /// </summary>
    public void OnTaskCompleted()
    {   
        TaskCompleted?.Invoke();
    }
}
