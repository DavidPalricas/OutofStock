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
    /// The grabORThrowAction attribute is the reference to the grab or throw action.
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
    /// If this action is pressed, the ThrowItem method is called if the player is holding an item, otherwise, the GrabItem method is called.
    void Update()
    {
        if (grabOrThrowAction.action.triggered)
        {
            if (itemGrabbed)
            {
                // ThrowItem();
            }
            else
            {
                GrabItem();
            }
        }
    }

    /// <summary>
    /// The GrabItem Method is responsible for grabbing an item.
    /// To see if the player is looking at an item, a raycast is created in front of the player.
    /// If the raycast hits a pickup item, we set the item's position to the itemGrabbedPos position and set the item as his child.
    /// </summary>
    private void GrabItem()
    {
        const float RAYCASTDISTANCE = 5f;

        if (Physics.Raycast(player.transform.position, player.transform.forward, out RaycastHit playerRaycast, RAYCASTDISTANCE) )
        {
            if (playerRaycast.collider.CompareTag("PickUpItem"))
            {
                playerRaycast.collider.transform.SetPositionAndRotation(itemGrabbedPos.position, itemGrabbedPos.rotation);
                playerRaycast.collider.transform.SetParent(itemGrabbedPos);
                itemGrabbed = true;
            }
        }
    }
}
