using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MarketStock : MonoBehaviour
{
    [SerializeField]
    private GameObject tunaCan1, tunaCan2, tunaCan3, tunaCan4;

    [SerializeField]
    private GameObject milk1, milk2;

    [SerializeField]
    private GameObject chips1, chips2;

    [SerializeField]
    private GameObject cereal1, cereal2;

    [SerializeField]
    private GameObject yougourt1, yougourt2;


    [SerializeField]
    private GameObject pinneApple;

    [SerializeField]
    private GameObject orange;

    [SerializeField]
    private GameObject redApple, greenApple;

    [SerializeField]
    private GameObject lemon;

    [SerializeField]
    private GameObject banana;



    [SerializeField]
    private GameObject beerBottle1, beerBottle2;

    [SerializeField]
    private GameObject beerCan1, beerCan2;


    [SerializeField]
    private GameObject water;

    [SerializeField]
    private GameObject whiskey;

    [SerializeField]
    private GameObject wine1, wine2;

    [SerializeField]
    private GameObject toiletPaper1, toiletPaper2;

    [SerializeField]
    private GameObject handSoap1, handSoap2;

    [SerializeField]
    private GameObject cleaningSpray1, cleaningSpray2;

    [SerializeField]
    private GameObject toothPaste;


    [SerializeField]
    private GameObject handsCream1, handsCream2;

    public List<GameObject> Tunas { get; private set; }
    public List<GameObject> Milks { get; private set; }
    public List<GameObject> Chips { get; private set; }
    public List<GameObject> Cereals { get; private set; }
    public List<GameObject> Yougourts { get; private set; }

    public List<GameObject> PinneApples { get; private set; }
    public List<GameObject> Oranges { get; private set; }
    public List<GameObject> RedApples { get; private set; }
    public List<GameObject> GreenApples { get; private set; }
    public List<GameObject> Lemons { get; private set; }
    public List<GameObject> Bananas { get; private set; }


    public List<GameObject> BeerBottles { get; private set; }
    public List<GameObject> BeerCans { get; private set; }
    public List<GameObject> Waters { get; private set; }
    public List<GameObject> Whiskeys { get; private set; }
    public List<GameObject> Wines { get; private set; }

    public List<GameObject> ToiletPapers { get; private set; }
    public List<GameObject> HandSoaps { get; private set; }
    public List<GameObject> CleaningSprays { get; private set; }
    public List<GameObject> ToothPastes { get; private set; }
    public List<GameObject> HandsCreams { get; private set; }


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
        Chips = marketProducts.Where(item => item.GetComponent<MarketProduct>().type == MarketProduct.ProductType.CHPS).ToList();
        Cereals = marketProducts.Where(item => item.GetComponent<MarketProduct>().type == MarketProduct.ProductType.CEREALS).ToList();
        Yougourts = marketProducts.Where(item => item.GetComponent<MarketProduct>().type == MarketProduct.ProductType.YOUGOURT).ToList();

        PinneApples = marketProducts.Where(item => item.GetComponent<MarketProduct>().type == MarketProduct.ProductType.PINNEAPPLE).ToList();
        Oranges = marketProducts.Where(item => item.GetComponent<MarketProduct>().type == MarketProduct.ProductType.ORANGE).ToList();
        RedApples = marketProducts.Where(item => item.GetComponent<MarketProduct>().type == MarketProduct.ProductType.RED_APPLE).ToList();
        GreenApples = marketProducts.Where(item => item.GetComponent<MarketProduct>().type == MarketProduct.ProductType.GREEN_APPLE).ToList();
        Lemons = marketProducts.Where(item => item.GetComponent<MarketProduct>().type == MarketProduct.ProductType.LEMON).ToList();
        Bananas = marketProducts.Where(item => item.GetComponent<MarketProduct>().type == MarketProduct.ProductType.BANANA).ToList();


        BeerBottles = marketProducts.Where(item => item.GetComponent<MarketProduct>().type == MarketProduct.ProductType.BEER_BOTTLE).ToList();
        BeerCans = marketProducts.Where(item => item.GetComponent<MarketProduct>().type == MarketProduct.ProductType.BEER_CAN).ToList();
        Waters = marketProducts.Where(item => item.GetComponent<MarketProduct>().type == MarketProduct.ProductType.WATER).ToList();
        Whiskeys = marketProducts.Where(item => item.GetComponent<MarketProduct>().type == MarketProduct.ProductType.WHISKYEY).ToList();
        Wines = marketProducts.Where(item => item.GetComponent<MarketProduct>().type == MarketProduct.ProductType.WINE).ToList();

        ToiletPapers = marketProducts.Where(item => item.GetComponent<MarketProduct>().type == MarketProduct.ProductType.TOILET_PAPER).ToList();
        HandSoaps = marketProducts.Where(item => item.GetComponent<MarketProduct>().type == MarketProduct.ProductType.HAND_SOAP).ToList();
        CleaningSprays = marketProducts.Where(item => item.GetComponent<MarketProduct>().type == MarketProduct.ProductType.CLEANING_SPRAY).ToList();
        ToothPastes = marketProducts.Where(item => item.GetComponent<MarketProduct>().type == MarketProduct.ProductType.TOOTHPASTE).ToList();
        HandsCreams = marketProducts.Where(item => item.GetComponent<MarketProduct>().type == MarketProduct.ProductType.HANDS_CREAM).ToList();
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

            case MarketProduct.ProductType.CHPS:
                return new GameObject[] { chips1, chips2 };


            case MarketProduct.ProductType.CEREALS:
                return new GameObject[] { cereal1, cereal2 };


            case MarketProduct.ProductType.YOUGOURT:
                return new GameObject[] { yougourt1, yougourt2 };


            case MarketProduct.ProductType.PINNEAPPLE:
                return new GameObject[] { pinneApple };


            case MarketProduct.ProductType.ORANGE:
                return new GameObject[] { orange };


            case MarketProduct.ProductType.RED_APPLE:
                return new GameObject[] { redApple };


            case MarketProduct.ProductType.GREEN_APPLE:
                return new GameObject[] { greenApple };

            case MarketProduct.ProductType.BANANA:
                return new GameObject[] { banana };

            case MarketProduct.ProductType.LEMON:
                return new GameObject[] { lemon };


            case MarketProduct.ProductType.BEER_BOTTLE:
                return new GameObject[] { beerBottle1, beerBottle2 };

            case MarketProduct.ProductType.BEER_CAN:
                return new GameObject[] { beerCan1, beerCan2 };

            case MarketProduct.ProductType.WATER:
                return new GameObject[] { water };

            case MarketProduct.ProductType.WHISKYEY:
                return new GameObject[] { whiskey };

            case MarketProduct.ProductType.WINE:
                return new GameObject[] { wine1, wine2 };


            case MarketProduct.ProductType.TOILET_PAPER:
                return new GameObject[] { toiletPaper1, toiletPaper2 };

            case MarketProduct.ProductType.HAND_SOAP:
                return new GameObject[] { handSoap1, handSoap2 };

            case MarketProduct.ProductType.CLEANING_SPRAY:
                return new GameObject[] { cleaningSpray1, cleaningSpray2 };

            case MarketProduct.ProductType.TOOTHPASTE:
                return new GameObject[] { toothPaste };


            case MarketProduct.ProductType.HANDS_CREAM:
                return new GameObject[] { handsCream1, handsCream2 };

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

            case MarketProduct.ProductType.CHPS:
                Chips.Remove(product);
                return;

            case MarketProduct.ProductType.CEREALS:
                Cereals.Remove(product);
                return;


            case MarketProduct.ProductType.YOUGOURT:
                Yougourts.Remove(product);
                return;


            case MarketProduct.ProductType.PINNEAPPLE:
                PinneApples.Remove(product);
                return;


            case MarketProduct.ProductType.ORANGE:
                Oranges.Remove(product);
                return;


            case MarketProduct.ProductType.RED_APPLE:
                RedApples.Remove(product);
                return;

            case MarketProduct.ProductType.GREEN_APPLE:
                GreenApples.Remove(product);
                return;


            case MarketProduct.ProductType.BANANA:
                Bananas.Remove(product);
                return;


            case MarketProduct.ProductType.LEMON:
                Lemons.Remove(product);
                return;

            case MarketProduct.ProductType.BEER_BOTTLE:
                BeerBottles.Remove(product);
                return;

            case MarketProduct.ProductType.BEER_CAN:
                BeerCans.Remove(product);
                return;


            case MarketProduct.ProductType.WATER:
                Waters.Remove(product);
                return;


            case MarketProduct.ProductType.WHISKYEY:
                Whiskeys.Remove(product);
                return;


            case MarketProduct.ProductType.WINE:
                Wines.Remove(product);
                return;

            case MarketProduct.ProductType.TOILET_PAPER:
                ToiletPapers.Remove(product);
                return;

            case MarketProduct.ProductType.HAND_SOAP:
                HandSoaps.Remove(product);
                return;

            case MarketProduct.ProductType.CLEANING_SPRAY:
                CleaningSprays.Remove(product);
                return;

            case MarketProduct.ProductType.TOOTHPASTE:
                ToothPastes.Remove(product);
                return;


            case MarketProduct.ProductType.HANDS_CREAM:
                HandsCreams.Remove(product);
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
            MarketProduct.ProductType.CHPS => Chips.Contains(product),
            MarketProduct.ProductType.CEREALS => Cereals.Contains(product),
            MarketProduct.ProductType.YOUGOURT => Yougourts.Contains(product),

            MarketProduct.ProductType.PINNEAPPLE => PinneApples.Contains(product),
            MarketProduct.ProductType.ORANGE => Oranges.Contains(product),
            MarketProduct.ProductType.RED_APPLE => RedApples.Contains(product),
            MarketProduct.ProductType.GREEN_APPLE => GreenApples.Contains(product),
            MarketProduct.ProductType.BANANA => Bananas.Contains(product),
            MarketProduct.ProductType.LEMON => Lemons.Contains(product),

            // MarketProduct.ProductType.WINE => wines.Contains(product),
            MarketProduct.ProductType.BEER_BOTTLE => BeerBottles.Contains(product),
            MarketProduct.ProductType.BEER_CAN => BeerCans.Contains(product),
            MarketProduct.ProductType.WATER => Waters.Contains(product),
            MarketProduct.ProductType.WHISKYEY => Whiskeys.Contains(product),
            MarketProduct.ProductType.WINE => Wines.Contains(product),

            MarketProduct.ProductType.TOILET_PAPER => ToiletPapers.Contains(product),
            MarketProduct.ProductType.HAND_SOAP => HandSoaps.Contains(product),
            MarketProduct.ProductType.CLEANING_SPRAY => CleaningSprays.Contains(product),
            MarketProduct.ProductType.TOOTHPASTE => ToothPastes.Contains(product),
            MarketProduct.ProductType.HANDS_CREAM => HandsCreams.Contains(product),
            _ => false
        };
    }

    public bool IsOutOfStock(MarketProduct.ProductType type)
    {
        return type switch
        {
            MarketProduct.ProductType.TUNA_CAN => Tunas.Count == 0,
            MarketProduct.ProductType.MILK => Milks.Count == 0,
            MarketProduct.ProductType.CHPS => Chips.Count == 0,
            MarketProduct.ProductType.CEREALS => Cereals.Count == 0,
            MarketProduct.ProductType.YOUGOURT => Yougourts.Count == 0,

            MarketProduct.ProductType.PINNEAPPLE => PinneApples.Count == 0,
            MarketProduct.ProductType.ORANGE => Oranges.Count == 0,
            MarketProduct.ProductType.RED_APPLE => RedApples.Count == 0,
            MarketProduct.ProductType.GREEN_APPLE => GreenApples.Count == 0,
            MarketProduct.ProductType.BANANA => Bananas.Count == 0,
            MarketProduct.ProductType.LEMON => Lemons.Count == 0,

            MarketProduct.ProductType.BEER_BOTTLE => BeerBottles.Count == 0,
            MarketProduct.ProductType.BEER_CAN => BeerCans.Count == 0,
            MarketProduct.ProductType.WATER => Waters.Count == 0,
            MarketProduct.ProductType.WHISKYEY => Whiskeys.Count == 0,
            MarketProduct.ProductType.WINE => Wines.Count == 0,

            MarketProduct.ProductType.TOILET_PAPER => ToiletPapers.Count == 0,
            MarketProduct.ProductType.HAND_SOAP => HandSoaps.Count == 0,
            MarketProduct.ProductType.CLEANING_SPRAY => CleaningSprays.Count == 0,
            MarketProduct.ProductType.TOOTHPASTE => ToothPastes.Count == 0,
            MarketProduct.ProductType.HANDS_CREAM => HandsCreams.Count == 0,

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
            case MarketProduct.ProductType.CHPS:
                Chips.Add(product.gameObject);
                break;
            case MarketProduct.ProductType.CEREALS:
                Cereals.Add(product.gameObject);
                break;
            case MarketProduct.ProductType.YOUGOURT:
                Yougourts.Add(product.gameObject);
                break;

            case MarketProduct.ProductType.PINNEAPPLE:
                PinneApples.Add(product.gameObject);
                break;
            case MarketProduct.ProductType.ORANGE:
                Oranges.Add(product.gameObject);
                break;
            case MarketProduct.ProductType.RED_APPLE:
                RedApples.Add(product.gameObject);
                break;
            case MarketProduct.ProductType.GREEN_APPLE:
                GreenApples.Add(product.gameObject);
                break;
            case MarketProduct.ProductType.BANANA:
                Bananas.Add(product.gameObject);
                break;
            case MarketProduct.ProductType.LEMON:
                Lemons.Add(product.gameObject);
                break;

            case MarketProduct.ProductType.BEER_BOTTLE:
                BeerBottles.Add(product.gameObject);
                break;
            case MarketProduct.ProductType.BEER_CAN:
                BeerCans.Add(product.gameObject);
                break;
            case MarketProduct.ProductType.WATER:
                Waters.Add(product.gameObject);
                break;
            case MarketProduct.ProductType.WHISKYEY:
                Whiskeys.Add(product.gameObject);
                break;
            case MarketProduct.ProductType.WINE:
                Wines.Add(product.gameObject);
                break;

            case MarketProduct.ProductType.TOILET_PAPER:
                ToiletPapers.Add(product.gameObject);
                break;
            case MarketProduct.ProductType.HAND_SOAP:
                HandSoaps.Add(product.gameObject);
                break;
            case MarketProduct.ProductType.CLEANING_SPRAY:
                CleaningSprays.Add(product.gameObject);
                break;
            case MarketProduct.ProductType.TOOTHPASTE:
                ToothPastes.Add(product.gameObject);
                break;
            case MarketProduct.ProductType.HANDS_CREAM:
                HandsCreams.Add(product.gameObject);
                break;
            default:
                Debug.LogError("Unknown product type: " + type);
                break;
        }
    }
}

