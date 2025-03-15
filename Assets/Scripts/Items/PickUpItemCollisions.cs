using UnityEngine;
using System.Collections;

/// <summary>
///  The PickUpItemCollisions class is responsible for handling the collisions of the pick up itens.
/// </summary>
public class PickUpItemCollisions : MonoBehaviour
{   
    /// <summary>
    /// The itemRb is the Rigidbody component of the item.
    /// </summary>
    [SerializeField]
    private Rigidbody itemRb;

    /// <summary>
    /// The OnCollisionEnter Method is called when this collider/rigidbody has begun touching another rigidbody/collider (Unity Callback).
    /// In this method, after the item collided its layer is changed to Default, to be rendered by th main camera instead of the camera that renders the item grabbed by the player.
    /// </summary>
    /// <remarks>
    /// If the item collided with a customer, the KnockCustumer method is called to knock down the customer hitted.
    /// </remarks>
    /// <param name="collision">The collision.</param>
    /// 
    private void OnCollisionEnter(Collision collision)
    {
        gameObject.layer = LayerMask.NameToLayer("Default");

        if (collision.gameObject.CompareTag("Customer") && !itemRb.isKinematic)
        {
            KnockCustumer(collision.gameObject);
        }
    }
    
    /// <summary>
    /// The KnockCustumer method is responsible for knocking down a customer.
    /// </summary>
    /// <remarks>
    ///  When a customer is hitted by an item, its navmesh agent is disabled, its rigidbody is set to kinematic (disabling physics), its position and rotation are changed to simultate the customer is layed.
    /// After 5 seconds, the customer is set to stand up again (StandUp Coroutine).
    /// </remarks>
    /// <param name="customer"> The customer hitted by the objected </param>
    private void KnockCustumer(GameObject customer)
    {
        if (!customer.GetComponent<CustomerMovement>().IsAgentEnabled())
        {
            return;
        }

        customer.GetComponent<CustomerMovement>().EnableOrDisanableAgent(false);

        Rigidbody customerRb = customer.GetComponent<Rigidbody>();

        customerRb.isKinematic = true;
        
        customer.transform.rotation = Quaternion.Euler(90f, 0, 0);
        
        Vector3 customerPos = customer.transform.position;

        const float POSYOFFSET = 0.1f;
        customer.transform.position = new Vector3(customerPos.x, customerPos.y - POSYOFFSET, customerPos.z);

        StartCoroutine(StandUp(customerRb, customer, POSYOFFSET));
    }
    
    /// <summary>
    /// The StandUp Coroutine is responsible for making the customer stand up again after 5 seconds.
    /// </summary>
    /// <remarks>
    /// To simulate the customer standing up, the customer rigidbody is set to non kinematic (enbabling physics), its position and rotation are changed to their original values and its navmesh agent is enabled again.
    /// </remarks> 
    /// <param name="custumerRb">The customer Rigidbody</param>
    /// <param name="customer">The customer hitted by the object</param>
    /// <param name="POSYOFFSET">The y coordinate offset of the customer </param>
    /// <returns></returns>
    private IEnumerator StandUp(Rigidbody custumerRb, GameObject customer, float POSYOFFSET)
    {   
        const float KONCKDOWNTIME = 5f;

        yield return new WaitForSeconds(KONCKDOWNTIME);

        custumerRb.isKinematic = false;

        customer.transform.rotation = Quaternion.identity;

        Vector3 customerPos = customer.transform.position;
        customer.transform.position = new Vector3(customerPos.x, customerPos.y + POSYOFFSET, customerPos.z);

        customer.GetComponent<CustomerMovement>().EnableOrDisanableAgent(true);
    }
}
