using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shelf : MonoBehaviour, ISubject
{
    private IObserver[] observers;

    [SerializeField]
    private Transform productsPlaceHolderArea;

    [SerializeField]
    private GameObject waterBottle1, waterBottle2, beerCan, beerBottle;

    public GameObject[] ProductsPlaceHolder { get; private set; }

    public GameObject[] ShelfProducts { get; private set; }
    public int MaxProducts { get; private set; }

    public int CurrentProducts { get; set; }

    public MarketProduct.ProductType productsType;

    private void Awake()
    {
        MaxProducts = GameObject.FindGameObjectsWithTag("Item")
           .Count(item => item.TryGetComponent(out MarketProduct product) && product.shelf == gameObject);
        
        CurrentProducts = MaxProducts;

        ProductsPlaceHolder = Utils.GetChildren(productsPlaceHolderArea);

        IObserver taskManager = GameObject.FindGameObjectWithTag("TaskManager").GetComponent<TaskManager>();

        AddObservers(new IObserver[] { taskManager });

        AddProducts();

        foreach (GameObject placeHolder in ProductsPlaceHolder)
        {
            placeHolder.SetActive(false);
        }
    }


    private void AddProducts()
    {
        switch (productsType)
        {
            case MarketProduct.ProductType.WATER:
                ShelfProducts = new GameObject[] { waterBottle1, waterBottle2 };
                break;

            case MarketProduct.ProductType.BEER:
                ShelfProducts = new GameObject[] { beerCan, beerBottle };
                break;

            default:
                ShelfProducts = new GameObject[] { waterBottle1, waterBottle2, beerCan, beerBottle };
                break;
        }

        const int MAX_PRODUCTS = 5;

        MaxProducts = MAX_PRODUCTS;

        HashSet<int> usedPlaceHolders = new();

        for (int i = 0; i < MAX_PRODUCTS; i++)
        {
            int randomIndex = Utils.RandomInt(0, ProductsPlaceHolder.Length);

            if (!usedPlaceHolders.Add(randomIndex))
            {
                i--;
                continue;
            }

            GameObject placeHolder = ProductsPlaceHolder[randomIndex];

            GameObject productVariant = ShelfProducts[Utils.RandomInt(0, ShelfProducts.Length)];

            GameObject product = Instantiate(productVariant, placeHolder.transform.position, Quaternion.identity);

            MarketProduct marketProduct = product.GetComponent<MarketProduct>();

            marketProduct.shelf = gameObject;

            CurrentProducts++;
        }
    }

    public void ProductRemoved()
    {
        CurrentProducts--;


        if (GetComponent<ReStockShelf>().enabled)
        {
            GetComponent<ReStockShelf>().ProductsToRestock++;
        }

        if (CurrentProducts <= 0)
        {
            // Wait for a certain amount of time before notifying observers (the restock task)
            StartCoroutine(Utils.WaitAndExecute(5f, () => NotifyObservers(gameObject)));
      
        }
    }

    public void AddObservers(IObserver[] observers)
    {
        this.observers = observers;
    }

    public void RemoveObservers()
    {
        observers = null;
    }

    public void NotifyObservers(object data)
    {
        foreach (IObserver observer in observers)
        {
            observer.OnNotify(data);
        }
    }
}
