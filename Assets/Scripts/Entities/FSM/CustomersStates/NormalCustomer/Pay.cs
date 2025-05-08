using UnityEngine;

public class Pay : CustomerBaseState
{
    [SerializeField] 
    private float minTimeToPay, maxTimeToPay;

    protected override void Awake()
    {
        base.Awake();
        stateName = GetType().Name;
    
    }
    public override void Enter()
    {
        base.Enter();
        customerMovement.SetAgentDestination(customerMovement.AreasPos["Payment"]);
    }


    public override void Execute()
    {
        base.Execute();


        if (customerMovement.DestinationReached)
        {
            PayItem();
        }
    }
    public override void Exit()
    {
        base.Exit();
    }


    private void PayItem()
    {
        StartCoroutine(Utils.WaitAndExecute(Utils.RandomFloat(minTimeToPay, maxTimeToPay), () => fSM.ChangeState("ProductPaid")));
    }
}
