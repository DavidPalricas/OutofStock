using UnityEngine;

public class Knocked : CustomerBaseState
{
    [SerializeField]
    private float knockDownTime;

    private float timer;

    private Vector3 customerPosBeforeKnock;

    private KnockEntity knockEntity;


    protected override void Awake()
    {
        base.Awake();
        knockEntity = GetComponent<KnockEntity>();
        stateName = GetType().Name;

    }
    public override void Enter()
    {
        base.Enter();

        timer = Time.time + knockDownTime;

        customerPosBeforeKnock = transform.position;

        knockEntity.Knock(gameObject, customerMovement.GetComponent<Rigidbody>(), customerPosBeforeKnock);
    }

    public override void Execute()
    {
        base.Execute();

        if (Time.time >= timer)
        {
            knockEntity.StandUp(gameObject, customerMovement.GetComponent<Rigidbody>(), customerPosBeforeKnock.y);
            fSM.ChangeState("StandUp");
        }
    }
    
    public override void Exit()
    {
        base.Exit();

        customerMovement.WasAttacked = false;

        EventManager.GetInstance().LastCustomerAttacked = null;
    }
}
