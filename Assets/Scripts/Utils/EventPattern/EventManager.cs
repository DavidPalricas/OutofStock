using System;

public class EventManager
{
    private static EventManager instance = null;

    public event Action CustomerHitted;

    private EventManager()
    {
        instance ??= this;
    }

    public static EventManager GetInstance()
    {
        instance ??= new EventManager();
        return instance;
    }

    public void OnCustomerHitted()
    {  
        CustomerHitted?.Invoke();
    }
}
