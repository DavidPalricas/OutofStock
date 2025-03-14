using UnityEngine;
using UnityEngine.AI;

public class CustomerMovement : MonoBehaviour, ISubject
{
    [SerializeField]
    private float walkRange;

    [SerializeField]
    private NavMeshAgent agent;

    private IObserver[] observers;

    private bool itemPicked = false;

    private const float DESTINATIONOFFSET = 0.5f;

    public GameObject TargetItem { get; set; }

    public Vector3 MarketExitPos { get; set; }

    private void OnEnable()
    {
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        SetAgentDestination();
    }


    private void Update()
    {   
        if (IsAgentEnabled() && agent.remainingDistance <= DESTINATIONOFFSET)
        {
           ReachDestination();
        }
    }

    private void SetAgentDestination()
    {
        agent.SetDestination(itemPicked ? MarketExitPos : TargetItem.transform.position);
    }

    private void ReachDestination()
    {   
        if (!itemPicked)
        {
            PickItem();

            return;
        }

        ExitMarket();
    }

    private void ExitMarket()
    {
        NotifyObservers();
        RemoveObservers();

        Destroy(gameObject);
    }

    private void PickItem()
    {
        itemPicked = true;
        Destroy(TargetItem);

        SetAgentDestination();

        agent.isStopped = false;
    }


    public void ActivateOrDesactivateAgent(bool activate)
    {
        agent.enabled = activate;

        if (activate)
        {
            SetAgentDestination();
        }
    }

    public bool IsAgentEnabled()
    {
        return agent.enabled;
    }

    public void RemoveObservers()
    {
        observers = null;
    }

    public void NotifyObservers()
    {
        foreach (IObserver observer in observers)
        {
            observer.UpdateObserver();
        }
    }

    public void AddObservers(IObserver[] observers)
    {   
        this.observers = observers;
    }
}
