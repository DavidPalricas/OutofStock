using UnityEngine;

public class BlockEntityCollision : MonoBehaviour
{
    [SerializeField]
    private CapsuleCollider entityCollider, entityColliderBlocker;

    private void Start()
    {
        Physics.IgnoreCollision(entityCollider, entityColliderBlocker, true);
    }
}
