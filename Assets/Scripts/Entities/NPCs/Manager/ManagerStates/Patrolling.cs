using UnityEngine;

/// <summary>
///  The Patrolling class is responsible for managing the patrolling state of the supermarket's manager.
/// </summary>
public class Patrolling : ManagerState
{
    /// <summary>
    /// The Awake method is called when the script instance is being loaded (Unity Callback).
    /// In this method,the StateName is set to the name of the current class and the time to wait is retrieved from PlayerPrefs.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        StateName = GetType().Name;
        timetoWait = PlayerPrefs.GetFloat("ManagerPatrolTime");
    }

    /// <summary>
    /// The Enter method is called when the state is entered.
    /// This method enables the manager's patrolling behavior by setting the IsPatrolling flag to true and choosing a point to patrol.
    /// </summary>
    public override void Enter()
    {
        base.Enter();

        timer = 0f;

        movement.IsPatrolling = true;

        movement.ChoosePointToPatrol();

        animator.SetFloat("Speed", 1f);
    }

    /// <summary>
    /// The Execute method is called when the state is executed, to perform the actions of the state.
    /// It calls the base class Execute method.
    /// <remarks>
    /// This methods checks for possible conditions to change the state, otherwise it continues the states actions.
    /// The possible transitions are:
    ///    1. Attacked Transition: If the customer is attacked, it changes to the Knocked state.
    ///    2. Go To Office Transition: If the manager reaches the patrol point, it changes to the Office state after waiting for a certain amount of time.
    ///    
    /// If the none of these conditions are met, nothing happens until the manager reaches its destination or is attacked.
    /// </remarks>
    /// </summary>
    public override void Execute()
    {
        base.Execute();

        if (movement.WasAttacked)
        {
            fSM.ChangeState("Attacked");
            return;
        }

        if (movement.DestinationReached)
        {
            StayOnPoint("GoToOffice");
        }
    }

    /// <summary>
    /// The Exit method is called when the state is exited, to handle its final actions.
    /// It calls the base class Exit method, sets the WasAttacked attribute to false and resets the LastCustomerAttacked attribute in the EventManager instance.
    /// </summary>
    public override void Exit()
    {
       base.Exit();
    }
}
