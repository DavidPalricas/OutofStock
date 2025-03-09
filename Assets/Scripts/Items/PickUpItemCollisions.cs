using UnityEngine;

/// <summary>
///  The PickUpItemCollisions class is responsible for handling the collisions of the pick up itens.
/// </summary>
public class PickUpItemCollisions : MonoBehaviour
{
    /// <summary>
    /// The OnCollisionEnter Method is called when this collider/rigidbody has begun touching another rigidbody/collider (Unity Callback).
    /// In this method, after the item collided its layer is changed to Default, to be rendered by th main camera instead of the camera that renders the item grabbed by the player.
    /// </summary>
    /// <param name="collision">The collision.</param>
    private void OnCollisionEnter(Collision collision)
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
