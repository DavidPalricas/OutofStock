using UnityEngine;

/// <summary>
/// The Running class is responsible for handling the running state of the annoying kid in the market.
/// This state is exclusively for the Annoying Kid Customer Stereotype.
/// </summary>
public class Running : CustomerBaseState
{
    /// <summary>
    /// The timeToDecideToGrabAProduct attribute is the time that the annoying kid will take to decide whether to grab a product or not.
    /// </summary>
    [SerializeField]
    private float timeToDecideToGrabAProduct;

    /// <summary>
    /// The probToGrabAProduct attribute is the probability that the annoying kid will grab a product.
    /// </summary>
    [SerializeField]
    [Range(0, 1)]
    private float probToGrabAProduct;

    /// <summary>
    /// The timer attribute is used to count the time that the annoying kid will take to decide whether to grab a product or not.
    /// </summary>
    private float timer;

    /// <summary>
    /// The holdsProduct attribute is a flag that indicates whether the annoying kid holds a market product or not.
    /// </summary>
    private bool holdsProduct = false;

    /// <summary>
    /// The annoyingKidMovement attribute is a reference to the AnnoyingKidMovement component.
    /// </summary>
    private AnnoyingKidMovement annoyingKidMovement;

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
    /// It calls the base class Enter method and sets the timer to the current time plus the timeToDecideToGrabAProduct and
    /// sets the annoyingKid to run.
    /// </summary>
    public override void Enter()
    {
        base.Enter();

        timer = Time.time + timeToDecideToGrabAProduct;

        if (customerMovement is AnnoyingKidMovement)
        {
            annoyingKidMovement = customerMovement as AnnoyingKidMovement;
            annoyingKidMovement.Run();

            holdsProduct = annoyingKidMovement.HoldsProduct;
        }
    }

    /// <summary>
    /// The Execute method is called when the state is executed, to perform the actions of the state.
    /// It calls the base class Execute method.
    /// <remarks>
    /// This methods checks for possible conditions to change the state, otherwise it continues the states actions.
    /// The possible transitions are:
    ///    1. Attacked Transition: If the kid is attacked, it changes to the Knocked state.
    ///    2. PickAProduct Transition: If the time to decide a product and the kid decides to grab it, it changes to the Shopping state.
    ///    
    /// If the none of these conditions are met, this method checks if the kid has reached its destination, if so, it runs again to a new destination.
    /// </remarks>
    /// </summary>
    public override void Execute()
    {
        base.Execute();

        if (customerSanity.CurrentSanity != customerSanity.maxSanity)
        {
            fSM.ChangeState("Attacked");
            return;
        }
 
       if (!holdsProduct && Time.time >= timer)
       {
         if (DecidedToGrabAProduct())
         {
            fSM.ChangeState("PickAProduct");
            return;
         }

         // The time is reset if the kid decides not to grab a product.
         timer = Time.time + timeToDecideToGrabAProduct;
       }
        
        if (annoyingKidMovement.DestinationReached)
        {
            annoyingKidMovement.Run();
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
    /// The DecidedToGrabAProduct method is responsible for deciding whether the annoying kid will grab a product or not.
    /// To that a random value is generated and compared with the probToGrabAProduct attribute.
    /// </summary>
    /// <returns>
    ///  <c>true</c> if decides to grab a product; otherwise, <c>false</c>.
    /// </returns>
    private bool DecidedToGrabAProduct()
    {
        return Utils.RandomFloat(0, 1) <= probToGrabAProduct;
    }
}
