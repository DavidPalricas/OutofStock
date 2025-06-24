using UnityEngine;

/// <summary>
/// The ManagerState class is responsible for representing the base state of the supermarket's manager.
/// </summary>
public class ManagerState : State
{
    /// <summary>
    /// The movement atribute stores a reference to the ManagerMovement component, which is responsible for the manager's movement logic.
    /// </summary>
    protected ManagerMovement movement;

    /// <summary>
    /// The timeToWait and timer attributes are used to store the time the manager should wait before transitioning to another state and the current timer value, respectively.
    /// </summary>
    protected float timetoWait, timer;

    /// <summary>
    /// The Awake method is called when the script instance is being loaded (Unity Callback).
    /// In this method, the movement and fSM attributes are initialized by getting the respective components from the GameObject this script is attached to.
    /// </summary>
    protected virtual void Awake()
    {
        movement = GetComponent<ManagerMovement>();
        fSM = GetComponent<FSM>();
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// The StayOnPoint method is responsible for keeping the manager on a specific point for a certain amount of time before transitioning to another state.
    /// </summary>
    /// <param name="transitionName">Name of the transition to another state.</param>
    protected void StayOnPoint(string transitionName){
        if (timer == 0f)
        {
            timer = Time.time + timetoWait;
            animator.SetFloat("Speed", 0f);
        }
        else if (timer <= Time.time)
        {
            fSM.ChangeState(transitionName);
        }
    }
}
