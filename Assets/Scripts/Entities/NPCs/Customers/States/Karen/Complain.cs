using UnityEngine;

/// <summary>
/// The Complain class is responsible for handling the complaining state of the Karen customer in the market.
/// This state is exclusively for the Karen Customer Stereotype.
/// </summary>
public class Complain : CustomerBaseState
{
    /// <summary>
    /// The maxComplaining attribute is the maximum number of times that the Karen can complain before she gets angry and attacks the player.
    /// </summary>
    [SerializeField]
    private int maxComplaining;

    /// <summary>
    /// The complainingCooldown attribute is the time that the Karen will take to complain again.
    /// </summary>
    [SerializeField]
    private float complainingCooldown;

    /// <summary>
    /// The karenMovement attribute is a reference to the KarenMovement component.
    /// </summary>
    private KarenMovement karenMovement;

    /// <summary>
    /// The complainingCounter attribute is used to count the number of times that the Karen has complained.
    /// </summary>
    private int complainingCounter = 0;

    /// <summary>
    /// The timer attribute is used to count the time that the Karen will take to complain again.
    /// </summary>
    private float timer;

    /// <summary>
    /// The Awake Method is called when the script instance is being loaded (Unity Callback).
    /// It calls the base class Awake method and sets the stateName to the name of the current class.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        StateName = GetType().Name;
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
    base.Execute();

    if (!karenMovement.PlayerInRange())
    {
        fSM.ChangeState("PlayerNotInRange");
        return;
    }

    if (complainingCounter >= maxComplaining)
    {
        fSM.ChangeState("ComplainedToMuch");
        return;
    }

    if (karenMovement.WasAttacked)
    {
        fSM.ChangeState("Attacked");
        return;
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
    }

    /// <summary>
    /// The Complaining method is called when the Karen complains to the player and increments the number of complaints.
    /// </summary>
    /// <remarks>
    /// The logic of this method is not implemented yet.
    /// </remarks>

}