using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// The AnnoyingKidMovement class is responsible for handling the annoying kid movement in the market.
/// </summary>
public class AnnoyingKidMovement : CustomerMovement
{
    /// <summary>
    /// The distanceToRun attribute is the distance that the annoying kid will run in the market.
    /// </summary>
    [SerializeField]
    private float distanceToRun;

    /// <summary>
    /// The HoldsProduct attribute is a flag that indicates whether the annoying kid holds a market product or not.
    /// </summary>
    /// <value>
    ///   <c>true</c> if [holds product]; otherwise, <c>false</c>.
    /// </value>
    public bool HoldsProduct { get; set; } = false;

    /// <summary>
    /// The GetRandomPos method is responsible for generating a random position in the market, which 
    /// is in the annoying kid's range and are of movement ( distance to run and its NavMesh).
    /// </summary>
    /// <returns>Random position in the market</returns>
    private Vector3 GetRandomPos()
    {
        Vector3 randomDirection = transform.position + Random.insideUnitSphere * distanceToRun;

        randomDirection.y = 0;

        float range = Utils.RandomFloat(1f, distanceToRun);

        NavMesh.SamplePosition(randomDirection, out NavMeshHit navHit, range, NavMesh.AllAreas);

        return navHit.position;
    }

    /// <summary>
    /// The Run method is responsible for setting the annoying kid's destination to a random position in the market,
    /// by calling the SetAgentDestination method with the GetRandomPos method as parameter.
    /// </summary>
    public void Run()
    {
        SetAgentDestination(GetRandomPos());
    }
}
