using UnityEngine;

/// <summary>
/// The ManagerMovement class is responsible for representing the supermarkt's manager movement logic.
/// </summary>
public class ManagerMovement : NPCMovement
{
    /// <summary>
    /// The waypointsGroup attribute represents the waypoints group where the manager will patrol.
    /// </summary>
    [SerializeField]
    private Transform waypointsGroup;

    /// <summary>
    /// The wayPoints attribute represents the waypoints where the manager will patrol.
    /// </summary>
    private GameObject[] wayPoints;

    /// <summary>
    /// The managerPost attribute represents the manager post position.
    /// </summary>
    private Vector3 managerPost;

    /// <summary>
    /// The ManagerStates enum represents the manager states.
    /// </summary>
    private enum ManagerStates
    {
        PATROL,
        OFFICE
    }

    /// <summary>
    /// The currentState attribute represents the current state of the manager.
    /// </summary>
    private ManagerStates currentState = ManagerStates.PATROL;

    /// <summary>
    /// The OnEnable method is called when the object becomes enabled and active (Unity Callback).
    /// In this method, the waypoints are retrieved and the manager post position is set.
    /// This method overrides the <see cref="NPCMovement.OnEnable"/> method from the <see cref="NPCMovement"/> class."/> and uses its base implementation.
    /// </summary>
    protected override void OnEnable()
    {
        wayPoints = Utils.GetChildren(waypointsGroup);
        managerPost = transform.position;

        // Calls the base implementation of the OnEnable method from the NPCMovement class.
        base.OnEnable();
    }

    /// <summary>
    /// The SetAgentDestination method is responsible for setting the agent destination.
    /// This method overrides the <see cref="NPCMovement.SetAgentDestination"/> method from the <see cref="NPCMovement"/> class.
    /// </summary>
    /// <remarks>
    /// If the current state is PATROL, a random waypoint is selected from the waypoints group and set as the agent destination.
    /// Otherwise, the manager post position is set as the agent destination.
    /// </remarks>
    protected override void SetAgentDestination()
    {
        if (currentState == ManagerStates.PATROL)
        {
            int randomWayPoint = Utils.RandomInt(0, wayPoints.Length);

            agent.SetDestination(wayPoints[randomWayPoint].transform.position);
        }
        else
        {
            agent.SetDestination(managerPost);
        }

        agent.isStopped = false;
    }

    /// <summary>
    /// The DestinationReached method is responsible for handling the destination reached event.
    /// This method uses its base implementation (stop the manager) and his state 
    /// is changed and the agent destination is updated after an random interval between 3 and 5 seconds.
    /// This method overrides the <see cref="NPCMovement.DestinationReached"/> method from the <see cref="NPCMovement"/> class.
    /// </summary>
    protected override void DestinationReached()
    {   
        base.DestinationReached();

        ChangeState();

        float timeToWait = currentState == ManagerStates.PATROL ? PlayerPrefs.GetFloat("ManagerPatrolTime") : PlayerPrefs.GetFloat("ManagerOfficeTime");

        // Convert the time to wait from minutes to seconds
        timeToWait *= 60f;

        Debug.Log($"Manager is waiting for {timeToWait} seconds in {currentState} state.");

        StartCoroutine(Utils.WaitAndExecute(timeToWait, () => SetAgentDestination()));
    }

    /// <summary>
    /// The ChangeState method is responsible for changing the Manager state.
    /// This method overrides the <see cref="NPCMovement.ChangeState"/> method from the <see cref="NPCMovement"/> class.
    /// </summary>
    protected override void ChangeState()
    {
        currentState = currentState == ManagerStates.PATROL ? ManagerStates.OFFICE : ManagerStates.PATROL;
    }
}
