using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The CustomerMovement class is responsible for handling the customer movement in the market.
/// </summary>
public class CustomerMovement : MonoBehaviour, ISubject
{
    /// <summary>
    /// The agent attribute represents the NavMeshAgent component.
    /// </summary>
    [SerializeField]
    private NavMeshAgent agent;

    /// <summary>
    /// The observers attribute stores the observers of the customer.
    /// </summary>
    private IObserver[] observers;

    /// <summary>
    /// The itemPicked its a flag that indicates if the customer has picked the item we wanted.
    /// </summary>
    private bool itemPicked = false;

    /// <summary>
    /// The destinationoffset attribute represents a offset from the customer destination 
    /// </summary>
    private const float DESTINATIONOFFSET = 0.5f;

    /// <summary>
    /// The TargetItem attribute represents the target item of the customer.
    /// </summary>
    /// <value>
    /// The target item of the customer.
    /// </value>
    public GameObject TargetItem { get; set; }

    /// <summary>
    /// The MarketExitPos attribute represents the market exit position.
    /// </summary>
    /// <value>
    /// The market exit position.
    /// </value>
    public Vector3 MarketExitPos { get; set; }

    /// <summary>
    /// The OnEnable method is called when the object becomes enabled and active (Unity Callback).
    /// In this method, the agent obstacle avoidance type is set to HighQualityObstacleAvoidance and the agent destination is set.
    /// </summary>
    private void OnEnable()
    {
        // It may cost a lot of processing power, but it is necessary to avoid the customers from colliding with each other.
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        SetAgentDestination();
    }

    /// <summary>
    /// The Update method is called every frame (Unity Callback).
    /// In this method, we are checking if the customer has reached its destination, if so, the ReachDestination method is called.
    /// </summary>
    private void Update()
    {   
        if (IsAgentEnabled() && agent.remainingDistance <= DESTINATIONOFFSET)
        {
           DestinationReached();
        }
    }

    /// <summary>
    /// The SetAgentDestination method is responsible for setting the agent destination.
    /// The destination is the target item if the item was not picked, otherwise, the destination is the market exit position.
    /// </summary>
    private void SetAgentDestination()
    {
        agent.SetDestination(itemPicked ? MarketExitPos : TargetItem.transform.position);
    }

    /// <summary>
    /// The DestinationReached method is responsible for handling the destination reached event.
    /// If the item was not picked, it means the customer has reached the target item, so the PickItem method is called.
    /// Otherwise, the ExitMarket method is called (the customer has picked the item and wants to exit the market).
    /// </summary>
    private void DestinationReached()
    {   
        if (!itemPicked)
        {
            PickItem();

            return;
        }

        ExitMarket();
    }

    /// <summary>
    /// The PickItem method is responsible for handling the logic when the customer picks the item.
    /// The itemPicked flag is set to true , the target item is destroyed and the agent destination is updated (SetAgentDestination method).
    /// </summary>
    private void PickItem()
    {
        itemPicked = true;
        Destroy(TargetItem);

        SetAgentDestination();
    }

    /// <summary>
    /// The ExitMarket method is responsible for handling the logic when the customer exits the market.
    /// In this method, the observers are notified and removed, and the customer game object is destroyed.
    /// </summary>
    private void ExitMarket()
    {
        NotifyObservers();
        RemoveObservers();

        Destroy(gameObject);
    }

    /// <summary>
    /// The EnableOrDisableAgent method is responsible for enabling or disabling the agent.
    /// </summary>
    /// <param name="enable">A flag that indicates whether to enable the agent (<c>true</c>) or disable it (<c>false</c>).</param>
    public void EnableOrDisanableAgent(bool enable)
    {
        agent.enabled = enable;

        if (enable)
        {
            SetAgentDestination();
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

    /// <summary>
    /// The RemoveObservers method is responsible for removing the observers from the subject (ISubject interface method).
    /// </summary>
    public void RemoveObservers()
    {
        observers = null;
    }

    /// <summary>
    /// The NotifyObservers method is responsible for notifying the customer observers (ISubject interface method).
    /// </summary>
    public void NotifyObservers()
    {
        foreach (IObserver observer in observers)
        {
            observer.UpdateObserver();
        }
    }

    /// <summary>
    /// The AddObserver method is responsible for adding observers to the customer (ISubject interface method).
    /// </summary>
    /// <param name="observers">The observers of the customer.</param>
    public void AddObservers(IObserver[] observers)
    {   
        this.observers = observers;
    }
}
