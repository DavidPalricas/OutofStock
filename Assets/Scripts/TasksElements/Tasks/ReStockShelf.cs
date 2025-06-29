using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// The ReStockShelf class represents the task to restock a shelf 
/// </summary>
public class ReStockShelf : Task
{
    /// <summary>
    /// The interactAction atribute is a reference to the input action that allows the player to restock the shelf's products.
    /// </summary>
    [SerializeField]
    private InputActionReference interactAction;

    /// <summary>
    /// The shelf atribute is a reference to the shelf which this task is associated with.
    /// </summary>
    private Shelf shelf;

    /// <summary>
    /// The productsPlaceHolder atribute stores the place holders for the products on the shelf.
    /// </summary>
    private GameObject[] productsPlaceHolder;

    /// <summary>
    /// The shelfProducts atribute stores the different variants of products that can be placed on the shelf.
    /// </summary>
    private GameObject[] shelfProducts;


    [SerializeField]
    private GameObject subTaskIcon;

    public Dictionary<GameObject, GameObject> RestockedProducts { get; set;} = new ();

    /// <summary>
    /// The ProductsToRestock property is used to store the number of products that need to be restocked on the shelf.
    /// </summary>
    /// <value>
    /// The number of products to restock on the shelf.
    /// </value>
    public int ProductsToRestock {get; set; }

    /// <summary>
    /// The OnEnable method is called when the script is enabled. (Unity's Callback)
    /// This method intianilizes some atrivutes of this class and enables the place holders for the products on the shelf.
    /// </summary>
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

        subTaskIcon.SetActive(true);
    }


    /// <summary>
    /// The Update method is called once per frame. (Unity's Callback)
    /// This method checks if the interact action is pressed, and if so, it calls the Restock method to restock the shelf's products.
    /// </summary>
    private void Update()
    {
        if (interactAction.action.IsPressed())
        {
            Restock();
        }
    }

    /// <summary>
    /// The Restock method is responsible for getting the product's place holder to place a new product on the shelf.
    /// </summary>
    private void Restock()
    {
        GameObject placeHolder = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemsInteractions>().ProductToPlace(productsPlaceHolder);

        if (placeHolder != null)
        {   
            PlaceProduct(placeHolder);
        }
    }
    /// <summary>
    /// The PlaceProduct method is responsible for instantiating a new product on the shelf at the specified place holder's position.
    /// </summary>
    /// <remarks>
    /// This method starts for getting a random product variant and instantiates it at the place holder's position.
    /// After that updates the number of products to restock and the current products on the shelf and if the shelf is fully restocked,
    /// it calls the TaskCompleted method to notify the task manager that the task is completed.
    /// </remarks>
    /// <param name="placeHolder">The product's place holder.</param>
    private void PlaceProduct(GameObject placeHolder)
    {   
        GameObject productVariant = shelfProducts[Utils.RandomInt(0, shelfProducts.Length)];

        productVariant.transform.position = Vector3.zero; // Reset the position of the product variant to avoid any offset issues.

        GameObject product = Instantiate(productVariant, placeHolder.transform.position, Quaternion.identity);

        // Initialize the product atributes.
        MarketProduct marketProduct = product.GetComponent<MarketProduct>();
        marketProduct.Shelf = gameObject;
        shelf.SetProductPickArea(marketProduct);

        StartCoroutine(Utils.WaitAndExecute(0.2f, () => marketProduct.canBePicked = true));

        GameObject.FindGameObjectWithTag("MarketStock").GetComponent<MarketStock>().AddProduct(marketProduct);

        ProductsToRestock--;
        shelf.CurrentProducts++;

        placeHolder.SetActive(false);

        RestockedProducts.Add(product, placeHolder);

        if (ProductsToRestock <= 0)
        {
            foreach (GameObject item in productsPlaceHolder)
            {
                item.SetActive(false);
            }

            subTaskIcon.SetActive(false);
            TaskCompleted();
        }
    }
}
