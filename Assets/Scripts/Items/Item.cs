
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The Item class is responsible for representing an item in the supermarket.
/// And implements the IEventDispatcher interface to dispatch the event when a thrown product hits a customer (customer attacked event).
/// </summary>
public class Item : MonoBehaviour
{
    /// <summary>
    /// The rb property is responsible for storing a reference to the rigidbody component of the item.
    /// </summary>
    protected Rigidbody rB;

    /// <summary>
    /// The thrown property is a flag that indicates whether the item has been thrown or not.
    /// </summary>
    protected bool thrown = false;

    /// <summary>
    /// The Grabbed property is a flag that indicates whether the item has been grabbed or not.
    /// </summary>
    /// <value>
    ///   <c>true</c> if grabbed; otherwise, <c>false</c>.
    /// </value>
    public bool Grabbed { get; private set; } = false;

    /// <summary>
    /// The Awake method is called when the script instance is being loaded (Unity Callback).
    /// In this method, the rigidbody component is initialized.
    /// </summary>
    private void Awake()
    {
        rB = GetComponent<Rigidbody>();

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>(), true);
        }
    }

    /// <summary>
    /// The OnCollisionEnter Method is called when this collider/rigidbody has begun touching another rigidbody/collider (Unity Callback).
    /// </summary>
    /// <remarks>
    /// In this method, after the product was thrown its layer is reset to Default, 
    /// to be rendered by the main camera instead of the camera that renders 
    /// the product grabbed by the player.
    /// After that, the wasThrown flag is set to false.
    /// 
    /// This method also checks if the item is thrown and collided with a customer. 
    /// If these conditions are met the attack event is triggered (called in the DispatchEvents method).
    /// 
    /// It also checks if the item collided with a manager, in which case the manager dispatch a strike to the player.
    /// </remarks>
    /// <param name="collision">The collision.</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (thrown)
        {
            if (collision.gameObject.CompareTag("Customer"))
            {   

                GameObject customer = collision.gameObject;

                CustomerMovement customerMovement;

                CustomerSanity customerSanity = customer.GetComponent<CustomerSanity>() != null ? customer.GetComponent<CustomerSanity>() : customer.transform.parent.GetComponent<CustomerSanity>();
                customerSanity.DecreasedSanity();

                customerMovement = customer.GetComponent<CustomerMovement>() != null ?  customer.GetComponent<CustomerMovement>() : customer.transform.parent.GetComponent<CustomerMovement>();
                customerMovement.WasAttacked = true;

                EventManager.GetInstance().LastCustomerAttacked = customerMovement.gameObject;
            }
            else if (collision.gameObject.CompareTag("Manager"))
            {   
                GameObject manager = collision.gameObject;

                if (manager.GetComponent<ManagerMovement>() != null)
                {
                    manager.GetComponent<ManagerMovement>().WasAttacked = true;
                }
                else
                {   // The item collide with the manager collider to block entities colisions
                    manager.transform.parent.GetComponent<ManagerMovement>().WasAttacked = true;
                }
               
            }

            gameObject.layer = LayerMask.NameToLayer("Item");
            thrown = false;
        }
    }

    /// <summary>
    /// The WasGrabbed method is responsible for handling the logic when the item is grabbed by the player.
    /// </summary>
    /// <remarks>
    /// First the product collider is set to trigger and is rigibody to kinematic
    /// to avoid physics interactions with other objects.
    /// The item layer is changed to "GrabbedItem" because it prevents the item from being rendered by the main camera
    /// and instead renders it through a dedicated camera. This ensures the prodcut appears in front of
    /// any objects between the player and the item, solving rendering issues like z-fighting or clipping.
    /// </remarks>
    public virtual void WasGrabbed()
    {
        GetComponent<Collider>().isTrigger = true;

        Grabbed = true;

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
        Grabbed = false;

        GetComponent<Collider>().isTrigger = false;

        rB.isKinematic = false;

        const float THROWFORCE = 15f;

        rB.AddForce(playerFowardDir * THROWFORCE, ForceMode.Impulse);

        thrown = true;
    }
}