using UnityEngine;
using UnityEngine.AI;


public class AnnoyingKidMovement : CustomerMovement
{
    [SerializeField]
    private float distanceToRun;
    public bool HoldsProduct { get; set; } = false;

    public void Run()
    {
        SetAgentDestination(GetRandomPos());
    }


    private Vector3 GetRandomPos()
    {   
        Vector3 randomDirection = transform.position + Random.insideUnitSphere * distanceToRun;

        randomDirection.y = 0;

        float range = Utils.RandomFloat(1f, distanceToRun);

        NavMesh.SamplePosition(randomDirection, out NavMeshHit navHit,range, NavMesh.AllAreas);
        
         return navHit.position;
    }
}
