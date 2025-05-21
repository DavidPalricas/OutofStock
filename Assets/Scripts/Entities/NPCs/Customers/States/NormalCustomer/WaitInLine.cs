using UnityEngine;

public class WaitInLine : CustomerBaseState
{   

    private PaymentLines paymentLines;

    private Vector3 paymentAreaPos;
    /// <summary>
    /// The Awake Method is called when the script instance is being loaded (Unity Callback).
    /// It calls the base class Awake method and sets the stateName to the name of the current class.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        StateName = GetType().Name;

        paymentLines = GameObject.FindGameObjectWithTag("PaymentLines").GetComponent<PaymentLines>();
    }


    private void Start()
    {
        paymentAreaPos = customerMovement.AreasPos["Payment"];
    }

    /// <summary>
    /// The Enter method is called when the state is entered.
    /// It calls the base class Enter method and sets the customer destination to the payment area.
    /// </summary>
    public override void Enter()
    {
        base.Enter();

        paymentLines.AddCustomerToLine(gameObject, paymentAreaPos);
    }

    /// <summary>
    /// The Execute method is called when the state is executed, to perform the actions of the state.
    /// It calls the base class Execute method.
    /// <remarks>
    /// This methods checks for possible conditions to change the state, otherwise it continues the states actions.
    /// The possible transitions are:
    ///    1. Attacked Transition: If the customer is attacked, it changes to the Knocked state.
    ///    2. ProductPaid Transition: Calls the PayItem method when the customer reaches its destination to handle the payment process and then changes to the Go Home state.
    ///    
    /// If the none of these conditions are met, nothing happens until the customer reaches its destination or is attacked.
    /// </remarks>
    /// </summary>
    public override void Execute()
    {
        base.Execute();

        if (customerMovement.WasAttacked)
        {
            paymentLines.RemoveCustomer(gameObject, paymentAreaPos);

            fSM.ChangeState("Attacked");
            return;
        }

        if (paymentLines.ReadyToPay(gameObject, paymentAreaPos))
        {
            paymentLines.RemoveCustomer(gameObject, paymentAreaPos);
            fSM.ChangeState("ReadyToPay");
            return;
        }
    }

    /// <summary>
    /// The Exit method is called when the state is exited, to handle its final actions.
    /// It calls the base class Exit method.
    /// </summary>
    public override void Exit()
    {
        base.Exit();
    }
}
