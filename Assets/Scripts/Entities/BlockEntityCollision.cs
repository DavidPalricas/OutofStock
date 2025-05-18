using UnityEngine;

/// <summary>
/// The BlockEntityCollision class is responsible for handling the collision between the entity collider and its blocker (extra collider).
/// </summary>
public class BlockEntityCollision : MonoBehaviour
{
    /// <summary>
    /// The entitiyCollider and entityColliderBlocker attributes are references to the CapsuleCollider components of the entity and its blocker respectively (extra collider).
    /// </summary>
    [SerializeField]
    private CapsuleCollider entityCollider, entityColliderBlocker;

    /// <summary>
    /// The Start method is called before the first frame update (Unity Callback).
    /// In this method, the collisions between the entity collider and its blocker are ignored.
    /// </summary>
    private void Start()
    {
        Physics.IgnoreCollision(entityCollider, entityColliderBlocker, true);
    }
}
