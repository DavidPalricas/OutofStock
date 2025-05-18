using UnityEngine;
using UnityEngine.InputSystem;

public class KnockEntity : MonoBehaviour 
{
    public void Knock(GameObject entity, Rigidbody rb, Vector3 entityPosBeforeKnock)
    {
        if (entity.CompareTag("Customer"))
        {
            entity.GetComponent<CustomerMovement>().EnableOrDisanableAgent(false);
        }

        rb.isKinematic = true;

        entity.transform.SetPositionAndRotation(new Vector3(entityPosBeforeKnock.x, 0.5f, entityPosBeforeKnock.z), Quaternion.Euler(90f, 0, 0));

        if (entity.CompareTag("Player"))
        {
            entity.GetComponent<PlayerInput>().enabled = false;

            StartCoroutine(Utils.WaitAndExecute(2f, () => StandUp(entity, rb, entityPosBeforeKnock.y))); 
        }
    }

    public void StandUp(GameObject entity, Rigidbody rb, float entityPosYBeforeKnock)
    {
        rb.isKinematic = false;

        Transform entityTransform = entity.transform;

        entityTransform.rotation = Quaternion.identity;

        Vector3 entityPos = entityTransform.position;
        entityTransform.position = new Vector3(entityPos.x, entityPosYBeforeKnock, entityPos.z);

        if (entity.CompareTag("Player"))
        {
            entity.GetComponent<PlayerInput>().enabled = true;
            return;
        }

        if (entity.CompareTag("Customer"))
        {
            entity.GetComponent<CustomerMovement>().EnableOrDisanableAgent(true);
        }
    }
}

