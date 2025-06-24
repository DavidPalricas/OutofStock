
using UnityEngine;

/// <summary>
/// The Office class is responsible for handling the logic when the player goes to the manager's office.
/// </summary>
public class Office: ManagerState
{
    /// <summary>
    /// The Awake method is called when the script instance is being loaded (Unity Callback).
    /// In this method,the stateName is set to the name of the current class and the time to wait is retrieved from PlayerPrefs.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        StateName = GetType().Name;
        timetoWait = PlayerPrefs.GetFloat("ManagerOfficeTime");
    }

    /// <summary>
    /// The Enter method is called when the state is entered.
    /// This method enables the manager's going to the office behavior by setting the IsPatrolling flag to false and setting the destination to the manager's office.
    /// </summary>
    public override void Enter()
    {
        base.Enter();

        timer = 0f;

        movement.IsPatrolling = false;

        movement.SetAgentDestination(movement.ManagerOffice);
        animator.SetFloat("Speed", 1f);
    }

    /// <summary>
    /// The Execute method is called when the state is executed, to perform the actions of the state.
    /// It calls the base class Execute method.
    /// <remarks>
    /// This methods checks for possible conditions to change the state, otherwise it continues the states actions.
    /// The possible transitions are:
    ///    1. Attacked Transition: If the manager is attacked, it changes to the Knocked state.
    ///    2. Patrol: If the manager reachs the office point, it changes to the Patrol state after waiting for a certain amount of time.
    ///    
    /// If the none of these conditions are met, nothing happens until the manager reaches its destination or is attacked.
    /// </remarks>
    /// </summary>
    public override void Execute()
    {
        base.Execute();

        Debug.Log("Manager is now going to his office.");

        if (movement.WasAttacked)
        {
            fSM.ChangeState("Attacked");
            return;
        }

        if (movement.DestinationReached)
        {
            StayOnPoint("Patrol");
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
