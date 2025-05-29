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
    /// The managerOffice attribute represents the manager's office position.
    /// </summary>
    public Vector3 ManagerOffice { get; private set; }

    /// <summary>
    /// The IsPatrolling attribute is a flag that indicates whether the manager is patrolling or not.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is patrolling; otherwise, <c>false</c>.
    /// </value>
    public bool IsPatrolling { get; set; } = false;

    /// <summary>
    /// The Awake method is called when the script instance is being loaded (Unity Callback).
    /// It initializes the wayPoints attribute with the children of the waypointsGroup transform and the ManagerOffice attribute with the transform position.
    /// </summary>
    private void Awake()
    {
        wayPoints = Utils.GetChildren(waypointsGroup);
        ManagerOffice = transform.position;
    }

    /// <summary>
    /// The ChoosePointToPatrol method is responsible for choosing a random waypoint from the wayPoints array and setting it as the agent's destination.
    /// </summary>
    public void ChoosePointToPatrol()
    {
        int randomWayPoint = Utils.RandomInt(0, wayPoints.Length);

        SetAgentDestination(wayPoints[randomWayPoint].transform.position);
    }

    public void StopManager()
    {
        if (agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }
    }
}
