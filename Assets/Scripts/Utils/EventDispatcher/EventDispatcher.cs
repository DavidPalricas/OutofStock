
using System.Collections.Generic;
using UnityEngine;

public class EventDispatcher : IEventDispatcher
{
    private readonly Dictionary<string, List<IEventListener>> listeners = new ();

    private static EventDispatcher _instance = null;

    public static EventDispatcher GetInstance()
    {
        _instance ??= new EventDispatcher();

        return _instance;
    }

    //
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

 
    public void RemoveListener(string eventType, IEventListener listener)
    {
        if (listeners.ContainsKey(eventType))
        {
            listeners[eventType].Remove(listener);
        }
    }


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
