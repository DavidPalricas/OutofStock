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
    public NavMeshAgent agent;

    /// <summary>
    /// The destinationoffset attribute represents a offset from the NPC destination 
    /// </summary>
    protected const float DESTINATION_OFFSET = 1.5f;

    /// <summary>
    /// The currentTargetPos attribute represents the current target position of the NPC
    /// </summary>
    protected Vector3 currentTargetPos;

    /// <summary>
    /// The WasAttacked attribute  is a flag that indicates whether the customer was attacked or not.
    /// </summary>
    /// <value>
    ///   <c>true</c> if [was attacked]; otherwise, <c>false</c>.
    /// </value>
    public bool WasAttacked { get; set; } = false;

    /// <summary>
    /// The DestinationReached property is a flag that indicates whether the NPC has reached its destination or not.
    /// </summary>
    /// <value>
    ///   <c>true</c> if [destination reached]; otherwise, <c>false</c>.
    /// </value>
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

        agent.enabled = true;

        if (agent.isOnNavMesh)
        {
            DestinationReached = false;

            agent.SetDestination(destination);
            agent.isStopped = false;
        }
    }

    /// <summary>
    /// The EnableOrDisableAgent method is responsible for enabling or disabling the agent.
    /// </summary>
    /// <param name="enable">A flag that indicates whether to enable the agent (<c>true</c>) or disable it (<c>false</c>).</param>
    public void EnableOrDisableAgent(bool enable)
    {
        agent.enabled = enable;

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
