using System;
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
        { "Product", Vector3.zero},
        { "Payment", Vector3.zero },
        { "MarketExit", Vector3.zero }
    };

    /// <summary>
    /// The ExitMarket method is responsible for handling the logic when the customer exits the market.
    /// In this method, the observers are notified and removed, and the customer game object is destroyed.
    /// </summary>
    public void ExitMarket()
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
    public override void SetAgentDestination(Vector3 destination)
    {
        if (AreasPos["Product"] == Vector3.zero)
        {
            agent.SetDestination(AreasPos["MarketExit"]);

            Debug.LogWarning("The item " + TargetItem.name + "does not have a pickItem Area");
            return;
        }

        base.SetAgentDestination(destination);
    }

    /// <summary>
    /// The EnableOrDisableAgent method is responsible for enabling or disabling the agent.
    /// </summary>
    /// <param name="enable">A flag that indicates whether to enable the agent (<c>true</c>) or disable it (<c>false</c>).</param>
    public void EnableOrDisanableAgent(bool enable)
    {
        agent.enabled = enable;

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
       
    }
}
