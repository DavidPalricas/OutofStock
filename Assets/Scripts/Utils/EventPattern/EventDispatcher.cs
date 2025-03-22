
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The EventDispatcher class is a singlethon responsible for handling the logic between event listeners and event dispatchers.
/// Like adding/removing listeners, dispatching events and removing event dispatchers.
/// </summary>
public class EventDispatcher 
{
    /// <summary>
    /// The listeners attribute is used to store the listeners of the events based on the event type.
    /// </summary>
    private readonly Dictionary<string, List<IEventListener>> listeners = new ();

    /// <summary>
    /// The _instance attribute is used to store the singleton instance of the EventDispatcher class.
    /// </summary>
    private static EventDispatcher _instance = null;

    /// <summary>
    /// The GetInstance method is used to get the singleton instance of the EventDispatcher class.
    /// </summary>
    /// <returns>The singlethon instance of this class</returns>
    public static EventDispatcher GetInstance()
    {
        _instance ??= new EventDispatcher();

        return _instance;
    }

    /// <summary>
    /// The AddListener method is used to add a listener to the event dispatcher.
    /// </summary>
    /// <remarks>
    ///  If the event type does not exist, a new list of listeners is created.
    ///  Otherwise, the listener is added to the list of listeners of the event type.
    /// </remarks>
    /// <param name="eventType">Type of the event.</param>
    /// <param name="listener">The listener to be added.</param>
    public void AddListener(string eventType, IEventListener listener)
    {
        if (!listeners.ContainsKey(eventType))
        {
            listeners[eventType] = new List<IEventListener>();
        }

        if (!listeners[eventType].Contains(listener))
        {
            listeners[eventType].Add(listener);
        }
    }

    /// <summary>
    /// The RemoveListener method is used to remove a listener from the event dispatcher.
    /// </summary>
    /// <param name="eventType">Type of the event.</param>
    /// <param name="listener">The listener to be removed.</param>
    public void RemoveListener(string eventType, IEventListener listener)
    {
        if (listeners.ContainsKey(eventType))
        {
            listeners[eventType].Remove(listener);
        }
    }

    /// <summary>
    /// The DispatchEvent method is used to dispatch an event to the listeners.
    /// </summary>
    /// <remarks>
    /// The event is only dispatched if there are listeners for the event type.
    /// It iterates over the listeners of the event type and calls the OnEvent method of each listener.
    /// to dispatch the event.
    /// </remarks>
    /// <param name="eventType">Type of the event.</param>
    /// <param name="target">The target's object which the event was triggerd.</param>
    public void DispatchEvent(string eventType, GameObject target)
    {
        if (listeners.ContainsKey(eventType))
        {
            foreach (var listener in listeners[eventType])
            {
                listener.OnEvent(eventType, target);
            }
        }
    }

    /// <summary>
    /// The RemoveEventDispatched method is used to remove a listener from the event dispatcher.
    /// It iterates over the listeners of the event type and removes the listener that has the same target.
    /// </summary>
    /// <param name="eventType">Type of the event.</param>
    /// <param name="target">The target's object which the event was triggerd.</param>
    public void RemoveEventDispatched(string eventType, GameObject target)
    {
        if (listeners.ContainsKey(eventType))
        {    
            foreach (var eventDispatched in listeners[eventType])
            {
                if (((MonoBehaviour)eventDispatched).gameObject == target)
                {    
                    listeners[eventType].Remove(eventDispatched);
                
                }
            }
        }
    }
}
