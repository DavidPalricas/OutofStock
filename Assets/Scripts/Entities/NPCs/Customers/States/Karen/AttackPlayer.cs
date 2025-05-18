using UnityEngine;

/// <summary>
/// The AttackPlayer class is responsible for handling the attack player state of the Karen.
/// This state is exclusively for the Karen Customer Stereotype.
/// </summary>
public class AttackPlayer : CustomerBaseState
{
    /// <summary>
    /// The maxAttacks attribute is the maximum number of attacks that the Karen can handle.
    /// </summary>
    [SerializeField]
    private int maxAttacks;

    /// <summary>
    /// The attackCooldown attribute is the cooldown time between attacks.
    /// </summary>
    [SerializeField]
    private float attackCooldown;

    /// <summary>
    /// The karenMovement attribute is a reference to the KarenMovement component.
    /// </summary>
    private KarenMovement karenMovement;

    /// <summary>
    /// The timer attribute is used to count the time that the Karen will take to attack the player.
    /// </summary>
    private float timer;

    /// <summary>
    /// The attacked attribute is used to count the number of times the Karen has been attacked by the player.
    /// </summary>
    private int attacked;

    /// <summary>
    /// The Awake Method is called when the script instance is being loaded (Unity Callback).
    /// It calls the base class Awake method and sets the stateName to the name of the current class.
    /// </summary>

    protected override void Awake()
    {
        base.Awake();
        stateName = GetType().Name;
    }

    /// <summary>
    /// The Enter method is called when the state is entered.
    /// It calls the base class Enter method and sets the timer to the current time plus the attackCooldown and 
    /// sets the KarenMovement component to the karenMovement attribute.
    /// </summary>
    public override void Enter()
    {
        base.Enter();

        timer = Time.time + attackCooldown;

        if (customerMovement is KarenMovement movement)
        {
            karenMovement = movement;
        }
    }

    /// <summary>
    /// The Execute method is called when the state is executed, to perform the actions of the state.
    /// It calls the base class Execute method.
    /// <remarks>
    /// This methods checks for possible conditions to change the state, otherwise it continues the states actions.
    /// The possible transitions are:
    ///    1. Attacked Transition: If the Karen is attacked, its attacked counter is incremented and it changes to the Knocked state.
    ///    2. PlayerNotInRange Transition: If the player is not in range, it changes to the Chase Player state.
    ///    3. AttackedToManyTimes Transition: If the Karen has been attacked too many times, it changes to the Go Home state.
    ///    
    /// If the none of these conditions are met, this method checks if the attack cooldown has passed, if so, it attacks the player.
    /// </remarks>
    /// </summary>

    public override void Execute()
    {
        base.Execute();

        if (karenMovement.WasAttacked)
        {
            fSM.ChangeState("Attacked");
            attacked++;
            return;
        }

        if (!karenMovement.PlayerInRange())
        {
            fSM.ChangeState("PlayerNotInRange");
            return;
        }

        if (attacked >= maxAttacks)
        {
            fSM.ChangeState("AttackedToManyTimes");
            return;
        }

        if (Time.time >= timer)
        {
            Attack();
            timer = Time.time + attackCooldown;
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
    /// The Attack method is called to perform the attack action on the player.
    /// It calls the Knock method from the KnockEntity component to knock the player down (the player proprieties are passed as parameters of the method).
    /// </summary>
    private void Attack()
    {        
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        GetComponent<KnockEntity>().Knock(player, player.GetComponent<Rigidbody>(), player.transform.position);
    }
}