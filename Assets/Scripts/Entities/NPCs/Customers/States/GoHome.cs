/// <summary>
/// The GoHome class is responsible for handling the go home state of the customer.
/// This state is used by all customer stereotypes.
/// </summary>
public class GoHome : CustomerBaseState
{
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
    /// It calls the base class Enter method and sets the market's exit as the destination for the customer.
    /// </summary>
    public override void Enter()
    {
        base.Enter();

        customerMovement.SetAgentDestination(customerMovement.AreasPos["MarketExit"]); 
    }

    /// <summary>
    /// The Execute method is called when the state is executed, to perform the actions of the state.
    /// It calls the base class Execute method.
    /// <remarks>
    /// This methods checks for possible conditions to change the state, otherwise it continues the states actions.
    /// The possible transitions are:
    ///    1. Attacked Transition: If the customer is attacked, it changes to the Knocked state.
    /// 
    /// If the none of these conditions are met, this method checks if the customer has reached its destination, if so, the ExitMarket method is called,
    /// to handle the customer's exit from the market.
    /// </remarks>
    /// </summary>
    public override void Execute()
    {
        base.Execute();

        if (customerMovement.WasAttacked)
        {
            fSM.ChangeState("Attacked");
            return;
        }

        if (customerMovement.DestinationReached)
        {
            customerMovement.ExitMarket();
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
