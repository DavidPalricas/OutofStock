public class ChasePlayer : CustomerBaseState
{   
    private KarenMovement karenMovement;


    protected override void Awake()
    {
        base.Awake();
        stateName = GetType().Name;
    }
    public override void Enter()
    {
        base.Enter();

        if (customerMovement is KarenMovement movement)
        {
            karenMovement = movement;
        }
    }

    public override void Execute()
    {
        base.Execute();


        if (karenMovement.PlayerInRange())
        {
            fSM.ChangeState("PlayerInRange");
            return;
        }

        karenMovement.ChasePlayer();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
