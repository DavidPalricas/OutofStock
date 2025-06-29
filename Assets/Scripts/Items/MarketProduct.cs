using UnityEngine;

/// <summary>
///  The MarketProduct class is responsible for handling the logic of the products in the market.
///  It implements the ISubject interface to notify its observers (customer tha want this product) when the product is grabbed by the player.
/// </summary>
public class MarketProduct : Item
{
    /// <summary>
    /// The ProductType enum is used to define the different types of products available in the market.
    /// </summary>
    public enum ProductType
    {
        TUNA_CAN,
        MILK,
        CHPS,
        CEREALS,
        YOUGOURT,
        
        PINNEAPPLE,
        ORANGE,
        RED_APPLE,
        GREEN_APPLE,
        LEMON,
        BANANA,
 
        BEER_BOTTLE,
        BEER_CAN,
        WATER,
        WHISKYEY,
        WINE,

        TOILET_PAPER,
        HAND_SOAP,
        CLEANING_SPRAY,
        HANDS_CREAM,
        TOOTHPASTE,
    }

    public GameObject Shelf { get; set; }

    /// <summary>
    /// The pickProductArea attribute is the area where the custumers can pick up the product.
    /// </summary>
    public Transform pickProductArea;

    /// <summary>
    /// The type attribute is used to define the type of product this MarketProduct represents.
    /// </summary>
    public ProductType type;


    public bool canBePicked = false;

    /// <summary>
    /// The WasGrabbed method is responsible for handling the logic when the item is grabbed by the player.
    /// It overrides the base class method to set the grabbed flag to true and notify the observers.
    /// </summary>
    /// <remarks>
    /// This method is override to notify the observers when the item is grabbed by the player.
    /// </remarks>
    public override void WasGrabbed()
    {
        base.WasGrabbed();

        EntityHasProduct();
        Debug.Log($"Product {name} was grabbed by the player.");
    }


    public void EntityHasProduct()
    {
        GameObject.FindGameObjectWithTag("MarketStock").GetComponent<MarketStock>().RemoveProduct(gameObject);

        if (Shelf == null)
        {
            return;
        }

        Shelf.GetComponent<Shelf>().ProductRemoved(gameObject);
    }



    public enum MaterialType
{
    M0,
    M1,
    M2,
    M3,
    M4,
    M5
}

public MaterialType GetMaterialType()
{
    switch (type)
    {
        case ProductType.TUNA_CAN:
        case ProductType.CEREALS:
        case ProductType.CHPS:
            return MaterialType.M0;

        case ProductType.HAND_SOAP:
        case ProductType.CLEANING_SPRAY:
        case ProductType.BEER_CAN:
                return MaterialType.M1;
            
        case ProductType.MILK:
        case ProductType.WATER:
        case ProductType.HANDS_CREAM:
            return MaterialType.M2;

        case ProductType.TOILET_PAPER:
        case ProductType.YOUGOURT:
        case ProductType.TOOTHPASTE:
            return MaterialType.M3;

        case ProductType.BEER_BOTTLE:
        case ProductType.WHISKYEY:
        case ProductType.WINE:
            return MaterialType.M4;

        default:
            return MaterialType.M5; // default fallback
    }
}

}
