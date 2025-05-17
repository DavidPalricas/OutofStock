using UnityEngine;

public class Complain : CustomerBaseState
{
    [SerializeField]
    private int maxComplaining;

    [SerializeField]
    private float complainingCooldown;

    private KarenMovement karenMovement;

    private int complainingCounter = 0;

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
       if (!karenMovement.PlayerInRange())
        {
            fSM.ChangeState("PlayerNotInRange");
            return;
        }

        if (karenMovement.WasAttacked)
        {
            fSM.ChangeState("Attacked");
            return;
        }

        if (complainingCounter >= maxComplaining)
        {
            fSM.ChangeState("ComplainedToMuch");
            return;
        }

        StartCoroutine(Utils.WaitAndExecute(complainingCooldown, Complaining));
    }
    public override void Exit()
    {
        base.Exit();
    }

    private void Complaining()
    {   
        Debug.Log("Complaining to player");
        complainingCounter++;
    }
}