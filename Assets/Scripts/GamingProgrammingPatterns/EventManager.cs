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
    /// The CustomerAttacked and TaskCompleted atributes, are events used to notify when a customer is attacked or a task is completed, respectively.
    /// </summary>
    public event Action CustomerAttacked, TaskCompleted;

    /// <summary>
    /// The LastCustomerAttacked attribute is used to store a reference to the last customer that was attacked.
    /// </summary>
    /// <value>
    /// The GameObject of the customer that was attacked.
    /// </value>
    public GameObject LastCustomerAttacked { get; set; } = null;

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
    /// The OnCustomerAttacked method is used to invoke the CustomerHitted event and updating the CustomerAttacked attribute.
    /// </summary>
    /// <param name="customerAttacked">The attacked customer's game obejct.</param>
    public void OnCustomerAttacked(GameObject customerAttacked)
    {
        LastCustomerAttacked = customerAttacked;
        CustomerAttacked?.Invoke();
    }

    /// <summary>
    /// The OnTaskCompleted method is used to invoke the TaskCompleted event.
    /// </summary>
    public void OnTaskCompleted()
    {   
        TaskCompleted?.Invoke();
    }
}
