using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class KnockEntity : MonoBehaviour
{
    private readonly float fallingAnimDuration = 0.5f;
    private readonly float standUpAnimDuration = 0.8f;

    private readonly float timeOnGround = 1.5f;

    private readonly float knockBounceHeight = 0.2f;

    private readonly float fallRotationSpeed = 360f;

    private Vector3 originalScale;

    private IEnumerator AnimateFall(GameObject entity, Rigidbody rb, Vector3 entityPosBeforeKnock)
    {
        Transform entityTransform = entity.transform;
        entityTransform.GetPositionAndRotation(out Vector3 startPosition, out _);
        float elapsedTime = 0f;

        while (elapsedTime < fallingAnimDuration)
        {
            float t = elapsedTime / fallingAnimDuration;

            float verticalOffset = Mathf.Sin(t * Mathf.PI) * knockBounceHeight;
            float fallProgress = Mathf.SmoothStep(0, 1, t);

            Vector3 newPosition = Vector3.Lerp(
                startPosition,
                new Vector3(entityPosBeforeKnock.x, 0.5f, entityPosBeforeKnock.z),
                fallProgress
            );

            newPosition.y += verticalOffset;

            float rotationProgress = fallRotationSpeed * elapsedTime;
            Quaternion newRotation = Quaternion.Euler(
                Mathf.Min(90f, rotationProgress),
                entityTransform.rotation.eulerAngles.y,
                0
            );

            entityTransform.SetPositionAndRotation(newPosition, newRotation);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        entityTransform.SetPositionAndRotation(
            new Vector3(entityPosBeforeKnock.x, 0.5f, entityPosBeforeKnock.z),
            Quaternion.Euler(90f, entityTransform.rotation.eulerAngles.y, 0)
        );

        yield return new WaitForSeconds(timeOnGround);

        StartCoroutine(AnimateStandUp(entity, rb, entityPosBeforeKnock.y));
    }

    private IEnumerator AnimateStandUp(GameObject entity, Rigidbody rb, float entityPosYBeforeKnock)
    {
        Transform entityTransform = entity.transform;
        entityTransform.GetPositionAndRotation(out Vector3 startPosition, out Quaternion startRotation);
        var endPosition = new Vector3(startPosition.x, entityPosYBeforeKnock, startPosition.z);
        Quaternion endRotation = Quaternion.identity;

        float elapsedTime = 0f;

        while (elapsedTime < standUpAnimDuration)
        {
            float t = elapsedTime / standUpAnimDuration;
            float smoothT = Mathf.SmoothStep(0, 1, t);

            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, smoothT);
            Quaternion newRotation = Quaternion.Slerp(startRotation, endRotation, smoothT);

            float scaleY = 1.0f + 0.2f * Mathf.Sin(t * Mathf.PI);
            float scaleXZ = 1.0f - 0.1f * Mathf.Sin(t * Mathf.PI);
            entityTransform.localScale = new Vector3(
                originalScale.x * scaleXZ,
                originalScale.y * scaleY,
                originalScale.z * scaleXZ
            );

            entityTransform.SetPositionAndRotation(newPosition, newRotation);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        entityTransform.SetPositionAndRotation(endPosition, endRotation);
        entityTransform.localScale = originalScale;

        rb.isKinematic = false;

        if (entity.CompareTag("Player"))
        {
            entity.GetComponent<PlayerInput>().enabled = true;
        }

        if (entity.CompareTag("Customer"))
        {
            entity.GetComponent<CustomerMovement>().EnableOrDisanableAgent(true);

            entity.GetComponent<FSM>().ChangeState("StandUp");
        }
    }


    public void Knock(GameObject entity, Rigidbody rb, Vector3 entityPosBeforeKnock)
    {   
        originalScale = entity.transform.localScale;

        if (entity.CompareTag("Customer"))
        {
            entity.GetComponent<CustomerMovement>().EnableOrDisanableAgent(false);
        }

        rb.isKinematic = true;

        if (entity.CompareTag("Player"))
        {
            entity.GetComponent<PlayerInput>().enabled = false;
        }

        StartCoroutine(AnimateFall(entity, rb, entityPosBeforeKnock));
    }
}