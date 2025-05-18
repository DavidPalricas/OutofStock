using UnityEngine;

/// <summary>
///  The Pay class is responsible for handling the payment state of the customer.
///  This state is exclusively for the Normal Customer Stereotype.
/// </summary>
public class Pay : CustomerBaseState
{
    /// <summary>
    /// The minimumTimeToPay and maximumTimeToPay attributes are the time that the customer will take to pay for the product.
    /// </summary>
    [SerializeField] 
    private float minTimeToPay, maxTimeToPay;

    /// <summary>
    /// The Awake Method is called when the script instance is being loaded (Unity Callback).
    /// It calls the base class Awake method and sets the stateName to the name of the current class.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        stateName = GetType().Name;
    
    }

    /// <summary>
    /// The Enter method is called when the state is entered.
    /// It calls the base class Enter method and sets the customer destination to the payment area.
    /// </summary>
    public override void Enter()
    {
        base.Enter();
        customerMovement.SetAgentDestination(customerMovement.AreasPos["Payment"]);
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
            fSM.ChangeState("Attacked");
        }

        if (customerMovement.DestinationReached)
        {
            PayItem();
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

    /// <summary>
    /// The PayItem method is responsible for handling the payment process.
    /// It simulates the time taken to pay for the product and then changes the state to the Go Home state.
    /// </summary>
    private void PayItem()
    {
        StartCoroutine(Utils.WaitAndExecute(Utils.RandomFloat(minTimeToPay, maxTimeToPay), () => fSM.ChangeState("ProductPaid")));
    }
}
