using UnityEngine;

/// <summary>
/// The EventListener interface for listening to events.
/// </summary>
public interface IEventListener
{
    /// <summary>
    /// The OnEvent method is called when an event is dispatched.
    /// Its logic is implemented in the classes that implement this interface.
    /// </summary>
    /// <param name="eventType">Type of the event.</param>
    /// <param name="target">The target's game object.</param>
    void OnEvent(string eventType, GameObject target);
}