using UnityEngine;

public class AttackPlayer : CustomerBaseState
{
    [SerializeField]
    private int maxAttacks;

    [SerializeField]
    private float attackCooldown;

    private KarenMovement karenMovement;

    public int AttackedCounter { get; set; } = 1;

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

        if (!karenMovement.PlayerInRange())
        {
            fSM.ChangeState("PlayerNotInRange");
            return;
        }

        if (AttackedCounter >= maxAttacks)
        {
            fSM.ChangeState("AttackedToManyTimes");
            return;
        }

        StartCoroutine(Utils.WaitAndExecute(attackCooldown, Attack));
    }


    public override void Exit()
    {
        base.Exit();
    }

    private void Attack()
    {   
        Debug.Log("Attacking player");
    }
}