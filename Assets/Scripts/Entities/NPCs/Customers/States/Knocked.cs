using UnityEngine;

/// <summary>
/// The Knocked class is responsible for handling the knocked state of the customer.
/// This state is implemented for all customer stereotypes.
/// </summary>
public class Knocked : CustomerBaseState
{
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
    /// It calls the base class Enter method and calls the Knock method from the KnockEntity component to knock the customer down.
    /// Inside this method after knocing out the customer and it stand up, its state will change to the AttackPlayer state if the customer is a Karen, otherwise it will change to the GoHome state.
    /// </summary>
    public override void Enter()
    {
        base.Enter();

        GetComponent<KnockEntity>().Knock(gameObject, customerMovement.GetComponent<Rigidbody>(), transform.position);
    }

    /// <summary>
    /// The Enter method is called when the state is entered.
    /// It calls the base class Execute method.
    /// </summary>
    public override void Execute()
    {
        base.Execute();
    }

    /// <summary>
    /// The Exit method is called when the state is exited, to handle its final actions.
    /// It calls the base class Exit method, sets the WasAttacked attribute to false and resets the LastCustomerAttacked attribute in the EventManager instance.
    /// </summary>
    public override void Exit()
    {
        base.Exit();

        customerMovement.WasAttacked = false;
    }
}
