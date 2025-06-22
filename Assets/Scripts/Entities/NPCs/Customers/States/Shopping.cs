using UnityEngine;

/// <summary>
/// The Shopping class is responsible for handling the shopping state of the customer in the market.
/// This state is implemented to allow the customer stereotypes.
/// </summary>
public class Shopping : CustomerBaseState
{
    /// <summary>
    /// The minimumTimeToPickProduct and maximumTimeToPickProduct attributes are used to determine the time that the customer will take to pick a product.
    /// </summary>
    [SerializeField]
    private float minTimeToPickProduct, maxTimeToPickProduct;

    /// <summary>
    /// The timer attribute is used to store the time when the customer started picking a product.
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
        timer = 0f;
    }

    /// <summary>
    /// The Enter method is called when the state is entered.
    /// It calls the base class Enter method checks if the customer is a Normal Customer and if it becames a thief. 
    /// If these conditions are met, the customer state is changed to Steal Product state; otherwise, it sets the customer destination to the product area.
    /// </summary>
    public override void Enter()
    {
        base.Enter();

        if (NormalCustomer() && BecamesThief())
        {
            fSM.ChangeState("BecameThief");
            return;
        }

        customerMovement.SetAgentDestination(customerMovement.AreasPos["Product"]);
    }

    /// <summary>
    /// The Execute method is called when the state is executed, to perform the actions of the state.
    /// It calls the base class Execute method.
    /// <remarks>
    /// This methods checks for possible conditions to change the state, otherwise it continues the states actions.
    /// The possible transitions are:
    ///    1. Attacked Transition: If the customer is attacked, it changes to the Knocked state.
    ///    2. ProductFound Transition: If the customer is a Karen and its destinaion is reached, it changes to the Chase Player state.
    ///    3. ProductPicked Transition: It calls the PickProduct method (when the timer to simulate the customer picking the market ends) and changes to the Running state (Annoying Kid) or to the Pay state (Normal Customer).
    ///    
    /// If the none of these conditions are met, nothing happens until the customer reaches its destination or is attacked.
    /// </remarks>
    /// </summary>
    public override void Execute()
    {
        base.Execute();

        if (customerMovement.WasAttacked)
        {
            fSM.ChangeState("Attacked");
            return;
        }

        if (customerMovement.DestinationReached)
        {   
            if (customerMovement is KarenMovement)
            {
               fSM.ChangeState("ProductFound");

                return;
            }

            /*
            if (!GameObject.FindGameObjectWithTag("MarketStock").GetComponent<MarketStock>().IsProductAvaible(customerMovement.TargetProduct.gameObject))
            {
                if( GameObject.FindGameObjectWithTag("MarketStock").GetComponent<MarketStock>().IsOutOfStock(customerMovement.TargetProduct.type))
                {
                    fSM.ChangeState("ProductNotFound");
                    return;
                }
            }
            */

            // Initialize the timer to pick a product
            if (timer == 0f)
            {
                timer = Time.time + Utils.RandomFloat(minTimeToPickProduct, maxTimeToPickProduct);
            }
            else if (Time.time >= timer)
            {
                PickProduct();
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
    }

    /// <summary>
    /// The BecamesThief method is responsible for checking if a Normal Customer becomes a thief.
    /// It generates a random value between 0 and 1 and compares it with the probability of becoming a thief,
    /// if the random value is less than the probability, it returns true, otherwise it returns false.
    /// </summary>
    /// <returns>
    ///  <c>true</c> if the customer becames a theif; otherwise, <c>false</c>.
    /// </returns>
    private bool BecamesThief()
    {
        float randomValue = Utils.RandomFloat(0f, 1f);

        float probBecamingThief = PlayerPrefs.GetFloat("CustomerBecameThiefProb");

        return randomValue < probBecamingThief;
    }

    /// <summary>
    /// The PickProduct method is responsible for handling the remaining actions of the customer when it picks a product and changes its state.
    /// If the customer is an Annoying Kid, it sets the HoldsProduct attribute to true and its state to Running, otherwise it sets the state to Pay (only Normal Customer).
    /// </summary>
    private void PickProduct()
    {   
        if (customerMovement is AnnoyingKidMovement annoyingKid)
        {
            annoyingKid.HoldsProduct = true;
        }

        MarketProduct product = customerMovement.TargetProduct;

        product.EntityHasProduct();

        fSM.ChangeState("ProductPicked");
    }

    /// <summary>
    /// The NormalCustomer method is responsible for checking if the customer is a normal customer.
    /// To do this, it checks if the name of the customer contains the string "NormalCustomer".
    /// </summary>
    /// <returns>
    ///  <c>true</c> if the customer is a Normal Customer; otherwise, <c>false</c>.
    /// </returns>
    private bool NormalCustomer()
    {
       return name.Contains("NormalCustomer");
    }
}
