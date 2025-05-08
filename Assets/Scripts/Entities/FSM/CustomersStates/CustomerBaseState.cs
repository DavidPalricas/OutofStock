public class CustomerBaseState : State
{
    protected enum CustomersTypes
    {
        Normal,
        Karen,
        AnnoyingKid
    }

    protected CustomersTypes customerType;

    protected CustomerMovement customerMovement;

    protected virtual void Awake()
    {
        fSM = GetComponent<FSM>();
        customerMovement = GetComponent<CustomerMovement>();
        customerType = GetCustomerType();
    }


    private CustomersTypes GetCustomerType()
    {
       if (gameObject.name.Contains("Karen"))
       {
            return CustomersTypes.Karen;
       }

       if (gameObject.name.Contains("AnnoyingKid"))
       {
           return CustomersTypes.AnnoyingKid;
       }

       return CustomersTypes.Normal;
    }
}
