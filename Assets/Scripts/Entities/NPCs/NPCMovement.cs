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
    protected const float DESTINATION_OFFSET = 1.5f;

    /// <summary>
    /// The currentTargetPos attribute represents the current target position of the NPC
    /// </summary>
    protected Vector3 currentTargetPos;

    public bool DestinationReached { get; protected set; } = false;

  
    /// <summary>
    /// The Update method is called every frame (Unity Callback).
    /// In this method, we are checking if the NPC has reached its destination, if so, the ReachDestination method is called.
    /// </summary>
    private void Update()
    {
        if (IsAgentEnabled()  && !agent.pathPending &&  !agent.isStopped && agent.remainingDistance <= DESTINATION_OFFSET)
        {
            agent.isStopped = true;
            DestinationReached = true;
        }
    }

    /// <summary>
    /// The SetAgentDestination method is responsible for setting the agent destination.
    /// Its implementation is defined in the child classes.
    /// </summary>
    public virtual void SetAgentDestination(Vector3 destination) {

        if (agent.isOnNavMesh)
        {
            DestinationReached = false;

            agent.SetDestination(destination);
            agent.isStopped = false;
        }
    }

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
