/// <summary>
///  The IEventDispatcher interface is an interface to implement in classes which are responsible for dispatching events.
/// </summary>
public interface IEventDispatcher
{
    /// <summary>
    /// The DispatchEvents method is used to dispatch one or more events.
    /// </summary>
    public void DispatchEvents();
}
