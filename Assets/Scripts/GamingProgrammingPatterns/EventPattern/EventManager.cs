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
    /// The CustomerAttacked and TaskCompleted atributes, are events used to notify when a customer is attacked or a task is completed
    /// and when a customer is sent to his home, respectively.
    /// </summary>
    public event Action TaskCompleted, CustomerSent;

    /// <summary>
    /// The LastTaskCompletedNumber attribute is used to store the number of the last task completed.
    /// </summary>
    /// <value>
    ///  The number of the last task completed, or int.MaxValue if there is no task completed yet.
    /// </value>
    public int LastTaskCompletedNumber { get; private set; } = int.MaxValue;


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
    /// The OnTaskCompleted method is used to invoke the TaskCompleted event and updating the LastTaskCompletedNumber attribute.
    /// </summary>
    /// <param name="taskNumber">The task completed number.</param>
    public void OnTaskCompleted(int taskNumber)
    {
        LastTaskCompletedNumber = taskNumber;
        TaskCompleted?.Invoke();
    }

    /// <summary>
    /// The OnCustomerSent method is used to invoke the CustomerSent event.
    /// </summary>
    public void OnCustomerSent()
    {
        CustomerSent?.Invoke();
    }
}
