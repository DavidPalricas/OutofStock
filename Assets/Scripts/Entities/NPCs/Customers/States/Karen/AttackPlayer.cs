using UnityEngine;

public class AttackPlayer : CustomerBaseState
{
    [SerializeField]
    private int maxAttacks;

    [SerializeField]
    private float attackCooldown;

    private KarenMovement karenMovement;

    private float timer;


    private int attacked;

    protected override void Awake()
    {
        base.Awake();
        stateName = GetType().Name;
    }

    public override void Enter()
    {
        base.Enter();

        timer = Time.time + attackCooldown;

        if (customerMovement is KarenMovement movement)
        {
            karenMovement = movement;
        }
    }
    public override void Execute()
    {
        base.Execute();

        if (karenMovement.WasAttacked)
        {
            fSM.ChangeState("Attacked");
            attacked++;
            return;
        }

        if (!karenMovement.PlayerInRange())
        {
            fSM.ChangeState("PlayerNotInRange");
            return;
        }

        if (attacked >= maxAttacks)
        {
            fSM.ChangeState("AttackedToManyTimes");
            return;
        }

        if (Time.time >= timer)
        {
            Attack();
            timer = Time.time + attackCooldown;
        }
    }


    public override void Exit()
    {
        base.Exit();
    }

    private void Attack()
    {   
        Debug.Log("Attacking player");
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        new KnockEntity().Knock(player,player.GetComponent<Rigidbody>(), player.transform.position);
    }
}