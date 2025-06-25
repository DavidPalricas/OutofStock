using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The KnockedState class is responsible for handling the knocked state of the NPCs (customers and Manager).
/// This state is implemented for all customer stereotypes.
/// </summary>
public class KnockedState : State
{
    /// <summary>
    /// The Awake Method is called when the script instance is being loaded (Unity Callback).
    //  It gets the name of the current class and sets it to the StateName property.
    /// </summary>

    private float timer  = 0f, standUpAnimationTime, knockAnimationTime;

    private void Awake()
    {
        StateName = GetType().Name;
        animator = GetComponent<Animator>();


        List<AnimationClip> clips = animator.runtimeAnimatorController.animationClips.ToList();

        standUpAnimationTime = clips.Find(x => x.name.ToLower() == "gettingup").length;
        knockAnimationTime = clips.Find(x => x.name.ToLower().Contains("fallingdown")).length;
    }


    /// <summary>  
    /// The Enter method is called when the state is entered.  
    /// It calls the base class Enter method and calls the Knock method from the KnockEntity component to knock the customer down.  
    /// Inside this method after knocking out the customer and it stands up, its state will change to the AttackPlayer state if the customer is a Karen, otherwise it will change to the GoHome state.  
    /// </summary>  
    public override void Enter()
    {
        base.Enter();

        if (gameObject.CompareTag("Manager"))
        {
            GetComponent<StrikesSystem>().DispatchStrike(true);
        }

        animator.SetTrigger("Hit");

        GetComponent<Rigidbody>().isKinematic = true;

        timer = Time.time + knockAnimationTime + standUpAnimationTime;

        if (gameObject.CompareTag("Manager"))
        {
            timer += animator.runtimeAnimatorController.animationClips.ToList().Find(x => x.name.ToLower() == "complain").length;
        }

        var customerMovement = GetComponent<CustomerMovement>();
        if (customerMovement != null && customerMovement.backPack != null)
        {
            customerMovement.backPack.gameObject.SetActive(false);
        }
    }

    // Commented out because this method requires a parameter we don't have here,
    // and it's already handled below more appropriately.
    // FindFirstObjectByType<AudioManager>().PlayImpactSFX(transform.position);


    // Optionally call it here if you know which item knocked the NPC:
    // PlayImpactSound(item);
    /// <summary>
    /// The Enter method is called when the state is entered.
    /// It calls the base class Execute method.
    /// </summary>
    public override void Execute()
    {
        base.Execute();

        if (Time.time >= timer)
        {   
            if (gameObject.CompareTag("Manager"))
            {
                ManagerMovement movement = GetComponent<ManagerMovement>();

                string transitionName = movement.IsPatrolling ? "ContinuePatrolling" : "ContinueGoingToOffice";

                GetComponent<FSM>().ChangeState(transitionName);

                return;
            }

            GetComponent<FSM>().ChangeState("StandUp");
        }
    }

    /// <summary>  
    /// The Exit method is called when the state is exited, to handle its final actions.  
    /// It calls the base class Exit method, sets the WasAttacked attribute to false and resets the LastCustomerAttacked attribute in the EventManager instance.  
    /// </summary>  
    public override void Exit()
    {
        base.Exit();
        GetComponent<NPCMovement>().WasAttacked = false;

        GetComponent<Rigidbody>().isKinematic = false;

        var customerMovement = GetComponent<CustomerMovement>();
        if (customerMovement != null && customerMovement.backPack != null)
        {
            customerMovement.backPack.gameObject.SetActive(true);
        }
    }
}
