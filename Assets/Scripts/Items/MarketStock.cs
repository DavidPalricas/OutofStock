using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MarketStock
{   
    private static MarketStock instance = null;

    public List<GameObject> tunas, milks, oranges, redApples, greenApples, redWines, whiteWines, beers, waters;

    private MarketStock()
    {
        SetStock();
    }

    public static MarketStock GetInstance()
    {
        return instance ??= new MarketStock();
    }


    private void SetStock()
    {
        GameObject[] marketProducts = GameObject.FindGameObjectsWithTag("Item")
           .Where(item => item.GetComponent<MarketProduct>() != null)
           .ToArray();

        tunas = marketProducts.Where(item => item.GetComponent<MarketProduct>().Type == MarketProduct.ProductType.TUNA).ToList();
        milks = marketProducts.Where(item => item.GetComponent<MarketProduct>().Type == MarketProduct.ProductType.MILK).ToList();
        oranges = marketProducts.Where(item => item.GetComponent<MarketProduct>().Type == MarketProduct.ProductType.ORANGE).ToList();
        redApples = marketProducts.Where(item => item.GetComponent<MarketProduct>().Type == MarketProduct.ProductType.RED_APPLE).ToList();
        greenApples = marketProducts.Where(item => item.GetComponent<MarketProduct>().Type == MarketProduct.ProductType.GREEN_APPLE).ToList();
        redWines = marketProducts.Where(item => item.GetComponent<MarketProduct>().Type == MarketProduct.ProductType.RED_WINE).ToList();
        whiteWines = marketProducts.Where(item => item.GetComponent<MarketProduct>().Type == MarketProduct.ProductType.WHITE_WINE).ToList();
        beers = marketProducts.Where(item => item.GetComponent<MarketProduct>().Type == MarketProduct.ProductType.BEER).ToList();
        waters = marketProducts.Where(item => item.GetComponent<MarketProduct>().Type == MarketProduct.ProductType.WATER).ToList();
    }

    public void RemoveProduct(GameObject product)
    {
        MarketProduct.ProductType type = product.GetComponent<MarketProduct>().Type;

        switch (type)
        {
            case MarketProduct.ProductType.TUNA:
                tunas.Remove(product);
                return;

            case MarketProduct.ProductType.MILK:
                milks.Remove(product);
                return;

            case MarketProduct.ProductType.ORANGE:
                oranges.Remove(product);
                return;

            case MarketProduct.ProductType.RED_APPLE:
                redApples.Remove(product);
                return;

            case MarketProduct.ProductType.GREEN_APPLE:
                greenApples.Remove(product);
                return;

            case MarketProduct.ProductType.RED_WINE:
                redWines.Remove(product);
                return;

            case MarketProduct.ProductType.WHITE_WINE:
                whiteWines.Remove(product);
                return;

            case MarketProduct.ProductType.BEER:
                beers.Remove(product);
                return;

            case MarketProduct.ProductType.WATER:
                waters.Remove(product);
                return;

            default:
                Debug.LogError("Unknown product type: " + type);
                return;
        }
    }


    public bool IsProductAvaible(GameObject product)
    {
        MarketProduct.ProductType type = product.GetComponent<MarketProduct>().Type;

        return type switch
        {
            MarketProduct.ProductType.TUNA => tunas.Contains(product),
            MarketProduct.ProductType.MILK => milks.Contains(product),
            MarketProduct.ProductType.ORANGE => oranges.Contains(product),
            MarketProduct.ProductType.RED_APPLE => redApples.Contains(product),
            MarketProduct.ProductType.GREEN_APPLE => greenApples.Contains(product),
            MarketProduct.ProductType.RED_WINE => redWines.Contains(product),
            MarketProduct.ProductType.WHITE_WINE => whiteWines.Contains(product),
            MarketProduct.ProductType.BEER => beers.Contains(product),
            MarketProduct.ProductType.WATER => waters.Contains(product),
            _ => false
        };
    }

    public bool IsOutOfStock(MarketProduct.ProductType type)
    {
        return type switch
        {
            MarketProduct.ProductType.TUNA => tunas.Count == 0,
            MarketProduct.ProductType.MILK => milks.Count == 0,
            MarketProduct.ProductType.ORANGE => oranges.Count == 0,
            MarketProduct.ProductType.RED_APPLE => redApples.Count == 0,
            MarketProduct.ProductType.GREEN_APPLE => greenApples.Count == 0,
            MarketProduct.ProductType.RED_WINE => redWines.Count == 0,
            MarketProduct.ProductType.WHITE_WINE => whiteWines.Count == 0,
            MarketProduct.ProductType.BEER => beers.Count == 0,
            MarketProduct.ProductType.WATER => waters.Count == 0,
            _ => false
        };
    }
}

