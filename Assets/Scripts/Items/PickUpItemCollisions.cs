using UnityEngine;
using System.Collections;
using UnityEngine.AI;

/// <summary>
///  The PickUpItemCollisions class is responsible for handling the collisions of the pick up itens.
/// </summary>
public class PickUpItemCollisions : MonoBehaviour
{
    [SerializeField]
    private Rigidbody itemRb;

    /// <summary>
    /// The OnCollisionEnter Method is called when this collider/rigidbody has begun touching another rigidbody/collider (Unity Callback).
    /// In this method, after the item collided its layer is changed to Default, to be rendered by th main camera instead of the camera that renders the item grabbed by the player.
    /// </summary>
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

    private void KnockCustumer(GameObject customer)
    {
        if (!customer.GetComponent<CustomerMovement>().IsAgentEnabled())
        {
            return;
        }

        customer.GetComponent<CustomerMovement>().ActivateOrDesactivateAgent(false);

        Rigidbody customerRb = customer.GetComponent<Rigidbody>();

        customerRb.isKinematic = true;
        
       customer.transform.rotation = Quaternion.Euler(90f, 0, 0);
        
        Vector3 customerPos = customer.transform.position;

        const float POSOFFSET = 0.1f;
        customer.transform.position = new Vector3(customerPos.x, customerPos.y - POSOFFSET, customerPos.z);

        StartCoroutine(StandUp(customerRb, customer, POSOFFSET));
    }

    private IEnumerator StandUp(Rigidbody custumerRb, GameObject customer, float POSOFFSET)
    {   
        const float KONCKDOWNTIME = 5f;

        yield return new WaitForSeconds(KONCKDOWNTIME);

        custumerRb.isKinematic = false;

        customer.transform.rotation = Quaternion.identity;

        Vector3 customerPos = customer.transform.position;
        customer.transform.position = new Vector3(customerPos.x, customerPos.y + POSOFFSET, customerPos.z);

        customer.GetComponent<CustomerMovement>().ActivateOrDesactivateAgent(true);
    }
}
