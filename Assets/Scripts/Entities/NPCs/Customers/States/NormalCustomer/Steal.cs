using System.Linq;
using UnityEngine;

/// <summary>
/// The Steal class is responsible for handling the stealing state of the customer.
/// It is exclusively for the Normal Customer Stereotype, if it becomes a thief.
/// </summary>
public class Steal : CustomerBaseState
{
    private float timer, timeToSteal;


    /// <summary>
    /// The Awake Method is called when the script instance is being loaded (Unity Callback).
    /// It calls the base class Awake method and sets the stateName to the name of the current class.
    /// </summary>
    
    private AudioManager audioManager;

    protected override void Awake()
    {
        base.Awake();
        StateName = GetType().Name;
        
        audioManager = FindFirstObjectByType<AudioManager>();
        timer = 0f;
        timeToSteal = animator.runtimeAnimatorController.animationClips.ToList().Find(x => x.name.ToLower() == "pickupobject").length;
    }

    /// <summary>
    /// The Enter method is called when the state is entered.
    /// It calls the base class Enter method and sets the thief destination to the product area.
    /// </summary>
    public override void Enter()
    {
        base.Enter();
        customerMovement.SetAgentDestination(customerMovement.AreasPos["Product"]);

         audioManager?.PlayAlarmSound(transform.position);
        animator.SetFloat("Speed", 1f);
    }

    /// <summary>
    /// The Execute method is called when the state is executed, to perform the actions of the state.
    /// It calls the base class Execute method.
    /// <remarks>
    /// This methods checks for possible conditions to change the state, otherwise it continues the states actions.
    /// The possible transitions are:
    ///    1. Attacked Transition: If the thief is attacked, it changes to the Knocked state.
    ///    2. ProductStealed Transition: Calls the StealProduct method when the customer reaches its destination to handle the stealing process and then changes to the Go Home state.
    ///    
    /// If the none of these conditions are met, nothing happens until the thief reaches its destination or is attacked.
    /// </remarks>
    /// </summary>
    public override void Execute()
    {
        base.Execute();

        if (customerMovement.WasAttacked)
        {
            audioManager?.StopAlarmSound();
            fSM.ChangeState("Attacked");
        }

        if (customerMovement.DestinationReached)
        {   
            if (timer == 0f)
            {  
                timer = Time.time + timeToSteal;
                animator.SetTrigger("pickUpItem");
            }
            else if (Time.time >= timer)
            {
                fSM.ChangeState("ProductStealed");
            }
        }
    }

    /// <summary>
    /// The Exit method is called when the state is exited, to handle its final actions.
    /// It calls the base class Exit method.
    /// </summary>
    public override void Exit()
    {
        base.Exit();

        audioManager?.StopAlarmSound();
    }
}
