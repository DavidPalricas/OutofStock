using UnityEngine;

/// <summary>
///  The MarketProduct class is responsible for handling the logic of the products in the market.
///  It implements the ISubject interface to notify its observers (customer tha want this product) when the product is
///  grabbed by the player.
/// </summary>
public class MarketProduct : Item, ISubject
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
    /// If these conditions are met the attack event is triggered, and the customer is knocked down.
    /// It also is checked if the attacked customer is a hostile Karen.
    /// After that, the base class behavior of this method is called.
    /// </remarks>
    /// <param name="collision">The collision.</param>
    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Customer") && thrown)
        {   
            GameObject customer = collision.gameObject;

            EventManager.GetInstance().OnCustomerAttacked(customer);

            HostileKarenAttacked(customer);

            KnockCustumer(customer);
        }

        base.OnCollisionEnter(collision); 
    }

    /// <summary>
    /// The KnockCustumer method is responsible for knocking down a customer.
    /// </summary>
    /// <remarks>
    ///  When a customer is hitted by a product, its navmesh agent is disabled, its rigidbody is set to kinematic (disabling physics), its position and rotation are changed to simultate the customer is layed.
    /// After 5 seconds, the customer is set to stand up again (StandUp Coroutine).
    /// </remarks>
    /// <param name="customer"> The customer hitted by the objected </param>
    private void KnockCustumer(GameObject customer)
    {
        if (!customer.GetComponent<CustomerMovement>().IsAgentEnabled())
        {
            return;
        }

        customer.GetComponent<CustomerMovement>().EnableOrDisanableAgent(false);

        Rigidbody customerRb = customer.GetComponent<Rigidbody>();

        customerRb.isKinematic = true;

        customer.transform.rotation = Quaternion.Euler(90f, 0, 0);

        Vector3 customerPos = customer.transform.position;

        customer.transform.position = new Vector3(customerPos.x, 0.5f, customerPos.z);

        const float KNOCKDOWNTIME = 5f;

        StartCoroutine(Utils.WaitAndExecute(KNOCKDOWNTIME, () => StandUp(customerRb, customer, customerPos.y)));
    }

    /// <summary>
    /// The StandUp method is responsible for making the customer stand up again.
    /// </summary>
    /// <remarks>
    /// In this method, the customer rigidbody is set to non kinematic, its position and rotation are changed to simulate the customer is standing up and its navmesh agent is enabled.
    /// </remarks>
    /// <param name="custumerRb">The custumer rigid body.</param>
    /// <param name="customer">The customer.</param>
    /// <param name="originalCustomerPosY">The original position in the y axis</param>
    private void StandUp(Rigidbody custumerRb, GameObject customer, float originalCustomerPosY)
    {
        custumerRb.isKinematic = false;

        customer.transform.rotation = Quaternion.identity;

        Vector3 customerPos = customer.transform.position;
        customer.transform.position = new Vector3(customerPos.x, originalCustomerPosY, customerPos.z);

        customer.GetComponent<CustomerMovement>().EnableOrDisanableAgent(true);
    }

    /// <summary>
    /// The HostileKarenAttacked method is responsible for checking if the customer attacked is a hostile Karen.
    /// </summary>
    /// <remarks>
    /// To check if the customer is a hostile Karen, it is checked if the current state of the customer is AttackPlayer.
    /// This state is exclusive to the Karen Stereotype and is used when she wants to attack the player (is hostile)
    /// If this condition is met, the attacked counter of the Karen is increased.
    /// </remarks>
    /// <param name="customer">The customer.</param>
    private void HostileKarenAttacked(GameObject customer)
    {
        if (customer.GetComponent<FSM>().CurrentState is AttackPlayer)
        {
             customer.GetComponent<AttackPlayer>().AttackedCounter++;   
        }
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
}
