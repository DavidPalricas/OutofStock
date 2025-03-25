using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// The PlayerActions class is responsible for handling the player's actions.
/// </summary>
public class PlayerActions : MonoBehaviour
{
    /// <summary>
    /// The player attribute is the player GameObject.
    /// </summary>
    [SerializeField]
    private GameObject player;

    /// <summary>
    /// The itemGrabbedPos attribute is the position where the item grabbed will be placed.
    /// </summary>
    [SerializeField]
    private Transform itemGrabbedPos;

    /// <summary>
    /// The grabOrThrowAction attribute is the reference to the grab or throw action.
    /// </summary>
    [SerializeField]
    private InputActionReference grabOrThrowAction;

    /// <summary>
    /// The itemGrabbed attribute is a flag that indicates if the player is holding an item.
    /// </summary>
    private bool itemGrabbed = false;

    /// <summary>
    /// The Update Method is called once per frame (Unity Callback).
    /// In this method, we check if the player pressed the grab or throw action.
    /// If this action is pressed, the GrabOrThrowItem method is called to check if the player will grab or throw an item.
    private void Update()
    {
        if (grabOrThrowAction.action.triggered)
        {
           GrabOrThrowItem();
        }
    }

    /// <summary>
    /// The GrabOrThrowItem Method is responsible for grabbing or throwing an item.
    /// In this method, we check if the player is holding an item.
    /// If the player is holding an item, the ThrowItem method is called otherwise the GrabItem method is called.
    /// </summary>
    private void GrabOrThrowItem()
    {
        if (itemGrabbed)
        {
            ThrowItem();

            return;
        }

        GrabItem();
    }

    /// <summary>
    /// The GrabItem Method is responsible for grabbing an item.
    /// </summary>
    /// <remarks>
    /// This method works by:
    /// 1. Casting a ray from the player's position in their forward direction to detect interactive items
    /// 2. If a "PickUpItem" is hit, the following occurs:
    ///    - The item's Rigidbody is set to kinematic to disable physics
    ///    - The item is positioned at the itemGrabbedPos and becomes its child
    ///    - The item's layer is changed to "GrabbedItem"
    ///    - The itemGrabbed flag is set to true
    /// 
    /// The layer change is significant: it prevents the item from being rendered by the main camera
    /// and instead renders it through a dedicated camera. This ensures the item appears in front of
    /// any objects between the player and the item, solving rendering issues like z-fighting or clipping.
    /// </remarks>
    private void GrabItem()
    {
        const float RAYCASTDISTANCE = 5f;

        if (Physics.Raycast(player.transform.position, player.transform.forward, out RaycastHit playerRaycast, RAYCASTDISTANCE) )
        {   
            if (playerRaycast.collider.CompareTag("PickUpItem"))
            {   

                Debug.Log("Item Grabbed");
                playerRaycast.collider.isTrigger = true;

                GameObject item = playerRaycast.collider.gameObject;

                Rigidbody itemRb = item.GetComponent<Rigidbody>();

                if (!itemRb.isKinematic)
                {
                    itemRb.isKinematic = true;
                }

                Transform itemTransform = item.transform;

                itemTransform.SetPositionAndRotation(itemGrabbedPos.position, itemGrabbedPos.rotation);

                itemTransform.SetParent(itemGrabbedPos);
                item.layer = LayerMask.NameToLayer("GrabbedItem");
                itemGrabbed = true;
            }
        }
    }

    /// <summary>
    /// The ThrowItem Method is responsible for throwing the item that the player is holding.
    /// </summary>
    /// <remarks>
    /// This method works by:
    /// 1. Retrieving the held item (child of itemGrabbedPos)
    /// 2. Preventing clipping by calling the StopClipping method
    /// 3. Detaching the item from the player by setting its parent to null
    /// 4. Enabling physics by setting isKinematic to false
    /// 5. Applying an impulse force in the player's forward direction
    /// 6. Resetting the itemGrabbed flag
    /// 
    /// The throw force is set to a constant value of 20f and uses ForceMode.Impulse for
    /// immediate application of the force.
    /// </remarks>
    private void ThrowItem()
    {
        GameObject itemToThrow = itemGrabbedPos.GetChild(0).gameObject;

        itemToThrow.GetComponent<Collider>().isTrigger = false;

        StopClipping(itemToThrow);

        itemToThrow.transform.SetParent(null);

        Rigidbody itemRigidbody = itemToThrow.GetComponent<Rigidbody>();

        itemRigidbody.isKinematic = false;

        const float THROWFORCE = 20f;

        itemRigidbody.AddForce(player.transform.forward * THROWFORCE, ForceMode.Impulse);

        itemGrabbed = false;
    }

    /// <summary>
    /// Prevents object clipping by repositioning the item before throwing.
    /// This method performs collision detection along the throwing trajectory to ensure
    /// the item doesn't intersect with walls, obstacles, or other objects in the scene.
    /// </summary>
    /// <remarks>
    /// The method works by:
    /// 1. Casting rays to detect potential collisions along the throw path
    /// 2. Calculating a safe position before any detected obstacle
    /// 3. Adding a slight lateral offset to avoid direct collisions
    /// 4. Verifying the new position is clear using a sphere check
    /// 5. If all else fails, positioning the object above the player
    /// </remarks>
    /// <param name="itemToThrow">The game object that will be thrown.</param>
    private void StopClipping(GameObject itemToThrow)
    {
        var clipRange = Vector3.Distance(itemToThrow.transform.position, transform.position);

        Collider itemCollider = itemToThrow.GetComponent<Collider>();

        // Pre-allocate array to avoid memory allocation
        RaycastHit[] hits = new RaycastHit[10]; 
        int hitCount = Physics.RaycastNonAlloc(transform.position, transform.TransformDirection(Vector3.forward),
                                              hits, clipRange, ~LayerMask.GetMask("Player"));

        bool potentialClipping = false;
        Vector3 newPosition = transform.position;

        for (int i = 0; i < hitCount; i++)
        {
            if (hits[i].collider == itemCollider || hits[i].collider.gameObject.CompareTag("Player"))
            {
                continue;
            }

            potentialClipping = true;

            float safeDistance = hits[i].distance * 0.9f; 
            newPosition = transform.position + transform.TransformDirection(Vector3.forward) * safeDistance;

            newPosition += transform.TransformDirection(Vector3.right) * 0.3f;

            break; 
        }

        if (potentialClipping)
        {
            if (!Physics.CheckSphere(newPosition, itemCollider.bounds.extents.magnitude * 0.8f,~LayerMask.GetMask("Player")))
            {
                itemToThrow.transform.position = newPosition;
            }
            else
            {

                itemToThrow.transform.position = transform.position + transform.TransformDirection(Vector3.up) * 1.5f;
            }
        }
    }
}
