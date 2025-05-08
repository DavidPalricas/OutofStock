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


    private void Awake()
    {   
        wayPoints = Utils.GetChildren(waypointsGroup);
        ManagerOffice = transform.position;
    }

    public void ChoosePointToPatrol()
    {   
        int randomWayPoint = Utils.RandomInt(0, wayPoints.Length);


        Debug.Log(agent);

        agent.SetDestination(wayPoints[randomWayPoint].transform.position);
    }
}
