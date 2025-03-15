/// <summary>
/// The IObserver interface is responsible for implementing an interface for implementing observers of the Observer pattern (gaming programing  pattern).
/// </summary>
public interface IObserver
{
    /// <summary>
    /// The UpdateObserver method is responsible for updating the observer.
    /// </summary>
    void UpdateObserver();
}

/// <summary>
/// The ISubject interface is responsible for implementing an interface for implementing subjects of the Observer pattern (gaming programing pattern).
/// </summary>
public interface ISubject
{
    /// <summary>
    /// The AddObserver method is responsible for adding an observer to the subject.
    /// </summary>
    /// <param name="observers">The observers.</param>
    void AddObservers(IObserver[] observers);

    /// <summary>
    /// The RemoveObservers method is responsible for removing the observers from the subject.
    /// </summary>
    void RemoveObservers();

    /// <summary>
    /// The NotifyObservers method is responsible for notifying the observers.
    /// </summary>
    void NotifyObservers();
}
