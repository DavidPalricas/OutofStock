using UnityEngine;

public class Item : MonoBehaviour
{   
    protected Rigidbody rB;

    protected bool thrown = false;

    public bool Grabbed { get; private set; } = false;

    protected virtual void Awake()
    {
        rB = GetComponent<Rigidbody>();
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (thrown)
        {
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
