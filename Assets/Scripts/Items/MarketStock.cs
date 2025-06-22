using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MarketStock : MonoBehaviour
{
    [SerializeField]
    private GameObject tunaCan1, tunaCan2, tunaCan3, tunaCan4;

    [SerializeField]
    private GameObject milk1, milk2;

    /*
    [SerializeField]
    private GameObject orange;
    
    [SerializeField]
    private GameObject redApple, greenApple;
  
    [SerializeField]
    private GameObject wine1, wine2;
    */

    [SerializeField]
    private GameObject beerBottle1, beerBottle2;

    [SerializeField]
    private GameObject beerCan1, beerCan2;

    /*
    [SerializeField]
    private GameObject water1, water2;
    */

    [SerializeField]
    private GameObject toiletPaper1, toiletPaper2;

    [SerializeField]
    private GameObject handSoap1, handSoap2;

    [SerializeField]
    private GameObject cleaningSpray1, cleaningSpray2;

    public List<GameObject> Tunas { get; private set; }

    public List<GameObject> Milks { get; private set; }


    // private List<GameObject> oranges, redApples, greenApples, bananas, lemons, watermelons;

    public List<GameObject> BeerBottles { get; private set; }
    public List<GameObject> BeerCans { get; private set; }

    public List<GameObject> ToiletPapers { get; private set; }
    public List<GameObject>  HandSoaps { get; private set; } 
    public List<GameObject> CleaningSprays { get; private set; }

    private void Start()
    {
        SetStock();
    }

    private void SetStock()
    {
        GameObject[] marketProducts = GameObject.FindGameObjectsWithTag("Item")
           .Where(item => item.GetComponent<MarketProduct>() != null)
           .ToArray();

        Tunas = marketProducts.Where(item => item.GetComponent<MarketProduct>().type == MarketProduct.ProductType.TUNA_CAN).ToList();
        Milks = marketProducts.Where(item => item.GetComponent<MarketProduct>().type == MarketProduct.ProductType.MILK).ToList();
   
        /*
        oranges = marketProducts.Where(item => item.GetComponent<MarketProduct>().Type == MarketProduct.ProductType.ORANGE).ToList();
        redApples = marketProducts.Where(item => item.GetComponent<MarketProduct>().Type == MarketProduct.ProductType.RED_APPLE).ToList();
        greenApples = marketProducts.Where(item => item.GetComponent<MarketProduct>().Type == MarketProduct.ProductType.GREEN_APPLE).ToList();
        */

        // wines = marketProducts.Where(item => item.GetComponent<MarketProduct>().Type == MarketProduct.ProductType.WINE).ToList();
        BeerBottles = marketProducts.Where(item => item.GetComponent<MarketProduct>().type == MarketProduct.ProductType.BEER_BOTTLE).ToList();
        BeerCans = marketProducts.Where(item => item.GetComponent<MarketProduct>().type == MarketProduct.ProductType.BEER_CAN).ToList();
        // waters = marketProducts.Where(item => item.GetComponent<MarketProduct>().Type == MarketProduct.ProductType.WATER).ToList();

        ToiletPapers = marketProducts.Where(item => item.GetComponent<MarketProduct>().type == MarketProduct.ProductType.TOILET_PAPER).ToList();
        HandSoaps = marketProducts.Where(item => item.GetComponent<MarketProduct>().type == MarketProduct.ProductType.HAND_SOAP).ToList();
        CleaningSprays = marketProducts.Where(item => item.GetComponent<MarketProduct>().type == MarketProduct.ProductType.CLEANING_SPRAY).ToList();
    }


    /// <summary>
    /// The GetProductVariants method gets the variants of the products based on the product type.
    /// </summary>
    /// <param name="productsType">Type of the products.</param>
    /// <returns>An array containing the product variants</returns>
    public GameObject[] GetProductVariants(MarketProduct.ProductType productsType)
    {
        switch (productsType)
        {
            case MarketProduct.ProductType.TUNA_CAN:
                return new GameObject[] { tunaCan1, tunaCan2, tunaCan3, tunaCan4 };

            case MarketProduct.ProductType.MILK:
                return new GameObject[] { milk1, milk2 };
               
            /*
            case MarketProduct.ProductType.ORANGE:
                ShelfProducts = new GameObject[] { orange };
                return;

            case MarketProduct.ProductType.RED_APPLE:
                ShelfProducts = new GameObject[] { redApple};
                return;

            case MarketProduct.ProductType.GREEN_APPLE:
                ShelfProducts = new GameObject[] { greenApple };
                return;
          
            case MarketProduct.ProductType.WINE:
                ShelfProducts = new GameObject[] { wine1, wine2 };
                return;

             */

            case MarketProduct.ProductType.BEER_BOTTLE:
                return new GameObject[] { beerBottle1, beerBottle2 };
        
            case MarketProduct.ProductType.BEER_CAN:
                return new GameObject[] { beerCan1, beerCan2 };
            /*
            case MarketProduct.ProductType.WATER:
                ShelfProducts = new GameObject[] { water1, water2 };
                return;
            */

            case MarketProduct.ProductType.TOILET_PAPER:
                return new GameObject[] { toiletPaper1, toiletPaper2 };

            case MarketProduct.ProductType.HAND_SOAP:
                return new GameObject[] { handSoap1, handSoap2 };

            case MarketProduct.ProductType.CLEANING_SPRAY:
                return new GameObject[] { cleaningSpray1, cleaningSpray2 };

            default:
                Debug.LogError("Unknown product type: " + productsType);
                return null;
        }
    }

    public void RemoveProduct(GameObject product)
    {
        MarketProduct.ProductType type = product.GetComponent<MarketProduct>().type;

        switch (type)
        {
            case MarketProduct.ProductType.TUNA_CAN:
                Tunas.Remove(product);
                return;

            case MarketProduct.ProductType.MILK:
                Milks.Remove(product);
                return;
            
            /*
            case MarketProduct.ProductType.ORANGE:
                oranges.Remove(product);
                return;

            case MarketProduct.ProductType.RED_APPLE:
                redApples.Remove(product);
                return;

            case MarketProduct.ProductType.GREEN_APPLE:
                redApples.Remove(product);
                return;

            case MarketProduct.ProductType.WINE:
                wines.Remove(product);
                return;
            */

            case MarketProduct.ProductType.BEER_BOTTLE:
                BeerBottles.Remove(product);
                return;

            case MarketProduct.ProductType.BEER_CAN:
                BeerCans.Remove(product);
                return;

            /*
            case MarketProduct.ProductType.WATER:
                waters.Remove(product);
                return;
            */

            case MarketProduct.ProductType.TOILET_PAPER:
                ToiletPapers.Remove(product);
                return;

            case MarketProduct.ProductType.HAND_SOAP:
                HandSoaps.Remove(product);
                return;

            case MarketProduct.ProductType.CLEANING_SPRAY:
                CleaningSprays.Remove(product);
                return;

            default:
                Debug.LogError("Unknown product type: " + type);
                return;
        }
    }


    public bool IsProductAvaible(GameObject product)
    {
        MarketProduct.ProductType type = product.GetComponent<MarketProduct>().type;

        return type switch
        {
            MarketProduct.ProductType.TUNA_CAN => Tunas.Contains(product),
            MarketProduct.ProductType.MILK => Milks.Contains(product),
            /*
            MarketProduct.ProductType.ORANGE => oranges.Contains(product),
            MarketProduct.ProductType.RED_APPLE => redApples.Contains(product),
            MarketProduct.ProductType.GREEN_APPLE => greenApples.Contains(product),
            */

            // MarketProduct.ProductType.WINE => wines.Contains(product),
            MarketProduct.ProductType.BEER_BOTTLE => BeerBottles.Contains(product),
            MarketProduct.ProductType.BEER_CAN => BeerCans.Contains(product),
            // MarketProduct.ProductType.WATER => waters.Contains(product),

            MarketProduct.ProductType.TOILET_PAPER => ToiletPapers.Contains(product),
            MarketProduct.ProductType.HAND_SOAP => HandSoaps.Contains(product),
            MarketProduct.ProductType.CLEANING_SPRAY => CleaningSprays.Contains(product),
            _ => false
        };
    }

    public bool IsOutOfStock(MarketProduct.ProductType type)
    {
        return type switch
        {
            MarketProduct.ProductType.TUNA_CAN => Tunas.Count == 0,
            MarketProduct.ProductType.MILK => Milks.Count == 0,
            /*
            MarketProduct.ProductType.ORANGE => oranges.Count == 0,
            MarketProduct.ProductType.RED_APPLE => redApples.Count == 0,
             MarketProduct.ProductType.GREEN_APPLE => greenApples.Count == 0,
            MarketProduct.ProductType.WINE => wines.Count == 0,
            */
            MarketProduct.ProductType.BEER_BOTTLE => BeerBottles.Count == 0,
            // MarketProduct.ProductType.WATER => waters.Count == 0,

            MarketProduct.ProductType.BEER_CAN => BeerCans.Count == 0,

            MarketProduct.ProductType.TOILET_PAPER => ToiletPapers.Count == 0,
            MarketProduct.ProductType.HAND_SOAP => HandSoaps.Count == 0,
            MarketProduct.ProductType.CLEANING_SPRAY => CleaningSprays.Count == 0,
            _ => false
        };
    }


    public void AddProduct(MarketProduct product)
    {
        MarketProduct.ProductType type = product.type;
        switch (type)
        {
            case MarketProduct.ProductType.TUNA_CAN:
                Tunas.Add(product.gameObject);
                break;
            case MarketProduct.ProductType.MILK:
                Milks.Add(product.gameObject);
                break;
            /*
            case MarketProduct.ProductType.ORANGE:
                oranges.Add(product.gameObject);
                break;
            case MarketProduct.ProductType.RED_APPLE:
                redApples.Add(product.gameObject);
                break;
            case MarketProduct.ProductType.GREEN_APPLE:
                greenApples.Add(product.gameObject);
                break;
            */
            // case MarketProduct.ProductType.WINE:
            //     wines.Add(product.gameObject);
            //     break;
            case MarketProduct.ProductType.BEER_BOTTLE:
                BeerBottles.Add(product.gameObject);
                break;
            case MarketProduct.ProductType.BEER_CAN:
                BeerCans.Add(product.gameObject);
                break;
            // case MarketProduct.ProductType.WATER:
            //     waters.Add(product.gameObject);
            //     break;
            case MarketProduct.ProductType.TOILET_PAPER:
                ToiletPapers.Add(product.gameObject);
                break;
            case MarketProduct.ProductType.HAND_SOAP:
                HandSoaps.Add(product.gameObject);
                break;
            case MarketProduct.ProductType.CLEANING_SPRAY:
                CleaningSprays.Add(product.gameObject);
                break;
            default:
                Debug.LogError("Unknown product type: " + type);
                break;
        }
    }
    }

