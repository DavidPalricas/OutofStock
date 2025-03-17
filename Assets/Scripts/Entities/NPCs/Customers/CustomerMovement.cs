using UnityEngine;

/// <summary>
/// The CustomerMovement class is responsible for handling the customer movement in the market.
/// </summary>
public class CustomerMovement : NPCMovement, ISubject
{
    /// <summary>
    /// The observers attribute stores the observers of the customer.
    /// In this case, there is only one observer (the customers spawn)
    /// This observer is notified when the customer exits the market to spawn a new customer.
    /// </summary>
    private IObserver[] observers;

    /// <summary>
    /// The CustomerStates enum represents the customer states.
    /// </summary>
    private enum CustomerStates
    {
        SHOPPING,
        GO_HOME
    }

    /// <summary>
    /// The currentState attribute represents the current state of the customer.
    /// </summary>
    private CustomerStates currentState = CustomerStates.SHOPPING;

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
    /// The PickItem method is responsible for handling the logic when the customer picks the item.
    /// The item is destroyed, the customer state is changed, and the agent destination is updated.
    /// </summary>
    private void PickItem()
    {
        Destroy(TargetItem);

        ChangeState();

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
    /// The SetAgentDestination method is responsible for setting the agent destination.
    /// The destination is the target item if the customer is shopping, otherwise, the market exit position.
    /// This method overrides the <see cref="NPCMovement.SetAgentDestination"/> method from the <see cref="NPCMovement"/> class.
    /// </summary>
    protected override void SetAgentDestination()
    {
        agent.SetDestination(currentState == CustomerStates.SHOPPING ? TargetItem.transform.position : MarketExitPos);
    }

    /// <summary>
    /// The DestinationReached method is responsible for handling the destination reached event.
    /// This method overrides the <see cref="NPCMovement.DestinationReached"/> method from the <see cref="NPCMovement"/> class.
    /// </summary>
    /// <remarks>
    /// If the customer is shopping, the PickItem method is called to handle the logic when the customer picks the item.
    /// Otherwise, the customer exits the market by calling the ExitMarket method.
    /// </remarks>
    protected override void DestinationReached()
    {
        if (currentState == CustomerStates.SHOPPING)
        {
            PickItem();

            return;
        }

        ExitMarket();
    }

    /// <summary>
    /// The ChangeState method is responsible for changing the Customer state.
    /// This method overrides the <see cref="NPCMovement.ChangeState"/> method from the <see cref="NPCMovement"/> class.
    /// </summary>
    protected override void ChangeState()
    {
        currentState = currentState == CustomerStates.SHOPPING ? CustomerStates.GO_HOME : CustomerStates.SHOPPING;
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
