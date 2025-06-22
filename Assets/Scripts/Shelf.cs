using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The Shelf class is responsible for managing the products on a shelf in the market.
/// It implements the ISubject interface to notify observers(TaskManager) when products are removed from the shelf,
/// to enable the restock task.
/// </summary>
public class Shelf : MonoBehaviour, ISubject
{    /// <summary>
     /// The productsPlaceHolderGroup and pickAreasGroup attributes are used to store the groups of place holders for the products on the shelf and the pick areas for the products.
     /// </summary>
    [SerializeField]
    private Transform productsPlaceHolderGroup, pickAreasGroup;

    /// <summary>
    /// The observers atribute stores the observers of the shelf (TaskManager). 
    /// </summary>
    private IObserver[] observers;

    /// <summary>
    /// The pickAreas attribute is used to store the pick areas for the products on the shelf.
    /// </summary>
    private GameObject[] pickAreas;

    /// <summary>
    /// The ProductsPlaceHolder property is used to store the place holders for the products on the shelf.
    /// </summary>
    /// <value>
    /// The products place holder.
    /// </value>
    public GameObject[] ProductsPlaceHolder { get; private set; }

    /// <summary>
    /// The ShelfProducts property is used to store the different variants of products that can be placed on the shelf.
    /// </summary>
    /// <value>
    /// The shelf products that can be placed on the shelf.
    /// </value>
    public GameObject[] ShelfProducts { get; private set; }

    /// <summary>
    /// The MaxProducts property is used to store the maximum number of products that can be placed on the shelf.
    /// </summary>
    /// <value>
    /// The maximum products that can be placed on the shelf.
    /// </value>
    public int MaxProducts { get; private set; }

    /// <summary>
    /// The CurrentProducts property is used to store the current number of products on the shelf.
    /// </summary>
    /// <value>
    /// The current products on the shelf.
    /// </value>
    public int CurrentProducts { get; set; }

    /// <summary>
    /// The productsType property is used to define the type of products that this shelf holds.
    /// </summary>
    public MarketProduct.ProductType productsType;

    /// <summary>
    /// The Awake method is called when the script instance is being loaded.
    /// </summary>
    /// <remarks>
    /// This method starts to get the maximum number of products that can be placed on the shelf, by reading it from the player prefs.
    /// It initiallizes other attributes, such as ProductsPlaceHolder and pickProduct, and calls the AddProducts method to add products to the shelf.
    /// </remarks>
    private void Awake()
    {   
        MaxProducts = Utils.RandomInt(PlayerPrefs.GetInt("MinProductsInShelfs"), PlayerPrefs.GetInt("MaxProductsInShelfs")); 
        CurrentProducts = 0;

        ProductsPlaceHolder = Utils.GetChildren(productsPlaceHolderGroup);
        pickAreas = Utils.GetChildren(pickAreasGroup);

        IObserver taskManager = GameObject.FindGameObjectWithTag("TaskManager").GetComponent<TaskManager>();

        AddObservers(new IObserver[] { taskManager });

        AddProducts();

        // Deactivate all place holders to the player not be able to see them in the game.
        foreach (GameObject placeHolder in ProductsPlaceHolder)
        {
            placeHolder.SetActive(false);
        }
    }

    /// <summary>
    /// The AddProducts method is responsible for adding products to the shelf based on the productsType.
    /// </summary>
    /// <remarks>
    /// This method starts to get the variants of the product based on its type.
    /// After that, it randomly selects place holders from the ProductsPlaceHolder array to instantiate the products with a random variant.
    /// </remarks>
    private void AddProducts()
    {

         ShelfProducts = GameObject.FindGameObjectWithTag("MarketStock")
            .GetComponent<MarketStock>()
            .GetProductVariants(productsType);

        // This HashSet is used to ensure that we do not use the same place holder more than once.
        HashSet<int> usedPlaceHolders = new();

        for (int i = 0; i < MaxProducts; i++)
        {
            int randomIndex = Utils.RandomInt(0, ProductsPlaceHolder.Length);

            if (!usedPlaceHolders.Add(randomIndex))
            {
                i--;
                continue;
            }

            GameObject placeHolder = ProductsPlaceHolder[randomIndex];
            GameObject productVariant = ShelfProducts[Utils.RandomInt(0, ShelfProducts.Length)];

            productVariant.transform.localPosition = Vector3.zero; // Reset the position of the product variant to avoid any offset issues.
            GameObject product = Instantiate(productVariant, placeHolder.transform.position, Quaternion.identity);

            MarketProduct marketProduct = product.GetComponent<MarketProduct>();

            marketProduct.Shelf = gameObject;
            SetProductPickArea(marketProduct);

            CurrentProducts++;
        }
    }

    /// <summary>
    /// The ProductRemoved method is responsible for handling the logic when a product is removed from the shelf.
    /// </summary>
    /// <remarks>
    /// This method decreases the current number of products on the shelf, if the shelf has its restock task actived,
    ///  increases the number of products to restock.
    ///  If the current number of products on the shelf is less than or equal to zero, it notifies the observers (TaskManager) to start the restock task after a delay of 5 seconds.
    /// </remarks>
    public void ProductRemoved()
    {
        CurrentProducts--;


        ReStockShelf restockShelf = GetComponent<ReStockShelf>();

        if (restockShelf.enabled)
        {
            restockShelf.ProductsToRestock++;
        }

        if (CurrentProducts <= 0 && !restockShelf.enabled)
        {
            StartCoroutine(Utils.WaitAndExecute(5f, () => NotifyObservers(gameObject)));      
        }
    }

    /// <summary>
    /// The AddObserver method (ISubject method) is responsible for adding an observer (TaskManager) to the subject.
    /// </summary>
    /// <param name="observers">The observers.</param>
    public void AddObservers(IObserver[] observers)
    {
        this.observers = observers;
    }

    /// <summary>
    /// The RemoveObservers method (ISubject method) is responsible for removing the observers from the subject.
    /// </summary>
    public void RemoveObservers()
    {
        observers = null;
    }

    /// <summary>
    /// The NotifyObservers method (ISubject method) is responsible for notifying the observers(TaskManager) when a product is removed from the shelf.
    /// </summary>
    /// <param name="data">Any argument to be sent to the observer.</param>
    public void NotifyObservers(object data)
    {
        foreach (IObserver observer in observers)
        {
            observer.OnNotify(data);
        }
    }

    /// <summary>
    /// The SetProductPickArea method is used to set the pick area for a product.
    /// It finds the closest pick area to the product's position and assigns it to the product's PickProductArea transform.
    /// </summary>
    /// <param name="product">The product.</param>
    public void SetProductPickArea(MarketProduct product)
    {
        product.PickProductArea = pickAreas
                   .OrderBy(p => Vector3.Distance(p.transform.position, product.transform.position))
                   .FirstOrDefault()?.transform;
    }
}
