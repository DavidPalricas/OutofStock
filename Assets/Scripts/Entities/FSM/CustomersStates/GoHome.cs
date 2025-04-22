public class GoHome : CustomerBaseState
{
    protected override void OnEnable()
    {
        base.OnEnable();
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

        if (customerMovement.GoalReached)
        {
            customerMovement.ExitMarket();
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
