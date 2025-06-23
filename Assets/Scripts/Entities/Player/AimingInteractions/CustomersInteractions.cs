using UnityEngine;

public class CustomersInteractions : AimingAction
{   
    private int frameCounter = 0;

    private void Update()
    {
        if (frameCounter >= 10)
        {
            CheckIfisAimingAtACustomer();
            frameCounter = 0;
        }

        frameCounter++;
    }

    public void CheckIfisAimingAtACustomer()
    {
        Ray ray = Utils.CastRayFromUI(crosshair);

        if (Physics.Raycast(ray, out RaycastHit hit, RAYCASTDISTANCE, LayerMask.GetMask("Entity")) && hit.collider.gameObject.CompareTag("Customer"))
        {   
            GameObject customer = hit.collider.gameObject;

            CustomerSanity customerSanity = customer.GetComponent<CustomerSanity>() != null ? customer.GetComponent<CustomerSanity>() : customer.transform.parent.GetComponent<CustomerSanity>();

            customerSanity.ShowSanityBar();

            CheckIfPlayerIsTryingtoSteal(customer);
        }
    }


    private void CheckIfPlayerIsTryingtoSteal(GameObject customer)
    {
       if (customer.name.Contains("BackPack") && interactAction.action.IsPressed())
        {
            Transform customerTransform = customer.transform;

            ItemsInteractions itemsInteractions = GetComponent<ItemsInteractions>();

            if (!itemsInteractions.ItemGrabbed && customer.transform.childCount != 0)
            {
                GameObject product = customerTransform.GetChild(0).gameObject;

                itemsInteractions.HoldItem(product);
            }
        }
    }
}
