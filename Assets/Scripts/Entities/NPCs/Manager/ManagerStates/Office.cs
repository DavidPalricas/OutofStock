
using UnityEngine;

public class Office: ManagerState
{
    protected override void Awake()
    {
        base.Awake();
        stateName = GetType().Name;
        timetoWait = PlayerPrefs.GetFloat("ManagerOfficeTime");
    }

    public override void Enter()
    {
        base.Enter();

        movement.SetAgentDestination(movement.ManagerOffice);
    }

    public override void Execute()
    {
        base.Execute();

        if (movement.DestinationReached)
        {
            StayOnPoint();
        }
    }


    protected override void StayOnPoint()
    {
        Utils.WaitAndExecute(timetoWait, () => fSM.ChangeState("Patrol"));
    }


    public override void Exit()
    {
        base.Exit();
    }
}
