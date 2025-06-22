using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// The GrabAndThrowItems class is responsible for grabbing and throwing items.
/// </summary>
public class GrabAndThrowItems : MonoBehaviour
{
    /// <summary>
    /// The player attribute is the player GameObject.
    /// </summary>
    [SerializeField]
    private GameObject player;

    /// <summary>
    /// The crosshair attribute is the crosshair RectTransform.
    /// </summary>
    public RectTransform crosshair;

    /// <summary>
    /// The itemGrabbedPos attribute is the position where the item grabbed will be placed.
    /// </summary>
    [SerializeField]
    private Transform itemGrabbedPos;

    /// <summary>
    /// The grabOrThrowAction attribute is the reference to the grab or throw action.
    /// </summary>
    [SerializeField]
    private InputActionReference throwAction, grabAction;

    /// <summary>
    /// The itemGrabbed attribute is a flag that indicates if the player is holding an item.
    /// </summary>
    private bool itemGrabbed = false;

    private const float RAYCASTDISTANCE = 2f;

    /// <summary>
    /// The Update Method is called once per frame (Unity Callback).
    /// In this method, we check if the player pressed the grab or throw action.
    /// If this action is pressed, the GrabOrThrowItem method is called to check if the player will grab or throw an item.
    private void Update()
    {
        if (grabAction.action.IsPressed() && !itemGrabbed)
        {
             GrabItem();

             return;
        }

        if (throwAction.action.triggered &&  itemGrabbed){

            ThrowItem();

        }
    }

    /// <summary>
    /// The GrabItem Method is responsible for grabbing an item.
    /// </summary>
    /// <remarks>
    /// This method works by:
    /// 1. Casting a ray from the player's crosshair position to detect interactive items
    /// 2. If a "Item" is hit, the following occurs:
    ///    - The item's grab logic is triggered by calling the WasGrabbed method
    ///    - The item is positioned at the itemGrabbedPos and becomes its child
    ///    - The item's rotation is adjusted to face the player
    ///    - The itemGrabbed flag is set to true
    /// </remarks>
    private void GrabItem()
    {
        Ray ray = Utils.CastRayFromUI(crosshair);

        if (Physics.Raycast(ray, out RaycastHit playerRaycast, RAYCASTDISTANCE, LayerMask.GetMask("Item")))
        {   
            if (playerRaycast.collider.CompareTag("Item"))
            {
                GameObject item = playerRaycast.collider.gameObject;

                Item itemLogic = playerRaycast.collider.GetComponent<Item>();

                itemLogic.WasGrabbed();

                Transform itemTransform = item.transform;

                Quaternion newRotation = Quaternion.Euler(0, 0, 0);

                itemTransform.SetPositionAndRotation(itemGrabbedPos.position, newRotation);

                itemTransform.SetParent(itemGrabbedPos);
          
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
    /// 4. Positioning the item at the crosshair trajectory origin plus a slight offset, to the item being thrown in front of the player
    /// 5. The item's throw logic is triggered by calling the WasThrown method
    /// 6. Resetting the itemGrabbed flag
    /// 
    /// The throw force is set to a constant value of 20f and uses ForceMode.Impulse for
    /// immediate application of the force.
    /// </remarks>
    private void ThrowItem()
    {
        GameObject itemToThrow = itemGrabbedPos.GetChild(0).gameObject;
        StopClipping(itemToThrow);
        itemToThrow.transform.SetParent(null);

        Ray crosshairRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        itemToThrow.GetComponent<Item>().WasThrown(crosshairRay.direction);       
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

    /// <summary>
    /// The ProductToPlace method is responsible for determining which product placeholder the player is aiming at.
    /// </summary>
    /// <remarks>
    /// This method works by casting a ray from the player's crosshair position and checking if it hits a product placeholder.
    /// </remarks>
    /// <param name="productsPlaceHolder">The products place holder.</param>
    /// <returns>The products place holder that the player is aiming or null if is aiming to none</returns>
    public GameObject ProductToPlace(GameObject[] productsPlaceHolder)
    {
        Ray ray = Utils.CastRayFromUI(crosshair);

        if (Physics.Raycast(ray, out RaycastHit hit, RAYCASTDISTANCE, LayerMask.GetMask("Item")))
        {
            if (!hit.collider.gameObject.name.Contains("PlaceHolder"))
            {
                return null;
            }

            foreach (GameObject productPlaceHolder in productsPlaceHolder)
            {
                if (hit.collider.gameObject == productPlaceHolder)
                {
                    return productPlaceHolder;
                }
            }
        }

        return null;
    }
}
