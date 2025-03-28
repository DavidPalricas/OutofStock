using UnityEngine;

/// <summary>
///  The PickUpItemCollisions class is responsible for handling the collisions of the pick up itens.
/// </summary>
public class ItemLogic : MonoBehaviour,ISubject
{
    private Rigidbody rB;

    /// <summary>
    /// The eventDispatcher attribute is used to store the singleton instance of the EventDispatcher class to dispatch events.
    /// </summary>
    private EventDispatcher eventDispatcher;

    /// <summary>
    /// The WasGrabbed property is used to store if the item was grabbed by the player.
    /// </summary>
    /// <value>
    ///   <c>true</c> if [was grabbed]; otherwise, <c>false</c>.
    /// </value>
    private bool wasThrown;

    /// <summary>
    /// The observers attribute is used to store the observers of the item.
    /// In this case there is only one observer, the customer who is looking for the item.
    /// </summary>
    private IObserver[] observers;

    /// <summary>
    /// The pickItemArea attribute is the area where the custumers can pick up the item.
    /// </summary>
    public Transform pickItemArea;

    /// <summary>
    /// The Awake Method is called when the script instance is being loaded (Unity Callback).
    /// In this method, some attributes are initialized.
    /// </summary>
    private void Awake()
    {
        rB = GetComponent<Rigidbody>();

        eventDispatcher = EventDispatcher.GetInstance();

        wasThrown = false;
    }

    /// <summary>
    /// The OnCollisionEnter Method is called when this collider/rigidbody has begun touching another rigidbody/collider (Unity Callback).
    /// </summary>
    /// <remarks>
    /// In this method, after the item was thrown its layer is reset to Default, to be rendered by the main camera instead of the camera that renders the item grabbed by the player.
    /// and if the item collided with a customer, the KnockCustumer method is called to knock down the customer hitted, and the "CustomerAttacked" event is dispatched.
    /// After that, the wasThrown flag is set to false.
    /// </remarks>
    /// <param name="collision">The collision.</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (wasThrown)
        {
            gameObject.layer = LayerMask.NameToLayer("Item");

            if (collision.gameObject.CompareTag("Customer"))
            {
                eventDispatcher.DispatchEvent("CustomerAttacked", collision.gameObject);
                KnockCustumer(collision.gameObject);
            }

            wasThrown = false;
        }
    }

    /// <summary>
    /// The KnockCustumer method is responsible for knocking down a customer.
    /// </summary>
    /// <remarks>
    ///  When a customer is hitted by an item, its navmesh agent is disabled, its rigidbody is set to kinematic (disabling physics), its position and rotation are changed to simultate the customer is layed.
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

        const float POSYOFFSET = 0.1f;
        customer.transform.position = new Vector3(customerPos.x, customerPos.y - POSYOFFSET, customerPos.z);

        const float KNOCKDOWNTIME = 5f;

        StartCoroutine(Utils.WaitAndExecute(KNOCKDOWNTIME, () => StandUp(customerRb, customer, POSYOFFSET)));
    }

    /// <summary>
    /// The StandUp method is responsible for making the customer stand up again.
    /// </summary>
    /// <remarks>
    /// In this method, the customer rigidbody is set to non kinematic, its position and rotation are changed to simulate the customer is standing up and its navmesh agent is enabled.
    /// </remarks>
    /// <param name="custumerRb">The custumer rigid body.</param>
    /// <param name="customer">The customer.</param>
    /// <param name="POSYOFFSET">The offset for the y position of the customer.</param>
    private void StandUp(Rigidbody custumerRb, GameObject customer, float POSYOFFSET)
    {
        custumerRb.isKinematic = false;

        customer.transform.rotation = Quaternion.identity;

        Vector3 customerPos = customer.transform.position;
        customer.transform.position = new Vector3(customerPos.x, customerPos.y + POSYOFFSET, customerPos.z);

        customer.GetComponent<CustomerMovement>().EnableOrDisanableAgent(true);
    }

    /// <summary>
    /// The WasGrabbed method is responsible for handling the logic when the item is grabbed by the player.
    /// </summary>
    /// <remarks>
    /// First the item collider is set to trigger and is rigibody to kinematic
    /// to avoid physics interactions with other objects.
    /// The item layer is changed to "GrabbedItem" because it prevents the item from being rendered by the main camera
    /// and instead renders it through a dedicated camera. This ensures the item appears in front of
    /// any objects between the player and the item, solving rendering issues like z-fighting or clipping.
    /// </remarks>
    public void WasGrabbed()
    {
        GetComponent<Collider>().isTrigger = true;

        if (!rB.isKinematic)
        {
            rB.isKinematic = true;
        }

       gameObject.layer = LayerMask.NameToLayer("GrabbedItem");
    }

    /// <summary>
    /// The WasThrown method is responsible for handling the logic when the item is thrown by the player.
    /// </summary>
    /// <remarks>
    ///  The wasThrown flag is set to true, the item collider is set to non trigger and its rigidboy not kinemmatic to enable physics interactions with other objects,
    ///  before applying a force to the item in the direction the player is facing, to simulate the player's throw.
    /// </remarks>

    /// <param name="playerFowardDir">The player foward direction.</param>
    public void WasThrown(Vector3 playerFowardDir)
    {
        wasThrown = true;

        GetComponent<Collider>().isTrigger = false;

        rB.isKinematic = false;

        const float THROWFORCE = 20f;

        rB.AddForce(playerFowardDir * THROWFORCE, ForceMode.Impulse);
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
            observer.UpdateObserver();
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
