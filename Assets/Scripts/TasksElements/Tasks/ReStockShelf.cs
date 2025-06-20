using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReStockShelf : Task
{  
    [SerializeField]
    private InputActionReference interactAction;

    private Shelf shelf;

    public int ProductsToRestock {get; set; }

    private GameObject[] productsPlaceHolder;


    private GameObject[] shelfProducts;

    private void OnEnable()
    {
        shelf = GetComponent<Shelf>();
        ProductsToRestock = shelf.MaxProducts;
        productsPlaceHolder = shelf.ProductsPlaceHolder;
        shelfProducts = shelf.ShelfProducts;

        Number = 2;

        foreach (GameObject placeHolder in productsPlaceHolder)
        {
            placeHolder.SetActive(true);
        }
    }


    private void Update()
    {
        if (interactAction.action.IsPressed())
        {
            Debug.Log("Boas");
            Restock();
        }
    }


    private void Restock()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Ray ray = new(player.transform.position, player.transform.forward);

        const float RAYCASTDISTANCE = 1.8f;

        if (Physics.Raycast(ray, out RaycastHit hit, RAYCASTDISTANCE, LayerMask.GetMask("Item")))
        {
            Debug.Log("Hit: " + hit.collider.gameObject.name);
            if (!hit.collider.gameObject.name.Contains("PlaceHolder"))
            {
                return;
            }

            foreach (GameObject productPlaceHolder in productsPlaceHolder)
            {
                if (hit.collider.gameObject == productPlaceHolder)
                {
                    PlaceProduct(productPlaceHolder);
                    return;
                }
            }
        }
    }

    private void PlaceProduct(GameObject placeHolder)
    {   
        GameObject productVariant = shelfProducts[Utils.RandomInt(0, shelfProducts.Length)];

        GameObject product = Instantiate(productVariant, placeHolder.transform.position, Quaternion.identity);

        Collider productCollider = product.GetComponent<Collider>();
        productCollider.enabled = false;


        MarketProduct marketProduct = product.GetComponent<MarketProduct>();
        marketProduct.shelf = gameObject;

        
        StartCoroutine(Utils.WaitAndExecute(0.5f, () => productCollider.enabled = true));
        ProductsToRestock--;
        shelf.CurrentProducts++;

        placeHolder.SetActive(false);

        if (ProductsToRestock <= 0)
        {
            foreach (GameObject item in productsPlaceHolder)
            {
                item.SetActive(false);
            }



            TaskCompleted();
        }
    }
}
