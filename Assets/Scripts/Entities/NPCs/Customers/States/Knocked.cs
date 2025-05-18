using UnityEngine;

public class Knocked : CustomerBaseState
{
    protected override void Awake()
    {
        base.Awake();
        stateName = GetType().Name;

    }
    public override void Enter()
    {
        base.Enter();

        GetComponent<KnockEntity>().Knock(gameObject, customerMovement.GetComponent<Rigidbody>(), transform.position);
    }

    public override void Execute()
    {
        base.Execute();
    }
    
    public override void Exit()
    {
        base.Exit();

        customerMovement.WasAttacked = false;

        EventManager.GetInstance().LastCustomerAttacked = null;
    }
}
