using UnityEngine;

/// <summary>
/// The KarenMovement class is responsible for handling the movement of the Karen in the market.
/// </summary>
public class KarenMovement : CustomerMovement
{
    /// <summary>
    /// The tresholdToResetChase and playerInRange attributes are used to determine the distance at which the Karen will reset its chase 
    /// and the distance at which the player is considered to be in range.
    /// </summary>
    [SerializeField]
    private float tresholdToResetChase, playerInRange;

    /// <summary>
    /// The maxAttacks attribute is the maximum number of attacks that the Karen can handle.
    /// </summary>
    [SerializeField]
    private int maxAttacks;

    /// <summary>
    /// The oldPlayerPos attribute is used to store the previous position of the player.
    /// </summary>
    private Vector3 oldPlayerPos = Vector3.zero;

    /// <summary>
    /// The playerTransform attribute is used to store the transform of the player.
    /// </summary>
    private Transform playerTransform;

    /// <summary>
    /// The attacksTaken attribute is used to keep track of the number of attacks that the Karen has taken.
    /// </summary>
    private int attacksTaken = 0;

    /// <summary>
    /// The AttackedToManyTimes property indicates whether the Karen has been attacked too many times.
    /// </summary>
    /// <value>
    ///   <c>true</c> if [attacked to many times]; otherwise, <c>false</c>.
    /// </value>
    public bool AttackedToManyTimes { get; private set; } = false;

    /// <summary>
    /// The Awake method is called when the script instance is being loaded (Unity Callback).
    /// It initializes the playerTransform attribute.
    /// </summary>
    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;   
    }

    /// <summary>
    /// The ChasePlayer method is responsible for putting the Karen chasing the player.
    /// </summary>
    /// <remarks>
    /// To avoid uncessary calculations,the new player position is only set at the agent destination if it is distance to the old position is 
    /// grater than the tresholdToResetChase.
    /// </remarks>
    public void ChasePlayer()
    {
        // To avoid errors when the agent is not on the nav mesh, we check if the agent is on the nav mesh before setting its destination.
        if (!agent.isOnNavMesh)
        {
          return;
        }

        // If the agent is stopped, we set it to move again.
        if (agent.isStopped)
        {
            agent.isStopped = false;
        }

        Vector3 playerPos = playerTransform.position;

        if (oldPlayerPos == Vector3.zero || Vector3.Distance(oldPlayerPos, playerPos) > tresholdToResetChase)
        {
            SetAgentDestination(playerPos);
            oldPlayerPos = playerPos;
        }
    }

    /// <summary>
    /// The StopChasingPlayer method is responsible for stopping the Karen from chasing the player.
    /// </summary>
    public void StopChasingPlayer()
    {
        // To avoid errors when the agent is not on the nav mesh, we check if the agent is on the nav mesh before setting its destination.
        if (!agent.isOnNavMesh)
        {
            return;
        }

        agent.isStopped = true;
    }

    /// <summary>
    /// The PlayerInRange method is used to check if the player is in range of the Karen.
    /// To do this, it calculates the distance between the player and the Karen and checks if it is less than the playerInRange attribute.
    /// </summary>
    /// <returns>
    ///  <c>true</c> if the player is in range; otherwise, <c>false</c>.
    /// </returns>
    public bool PlayerInRange()
    {
        return Vector3.Distance(playerTransform.position, transform.position) < playerInRange;
    }

    public void Attacked()
    {
        attacksTaken++;

        if (attacksTaken >= maxAttacks)
        {
            AttackedToManyTimes = true;
        }
    }
}
