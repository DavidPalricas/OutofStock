using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The CustomerMovement class is responsible for handling the customer movement in the market.
/// It inherits from the NPCMovement class the basics of the NPC movement and implements the ISubject and IObserver interfaces.
/// It is a subject to the customers spawn and an observer to the item he is looking for.
/// </summary>
public class CustomerMovement : NPCMovement, ISubject, IObserver
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
        PAY,
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
    /// The AreasPos attribute represents the positions of the areas in which 
    ///  the customer will move depending on its current state.
    /// </summary>
    public Dictionary<string, Vector3> AreasPos = new ()
    {
        { "PickItem", Vector3.zero},
        { "Payment", Vector3.zero },
        { "MarketExit", Vector3.zero }
    };

 
    /// <summary>
    /// The PayOrPickItem method is responsible for handling the logic when the customer pays or picks the item.
    /// It simulates the time it takes for the customer to pay or pick the item, the time of each type of simulitaion is different.
    /// After getiing the time, the customer state is changed and its destination is set.
    /// </summary>
    private void PayOrPickItem(){
        float minTimeSimulation , maxTimeSimulation;

        if (currentState == CustomerStates.SHOPPING)
        {
            minTimeSimulation = 2f;
            maxTimeSimulation = 3f;
        }
        else
        {
            minTimeSimulation = 3f;
            maxTimeSimulation = 5f;
        }

        ChangeState();

        StartCoroutine(Utils.WaitAndExecute(Utils.RandomFloat(minTimeSimulation, maxTimeSimulation), () => SetAgentDestination()));
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
    /// The SetAgentDestination method is responsible for setting the agent destination and enabling its movement.
    /// The destination can be the pick item area, the payment area, or the market exit area, dpeending on the current state of the customer.
    /// This method overrides the <see cref="NPCMovement.SetAgentDestination"/> method from the <see cref="NPCMovement"/> class.
    /// </summary>
    protected override void SetAgentDestination()
    {
        if (AreasPos["PickItem"] == Vector3.zero)
        {
            agent.SetDestination(AreasPos["MarketExit"]);

            Debug.LogWarning("The item " + TargetItem.name + "does not have a pickItem Area");
            return;
        }

        switch(currentState)
        {
            case CustomerStates.SHOPPING:
                agent.SetDestination(AreasPos["PickItem"]);
               break;

            case CustomerStates.PAY:
                // Destroys the item that he picked
                Destroy(TargetItem);

                agent.SetDestination(AreasPos["Payment"]);
                break;

            case CustomerStates.GO_HOME:
                agent.SetDestination(AreasPos["MarketExit"]);
               break;

            default:
                break;
        }

        agent.isStopped = false;
    }

    /// <summary>
    /// The DestinationReached method is responsible for handling the destination reached event.
    /// This method overrides the <see cref="NPCMovement.DestinationReached"/> method from the <see cref="NPCMovement"/> class.
    /// </summary>
    /// <remarks>
    /// This method uses its base implementation (stop the customer) and checks the current state of the customer.
    /// If the customer is shopping, the PickItem method is called to handle the logic when the customer picks the item.
    /// If the customer is paying, the Pay method is called to handle the logic when the customer pays the item.
    /// Otherwise, the customer exits the market by calling the ExitMarket method.
    /// </remarks>
    protected override void DestinationReached()
    {
        base.DestinationReached();

        switch (currentState)
        {
            case CustomerStates.SHOPPING:
            case CustomerStates.PAY:
                PayOrPickItem();
                return;

            case CustomerStates.GO_HOME:
                ExitMarket();
                return;

            default:
                return;
        }
    }

    /// <summary>
    /// The ChangeState method is responsible for changing the Customer state.
    /// This method overrides the <see cref="NPCMovement.ChangeState"/> method from the <see cref="NPCMovement"/> class.
    /// </summary>
    protected override void ChangeState()
    {   
        if (currentState == CustomerStates.SHOPPING)
        {
            currentState = CustomerStates.PAY;

            return;

        }
        
        if(currentState == CustomerStates.PAY)
        {
            currentState = CustomerStates.GO_HOME;
        }
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
    /// <param name="data">Any argument to be sent to the observer, in this case no argument is specified (null)</param>
    public void NotifyObservers(object data = null)
    {
        foreach (IObserver observer in observers)
        {
            observer.OnNotify();
        }
    }

    /// <summary>
    /// The AddObserver method is responsible for adding observers to the customer (ISubject interface method).
    /// </summary>
    /// <param name="observers">The observers (only one the customers spawn).</param>
    public void AddObservers(IObserver[] observers)
    {   
        this.observers = observers;
    }

    /// <summary>
    /// The OnNotify (IObserver method) method is responsible for updating the observer (this game object), when a subject notifies it.
    /// The customer state is changed to GO_HOME and the agent destination is set to the market exit area.
    /// Because the player picked the item that the customer was looking for, so he exits the market.
    /// </summary>
    /// <param name="data">Any argument to be sent to the observer, in this case no argument is specified (null)</param>
    public void OnNotify(object data = null)
    {
        currentState = CustomerStates.GO_HOME;

        SetAgentDestination();
    }
}
