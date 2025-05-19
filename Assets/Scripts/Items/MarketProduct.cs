using UnityEngine;

/// <summary>
///  The MarketProduct class is responsible for handling the logic of the products in the market.
///  It implements the ISubject interface to notify its observers (customer tha want this product) when the product is grabbed by the player.
///  And implements the IEventDispatcher interface to dispatch the event when a thrown product hits a customer (customer attacked event).
/// </summary>
public class MarketProduct : Item, ISubject, IEventDispatcher
{
    /// <summary>
    /// The observers attribute is used to store the observers of the product.
    /// In this case there is only one observer, the customer who is looking for the product.
    /// </summary>
    private IObserver[] observers;

    /// <summary>
    /// The pickProductArea attribute is the area where the custumers can pick up the product.
    /// </summary>
    public Transform pickProductArea;

    /// <summary>
    /// The customerHitted attribute is used to store the customer that was hit by the product.
    /// </summary>
    private GameObject customerHitted = null;

    /// <summary>
    /// The Awake Method is called when the script instance is being loaded (Unity Callback).
    /// In this method, its base class is called and the eventDispatcher property is initialized.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// The Update method is called every frame (Unity Callback).
    /// In this method, its checked if the product was grabbed and there are observers
    /// if these conditions are met, the observers are notified and removed.
    /// </summary>
    private void Update()
    {
        if (observers != null && Grabbed)
        {
            NotifyObservers();
            RemoveObservers();
        }
    }

    /// <summary>
    /// The OnCollisionEnter Method is called when this collider/rigidbody has begun touching another rigidbody/collider (Unity Callback).
    /// </summary>
    /// <remarks>
    /// This method checks if the product is thrown and collided with a customer, 
    /// If these conditions are met the attack event is triggered (called in the DispatchEvents method).
    /// </remarks>
    /// <param name="collision">The collision.</param>
    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Customer") && thrown)
        {
            // Checks if product collided with the customer or its extra collider that is used to block entities collisions
            customerHitted =  collision.gameObject.GetComponent<CustomerMovement>() != null ? collision.gameObject : collision.gameObject.transform.parent.gameObject;

            DispatchEvents();
        }

        base.OnCollisionEnter(collision); 
    }
 
    /// <summary>
    /// The AddObserver method is responsible for adding observers to the customer (ISubject interface method).
    /// </summary>
    /// <param name="data">Any argument to be sent to the observer, in this case no argument is specified (null)</param>s
    public void AddObservers(IObserver[] observers)
    {
        this.observers = observers;
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

        RemoveObservers();
    }

    /// <summary>
    /// The RemoveObservers method is responsible for removing the observers from the subject (ISubject interface method).
    /// </summary>
    public void RemoveObservers()
    {
        observers = null;
    }

    /// <summary>
    /// The DispatchEvents method is used to dispatch one or more events.
    /// It notifies the event manager when a product is thrown and hits a customer.
    /// </summary>
    public void DispatchEvents()
    {
        EventManager.GetInstance().OnCustomerAttacked(customerHitted);
    }
}
