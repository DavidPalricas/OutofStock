using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The NPCMovement class is responsible for representing the share logic between the 
/// movement of different  NPC types
/// </summary>
public class NPCMovement : MonoBehaviour
{
    /// <summary>
    /// The agent attribute represents the NavMeshAgent component.
    /// </summary>
    [SerializeField]
    protected NavMeshAgent agent;

    /// <summary>
    /// The destinationoffset attribute represents a offset from the NPC destination 
    /// </summary>
    protected const float DESTINATIONOFFSET = 1f;

    /// <summary>
    /// The OnEnable method is called when the object becomes enabled and active (Unity Callback).
    /// In this method, the agent obstacle avoidance type is set to HighQualityObstacleAvoidance 
    /// and the agent destination is set by calling the SetAgentDestination method.
    /// This method can be overridden in the child classes.
    /// </summary>
    protected virtual void OnEnable()
    {
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        SetAgentDestination();
    }

    /// <summary>
    /// The Update method is called every frame (Unity Callback).
    /// In this method, we are checking if the NPC has reached its destination, if so, the ReachDestination method is called.
    /// </summary>
    protected void Update()
    {
        if (IsAgentEnabled() && agent.remainingDistance <= DESTINATIONOFFSET)
        {
            DestinationReached();
        }
    }

    /// <summary>
    /// The SetAgentDestination method is responsible for setting the agent destination.
    /// Its implementation is defined in the child classes.
    /// </summary>
    protected virtual void SetAgentDestination() {}

    /// <summary>
    /// The DestinationReached method is responsible for handling the destination reached event.
    /// Its implementation is defined in the child classes.
    /// </summary>
    protected virtual void DestinationReached() {}

    /// <summary>
    /// The ChangeState method is responsible for changing the NPC state.
    /// Its implementation is defined in the child classes.
    /// </summary>
    protected virtual void ChangeState() { }

    /// <summary>
    /// The IsAgentEnabled method is responsible for checking if the agent is enabled.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if [is agent enabled]; otherwise, <c>false</c>.
    /// </returns>
    public bool IsAgentEnabled()
    {
        return agent.enabled;
    }
}
