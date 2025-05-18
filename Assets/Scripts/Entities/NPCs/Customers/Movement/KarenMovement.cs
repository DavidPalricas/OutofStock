using UnityEngine;

public class KarenMovement : CustomerMovement
{
    [SerializeField]
    private float tresholdToResetChase, playerInRange;

    private Vector3 oldPlayerPos = Vector3.zero;

    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;   
    }

    public void ChasePlayer()
    {   
        if (!agent.isOnNavMesh)
        {
          return;
        }


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


    public void StopChasingPlayer()
    {   
        if (!agent.isOnNavMesh)
        {
            return;
        }

        agent.isStopped = true;
    }


    public bool PlayerInRange()
    {
        return Vector3.Distance(playerTransform.position, transform.position) < playerInRange;
    }
}
