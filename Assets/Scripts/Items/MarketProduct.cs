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
        TUNA,
        MILK,
        ORANGE,
        RED_APPLE,
        GREEN_APPLE,
        RED_WINE,
        WHITE_WINE,
        BEER,
        WATER,
    }

    /// <summary>
    /// The pickProductArea attribute is the area where the custumers can pick up the product.
    /// </summary>
    public Transform pickProductArea;

    /// <summary>
    /// The type attribute is used to define the type of product this MarketProduct represents.
    /// </summary>
    public ProductType type;

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

        MarketStock.GetInstance().RemoveProduct(gameObject);
    }
}
