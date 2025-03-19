using UnityEngine;

/// <summary>
/// The EventDispatcher interface  for dispatching events.
/// </summary>
public interface IEventDispatcher
{
    void DispatchEvent(string eventType, GameObject target);

    void RemoveEventDispatched(string eventType, GameObject target);
}

/// <summary>
/// The EventListener interface for listening to events.
/// </summary>
public interface IEventListener
{
    void OnEvent(string eventType, GameObject target);
}