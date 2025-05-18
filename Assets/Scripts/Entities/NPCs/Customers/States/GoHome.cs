public class GoHome : CustomerBaseState
{
    protected override void Awake()
    {
        base.Awake();
        stateName = GetType().Name;
      
    }
    public override void Enter()
    {
        base.Enter();

        customerMovement.SetAgentDestination(customerMovement.AreasPos["MarketExit"]); 
    }

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
    public override void Exit()
    {
        base.Exit();
    }
}
