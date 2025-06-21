using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// The ReStockShelf class represents the task to restock a shelf 
/// </summary>
public class ReStockShelf : Task
{
    /// <summary>
    /// The taskNumber atribute is used to store the number of restock shelf tasks.
    /// </summary>
    /// <remarks>
    /// It starts at number 2 because the number 0 and 1 are reserved for other tasks (clean the floor and fix fuse box),
    /// which are randomly assigned to the player during the game by the TaskManager.
    /// </remarks>
    private static int restockTaskNumber = 2;

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

    /// <summary>
    /// The ProductsToRestock property is used to store the number of products that need to be restocked on the shelf.
    /// </summary>
    /// <value>
    /// The number of products to restock on the shelf.
    /// </value>
    public int ProductsToRestock {get; set; }

    /// <summary>
    /// The Awake method is called when the script instance is being loaded. (Unity's Callback)
    /// It initializes the task number for this restock shelf task by incrementing the static restockTaskNumber variable.
    /// </summary>
    private void Awake()
    {
        Number = restockTaskNumber;
        restockTaskNumber++;
    }

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

        foreach (GameObject placeHolder in productsPlaceHolder)
        {
            placeHolder.SetActive(true);
        }
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
        GameObject placeHolder = GameObject.FindGameObjectWithTag("Player").GetComponent<GrabAndThrowItems>().ProductToPlace(productsPlaceHolder);

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

        GameObject product = Instantiate(productVariant, placeHolder.transform.position, Quaternion.identity);

        Collider productCollider = product.GetComponent<Collider>();
        productCollider.enabled = false;

        // Initialize the product atributes.
        MarketProduct marketProduct = product.GetComponent<MarketProduct>();
        marketProduct.Shelf = gameObject;
        shelf.SetProductPickArea(marketProduct);

        // Enables the product collider after a short delay to allow the product, to avoid the player grabbing it immediately after placing it.
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
