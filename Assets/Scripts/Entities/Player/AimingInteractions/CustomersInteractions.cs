using UnityEngine;

public class CustomersInteractions : AimingAction
{
    private void Update()
    {
        CheckIfisAimingAtACustomer();   
    }

    public void CheckIfisAimingAtACustomer()
    {
        Ray ray = Utils.CastRayFromUI(crosshair);

        if (Physics.Raycast(ray, out RaycastHit hit, RAYCASTDISTANCE, LayerMask.GetMask("Entity")) && hit.collider.gameObject.CompareTag("Customer"))
        {   
            GameObject customer = hit.collider.gameObject;

            CustomerSanity customerSanity = customer.GetComponent<CustomerSanity>() != null ? customer.GetComponent<CustomerSanity>() : customer.transform.parent.GetComponent<CustomerSanity>();

            customerSanity.ShowSanityBar();
        }
    }
}
