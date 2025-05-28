using UnityEngine;

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
    private void Awake()
    {
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

        if (gameObject.CompareTag("Manager"))
        {
            GetComponent<StrikesSystem>().DispatchStrike(true);
        }else if(gameObject.CompareTag("Customer") && gameObject.name.Contains("Karen"))
        {   
            // To increase the Karen's attacked numbers she must be attacked more than one time to leave the market
            GetComponent<KarenMovement>().Attacked();
        }

        Utils.PlaySoundEffect(Utils.SoundEffects.CUSTOMER_ATTACKED);

        GetComponent<KnockEntity>().Knock(gameObject, GetComponent<Rigidbody>(), transform.position);
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
   
        GetComponent<NPCMovement>().WasAttacked = false;
        
    }
}
