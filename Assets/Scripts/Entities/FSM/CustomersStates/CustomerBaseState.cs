public class CustomerBaseState : State
{   
    protected enum CustumersTypes
    {
        Normal,
        Karen,
        AnnoyingKid
    }

    protected CustumersTypes custumerType = CustumersTypes.Normal;

    protected CustomerMovement customerMovement;

    protected virtual void Awake()
    {
        fSM = GetComponent<FSM>();
        customerMovement = GetComponent<CustomerMovement>();
    }
}
