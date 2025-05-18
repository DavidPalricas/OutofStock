/// <summary>
/// The ChasePlayer class is responsible for handling the chase player state of the Karen in the market.
/// This state is exclusively for the Karen Customer Stereotype.
/// </summary>
public class ChasePlayer : CustomerBaseState
{
    /// <summary>
    /// The karenMovement attribute is a reference to the KarenMovement component.
    /// </summary>
    private KarenMovement karenMovement;

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
    /// It calls the base class Enter method and sets the  karenMovement attribute.
    /// </summary>
    public override void Enter()
    {
        base.Enter();

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
    ///    1. Attacked Transition: If the Karen is attacked, it changes to the Knocked state.
    ///    2. PlayerInRange Transition: If the player is in range, it changes to the Complain state
    ///    
    /// If the none of these conditions are met, the Karen continues to chase the player.
    /// </remarks>
    /// </summary>
    public override void Execute()
    {
        base.Execute();

        if(karenMovement.WasAttacked)
        {
            fSM.ChangeState("Attacked");
            return;
        }

        if (karenMovement.PlayerInRange())
        {
            fSM.ChangeState("PlayerInRange");
            return;
        }

        karenMovement.ChasePlayer();
    }

    /// <summary>
    /// The Exit method is called when the state is exited, to handle its final actions.
    /// It calls the base class Exit method and stops the Karen from chasing the player.
    /// </summary>
    public override void Exit()
    {
        base.Exit();

        karenMovement.StopChasingPlayer();
    }
}
