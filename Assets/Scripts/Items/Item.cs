using UnityEngine;

/// <summary>
/// The Item class is responsible for representing an item in the supermarket.
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

    private Vector3 originalPos;

    private bool positionChange = false;
    /// <summary>
    /// The Awake method is called when the script instance is being loaded (Unity Callback).
    /// In this method, the rigidbody component is initialized.
    /// </summary>
    protected virtual void Awake()
    {
        rB = GetComponent<Rigidbody>();
        originalPos = transform.position;

    }

    private void Update()
    {
        if (originalPos != transform.position)
        {
            positionChange = true;
            rB.isKinematic = false;

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
    /// </remarks>
    /// <param name="collision">The collision.</param>
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (thrown)
        {
            gameObject.layer = LayerMask.NameToLayer("Item");
            thrown = false;
        }

        if (positionChange)
        {
            rB.isKinematic = true;
            positionChange = false;
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
    public void WasGrabbed()
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

        const float THROWFORCE = 20f;

        rB.AddForce(playerFowardDir * THROWFORCE, ForceMode.Impulse);

        thrown = true;
    }
}
