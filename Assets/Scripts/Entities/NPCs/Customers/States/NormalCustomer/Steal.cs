using UnityEngine;

/// <summary>
/// The Steal class is responsible for handling the stealing state of the customer.
/// It is exclusively for the Normal Customer Stereotype, if it becomes a thief.
/// </summary>
public class Steal : CustomerBaseState
{
    /// <summary>
    /// The minimumTimeToSteal and maximumTimeToSteal attributes are the time that the customer will take to steal a product.
    /// </summary>
    [SerializeField]
    private float minTimeToSteal, maxTimeToSteal;

    /// <summary>
    /// The thiefMaterial attribute is the material that will be used to dress the thief.
    /// </summary>
    [SerializeField]
    private Material thiefMaterial;

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
    /// It calls the base class Enter method and sets the thief destination to the product area.
    /// </summary>
    public override void Enter()
    {
        base.Enter();

        customerMovement.SetAgentDestination(customerMovement.AreasPos["Product"]);
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
            fSM.ChangeState("Attacked");
        }

        if (customerMovement.DestinationReached)
        {   
            StealProduct();
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
    /// The StealProduct method is responsible for handling the stealing process.
    /// It changes the thief's clothes and simulates the stealing action by waiting for a random time between minTimeToSteal and maxTimeToSteal.
    /// After that the state changes to the Go Home state.
    /// </summary>
    private void StealProduct()
    {
        DressThiefClothes();
        StartCoroutine(Utils.WaitAndExecute(Utils.RandomFloat(minTimeToSteal, maxTimeToSteal), () => fSM.ChangeState("ProductStealed")));
    }

    /// <summary>
    /// The DressThiefClothes method is responsible for changing the material of the thief to the thiefMaterial.
    /// </summary>
    /// <remarks>
    /// For now, it is just changing the material of the thief to the thiefMaterial later can change the model of the thief to a different one.
    /// </remarks>
    private void DressThiefClothes()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = thiefMaterial;
    }
}
