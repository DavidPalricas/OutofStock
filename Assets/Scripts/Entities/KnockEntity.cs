using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

/// <summary>
/// The KnockEntity class is responsible for handling the knock and stand up animations of the attacked entities in the game (customers and player).
/// </summary>
public class KnockEntity : MonoBehaviour
{
    /// <summary>
    /// The fallingAnimDuration attribute is the duration of the falling animation.
    /// </summary>
    private readonly float fallingAnimDuration = 0.5f;

    /// <summary>
    /// The timeOnGround attribute is the time that the entity will stay on the ground before standing up.
    /// </summary>
    private readonly float timeOnGround = 1.5f;

    /// <summary>
    /// The knockBounceHeight attribute is the height of the bounce when the entity falls.
    /// </summary>
    private readonly float knockBounceHeight = 0.2f;

    /// <summary>
    /// The fallRotationSpeed attribute is the speed of the rotation when the entity falls.
    /// </summary>
    private readonly float fallRotationSpeed = 360f;

    /// <summary>
    /// The standUpAnimDuration attribute is the duration of the stand up animation.
    /// </summary>
    private float standUpAnimDuration = 2f;

    /// <summary>
    /// The originalScale attribute is the original scale of the entity before the knock animation.
    /// This atribute is used to restore the entity's scale after the stand up animation.
    /// </summary>
    private Vector3 originalScale;

    /// <summary>
    /// The AnimateFall method animates the entity falling to the ground with a bounce effect and rotation.
    /// </summary>
    /// <param name="entity">The GameObject of the entity to animate.</param>
    /// <param name="rb">The Rigidbody component of the entity.</param>
    /// <param name="entityPosBeforeKnock">The entity's position before being knocked down.</param>
    /// <returns>An IEnumerator for the coroutine system.</returns>
    private IEnumerator AnimateFall(GameObject entity, Rigidbody rb, Vector3 entityPosBeforeKnock)
    {
        Transform entityTransform = entity.transform;
        entityTransform.GetPositionAndRotation(out Vector3 startPosition, out _);
        float elapsedTime = 0f;

        while (elapsedTime < fallingAnimDuration)
        {
            // Calculate normalized time (0 to 1)
            float t = elapsedTime / fallingAnimDuration;

            // Create an arc motion with sine function for bounce effect
            float verticalOffset = Mathf.Sin(t * Mathf.PI) * knockBounceHeight;

            // Smooth the movement curve for natural fall
            float fallProgress = Mathf.SmoothStep(0, 1, t);

            // Interpolate from start position to ground position
            Vector3 newPosition = Vector3.Lerp(
                startPosition,
                new Vector3(entityPosBeforeKnock.x, 0.5f, entityPosBeforeKnock.z),
                fallProgress
            );

            // Add the bounce arc to the vertical position
            newPosition.y += verticalOffset;

            // Calculate rotation based on elapsed time and rotation speed
            float rotationProgress = fallRotationSpeed * elapsedTime;

            // Create rotation that gradually tips the entity to 90 degrees on X axis
            Quaternion newRotation = Quaternion.Euler(
                Mathf.Min(90f, rotationProgress),
                entityTransform.rotation.eulerAngles.y,
                0
            );

            // Sets the entiy knocked position and rotation
            entityTransform.SetPositionAndRotation(newPosition, newRotation);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final position and rotation are exact 
        entityTransform.SetPositionAndRotation(
            new Vector3(entityPosBeforeKnock.x, 0.5f, entityPosBeforeKnock.z),
            Quaternion.Euler(90f, entityTransform.rotation.eulerAngles.y, 0)
        );

        // Keep the entity on the ground for the configured time
        yield return new WaitForSeconds(timeOnGround);

        StartCoroutine(AnimateStandUp(entity, rb, entityPosBeforeKnock.y));
    }

    /// <summary>
    /// The AnimateStandUp method animates the entity standing up from a fallen position with a squash and stretch effect.
    /// </summary>
    /// <param name="entity">The GameObject of the entity to animate.</param>
    /// <param name="rb">The Rigidbody component of the entity.</param>
    /// <param name="entityPosYBeforeKnock">The original Y-position of the entity before being knocked down.</param>
    /// <returns>An IEnumerator for the coroutine system.</returns>
    private IEnumerator AnimateStandUp(GameObject entity, Rigidbody rb, float entityPosYBeforeKnock)
    {
        Transform entityTransform = entity.transform;
        entityTransform.GetPositionAndRotation(out Vector3 startPosition, out Quaternion startRotation);

        // Resetes the entity Y position to the original one
        var endPosition = new Vector3(startPosition.x, entityPosYBeforeKnock, startPosition.z);

        // Rests the entiy rotation to the original one
        Quaternion endRotation = Quaternion.identity;

        float elapsedTime = 0f;


        if (entity.CompareTag("Player"))
        {
            standUpAnimDuration = standUpAnimDuration / 2f;
        }

        while (elapsedTime < standUpAnimDuration)
        {
            // Calculate normalized time (0 to 1)
            float t = elapsedTime / standUpAnimDuration;

            // Smooth the movement curve for natural stand up motion
            float smoothT = Mathf.SmoothStep(0, 1, t);

            // Interpolate from fallen position to standing position
            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, smoothT);

            // Use spherical interpolation for rotation to get a smooth arc
            Quaternion newRotation = Quaternion.Slerp(startRotation, endRotation, smoothT);

            // Apply squash and stretch effect for visual emphasis
            // Entity stretches vertically while compressing horizontally
            float scaleY = 1.0f + 0.2f * Mathf.Sin(t * Mathf.PI);
            float scaleXZ = 1.0f - 0.1f * Mathf.Sin(t * Mathf.PI);

            // Apply scale modification while preserving original proportions
            entityTransform.localScale = new Vector3(
                originalScale.x * scaleXZ,
                originalScale.y * scaleY,
                originalScale.z * scaleXZ
            );

            // Set the entity's standing position and rotation
            entityTransform.SetPositionAndRotation(newPosition, newRotation);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final position, rotation and scale are exact
        entityTransform.SetPositionAndRotation(endPosition, endRotation);
        entityTransform.localScale = originalScale;

        // Restore physics control
        rb.isKinematic = false;

        if (entity.CompareTag("Player"))
        {
            entity.GetComponent<PlayerInput>().enabled = true;
            entity.GetComponent<FirstPersonController>().enabled = true;
        }
        else if (entity.CompareTag("Customer"))
        {
            entity.GetComponent<NPCMovement>().EnableOrDisableAgent(true);

            /* Changes the state of the customer to "StandUp" after is standing up animation to change for the next state
               If the customer is a Karen it will change to the Attack Player state, otherwise it will change to the Go Home state.
            */
            entity.GetComponent<FSM>().ChangeState("StandUp");
        }
        else if (entity.CompareTag("Manager"))
        {
            entity.GetComponent<NPCMovement>().EnableOrDisableAgent(true);

            bool isPatrolling = entity.GetComponent<ManagerMovement>().IsPatrolling;

            string transitionName = isPatrolling ? "ContinuePatrolling" : "ContinueGoingToOffice";

            entity.GetComponent<FSM>().ChangeState(transitionName);
        }
    }

    /// <summary>
    /// The Knock method knocks down the specified entity with a procedural animation.
    /// </summary>
    /// <param name="entity">The GameObject of the entity to knock down.</param>
    /// <param name="rb">The Rigidbody component of the entity.</param>
    /// <param name="entityPosBeforeKnock">The entity's position before being knocked down.</param>
    public void Knock(GameObject entity, Rigidbody rb, Vector3 entityPosBeforeKnock)
    {
        originalScale = entity.transform.localScale;

        if (entity.CompareTag("Customer") || entity.CompareTag("Manager"))
        {
            // Disabeles the navmesh agent of the customer to prevent it from moving
            entity.GetComponent<NPCMovement>().EnableOrDisableAgent(false);
        }

        // Make rigidbody kinematic to control movement through animation, to avoid physics interference
        rb.isKinematic = true;

        if (entity.CompareTag("Player"))
        {
            // Disables the player input and FirstPersonController to prevent the player from moving
            entity.GetComponent<PlayerInput>().enabled = false;
            entity.GetComponent<FirstPersonController>().enabled = false;

        }

        StartCoroutine(AnimateFall(entity, rb, entityPosBeforeKnock));
    }
}