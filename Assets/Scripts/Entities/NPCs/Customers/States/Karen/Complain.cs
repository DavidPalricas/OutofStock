using System.Linq;
using UnityEngine;

/// <summary>
/// The Complain class is responsible for handling the complaining state of the Karen customer in the market.
/// This state is exclusively for the Karen Customer Stereotype.
/// </summary>
public class Complain : CustomerBaseState
{
    /// <summary>
    /// The karenMovement attribute is a reference to the KarenMovement component.
    /// </summary>
    private KarenMovement karenMovement;

    private float timer, complainingCooldown;

    /// <summary>
    /// The Awake Method is called when the script instance is being loaded (Unity Callback).
    /// It calls the base class Awake method and sets the stateName to the name of the current class.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        StateName = GetType().Name;


        RuntimeAnimatorController controller = animator.runtimeAnimatorController;

        if (controller != null)
        {
            AnimationClip[] clips = controller.animationClips;

            foreach (AnimationClip clip in clips)
            {
                Debug.Log("Animation Clip: " + clip.name);
            }
        }

        complainingCooldown = animator.runtimeAnimatorController.animationClips.ToList().Find(x => x.name.ToLower() == "complain").length;
    }

    /// <summary>
    /// The Enter method is called when the state is entered.
    /// It calls the base class Enter method and sets the timer to the current time plus the complainingCooldown and sets
    /// the karenMovement reference to its associated attribute.
    /// </summary>
    private AudioManager audioManager;

public override void Enter()
{
    base.Enter();
    timer = Time.time + complainingCooldown;

    if (customerMovement is KarenMovement movement)
    {
        karenMovement = movement;
     
        animator.SetBool("isComplain", true);
        animator.SetFloat("Speed", 0f);

    }

    audioManager = FindFirstObjectByType<AudioManager>();
}

    public void ComplaintSFX(Vector3 position)
    {
        audioManager?.PlayKarenComplaint(position);
        Debug.DrawRay(position, Vector3.up * 2f, Color.red, 2f);
}


    /// <summary>
    /// The Execute method is called when the state is executed, to perform the actions of the state.
    /// It calls the base class Execute method.
    /// </summary>
    /// <remarks>
    /// This methods checks for possible conditions to change the state, otherwise it continues the states actions.
    /// The possible transitions are:
    ///   1. PlayerNotInRange: If the player is not in range, the state changes to Chase Player State.
    ///   2. ComplainedToMuch: If the Karen has complained too many times, the state changes to Attack Player State.
    ///   3. Attacked: If the Karen is attacked, the state changes to Knocked state.
    ///   
    /// If none of these conditions are met, the Karen will complain to the player, if the cooldown time has passed.
    /// </remarks>
    public override void Execute()
{
        if (!karenMovement.PlayerInRange())
        {
            fSM.ChangeState("PlayerNotInRange");
            return;
        }

        if (karenMovement.WasAttacked)
        {
            fSM.ChangeState("Attacked");
            return;
        }

        if (Time.time >= timer)
        {
            fSM.ChangeState("ComplainedToMuch");
            return;
        }
    }

    // Time to complain again?
    if (Time.time >= timer)
    {
        ComplaintSFX(transform.position);  // Play sound
        complainingCounter++;
        timer = Time.time + complainingCooldown;  // reset cooldown
    }
}


    /// <summary>
    /// The Exit method is called when the state is exited, to handle its final actions.
    /// It calls the base class Exit method.
    /// </summary>
    public override void Exit()
    {
        base.Exit();
        animator.SetBool("isComplain", false);
    }

    /// <summary>
    /// The Complaining method is called when the Karen complains to the player and increments the number of complaints.
    /// </summary>
    /// <remarks>
    /// The logic of this method is not implemented yet.
    /// </remarks>
    public void ComplaintSFX()
    {   
        FindFirstObjectByType<AudioManager>().PlayKarenComplaint(transform.position);
    }
}