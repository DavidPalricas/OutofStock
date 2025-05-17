public class CustomerBaseState : State
{
    protected CustomerMovement customerMovement;

    protected virtual void Awake()
    {
        fSM = GetComponent<FSM>();
        customerMovement = GetComponent<CustomerMovement>();
    }
}
