public interface IObserver
{
    void UpdateObserver();
}

public interface ISubject
{
    void AddObservers(IObserver[] observers);
    void RemoveObservers();
    void NotifyObservers();
}
