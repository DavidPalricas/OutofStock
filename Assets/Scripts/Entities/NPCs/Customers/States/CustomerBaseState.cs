/// <summary>
/// The CustomerBaseState class is responsible for representig the base state of a customer state.
/// </summary>
public class CustomerBaseState : State
{
    /// <summary>
    /// The customerMovement attribute is a reference to the CustomerMovement component.
    /// </summary>
    protected CustomerMovement customerMovement;

    /// <summary>
    /// The Awake Method is called when the script instance is being loaded (Unity Callback).
    /// It sets the fSM and customerMovement attributes.
    /// </summary>
    protected virtual void Awake()
    {
        fSM = GetComponent<FSM>();
        customerMovement = GetComponent<CustomerMovement>();
    }
}
