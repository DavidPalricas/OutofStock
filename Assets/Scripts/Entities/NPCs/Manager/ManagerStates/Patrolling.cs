using UnityEngine;

public class Patrolling : ManagerState
{
    protected override void Awake()
    {
        base.Awake();
        StateName = GetType().Name;
        timetoWait = PlayerPrefs.GetFloat("ManagerPatrolTime");
    }


    public override void Enter()
    {
        base.Enter();

        movement.ChoosePointToPatrol();
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
        Utils.WaitAndExecute(timetoWait, () => fSM.ChangeState("GoToOffice"));  
    }

    public override void Exit()
    {
       base.Exit();
    }
}
