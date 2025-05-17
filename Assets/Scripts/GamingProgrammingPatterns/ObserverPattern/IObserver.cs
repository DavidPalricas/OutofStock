/// <summary>
/// The IObserver interface is responsible for implementing an interface for implementing observers of the Observer pattern (gaming programing  pattern).
/// </summary>
public interface IObserver
{
    /// <summary>
    /// The OnNotify method is responsible for updating the observer, when a subject notifies it.
    /// </summary>
    /// <param name="data">Any argument to be sent to the observer.</param>
    void OnNotify(object data = null);
}
