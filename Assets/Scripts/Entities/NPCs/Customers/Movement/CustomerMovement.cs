using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The CustomerMovement class is responsible for handling the customer movement in the market.
/// It inherits from the NPCMovement class the basics of the NPC movement and implements the ISubject and IObserver interfaces.
/// It is a subject to the customers spawn and an observer to the item he is looking for.
/// </summary>
public class CustomerMovement : NPCMovement, ISubject
{
    /// <summary>
    /// The observers attribute stores the observers of the customer.
    /// In this case, there is only one observer (the customers spawn)
    /// This observer is notified when the customer exits the market to spawn a new customer.
    /// </summary>
    private IObserver[] observers;

    public Transform backPack;

    /// <summary>
    /// The TargetItem attribute represents the target item of the customer.
    /// </summary>
    /// <value>
    /// The target item of the customer.
    /// </value>
    public MarketProduct TargetProduct { get; private set; }

    /// <summary>
    /// The AreasPos attribute represents the positions of the areas in which 
    ///  the customer will move depending on its current state.
    /// </summary>
    public Dictionary<string, Vector3> AreasPos = new ()
    {
        { "Product", Vector3.zero},
        { "Payment", Vector3.zero },
        { "MarketExit", Vector3.zero }
    };

    /// <summary>
    /// The ExitMarket method is responsible for handling the logic when the customer exits the market.
    /// In this method, the observers are notified and removed, and the customer game object is destroyed.
    /// </summary>
    public void ExitMarket()
    {
        NotifyObservers(gameObject);
        RemoveObservers();
    }

    /// <summary>
    /// The SetAgentDestination method is responsible for setting the agent destination and enabling its movement.
    /// The destination can be the pick item area, the payment area, or the market exit area, dpeending on the current state of the customer.
    /// This method overrides the <see cref="NPCMovement.SetAgentDestination"/> method from the <see cref="NPCMovement"/> class.
    /// </summary>
    public override void SetAgentDestination(Vector3 destination)
    {   
        base.SetAgentDestination(destination);
    }

    /// <summary>
    /// The RemoveObservers method is responsible for removing the observers from the subject (ISubject interface method).
    /// </summary>
    public void RemoveObservers()
    {
        observers = null;
    }

    /// <summary>
    /// The NotifyObservers method is responsible for notifying the customer observers (ISubject interface method).
    /// </summary>
    /// <param name="data">Any argument to be sent to the observer, in this case no argument is specified (null)</param>
    public void NotifyObservers(object data = null)
    {
        if (observers == null)
        {
            return;
        }

        foreach (IObserver observer in observers)
        {   
           
            observer.OnNotify(data);
        }
    }

    /// <summary>
    /// The AddObserver method is responsible for adding observers to the customer (ISubject interface method).
    /// </summary>
    /// <param name="observers">The observers (only one the customers spawn).</param>
    public void AddObservers(IObserver[] observers)
    {   
        this.observers = observers;
    }

    
    public void SetTargetProduct(MarketProduct.ProductType productType)
    {
        MarketStock marketStock =  GameObject.FindGameObjectWithTag("MarketStock").GetComponent<MarketStock>();

        switch (productType)
        {
            case MarketProduct.ProductType.TUNA_CAN:
                TargetProduct = marketStock.Tunas[Utils.RandomInt(0, marketStock.Tunas.Count)].GetComponent<MarketProduct>();
                AreasPos["Product"] = TargetProduct.pickProductArea == null ? Vector3.zero : TargetProduct.pickProductArea.position;
                return;

            case MarketProduct.ProductType.MILK:
                TargetProduct = marketStock.Milks[Utils.RandomInt(0, marketStock.Milks.Count)].GetComponent<MarketProduct>();
                AreasPos["Product"] = TargetProduct.pickProductArea == null ? Vector3.zero : TargetProduct.pickProductArea.position;
                return;

            case MarketProduct.ProductType.CHPS:
                TargetProduct = marketStock.Chips[Utils.RandomInt(0, marketStock.Chips.Count)].GetComponent<MarketProduct>();
                AreasPos["Product"] = TargetProduct.pickProductArea == null ? Vector3.zero : TargetProduct.pickProductArea.position;
                return;

            case MarketProduct.ProductType.CEREALS:
                TargetProduct = marketStock.Cereals[Utils.RandomInt(0, marketStock.Cereals.Count)].GetComponent<MarketProduct>();
                AreasPos["Product"] = TargetProduct.pickProductArea == null ? Vector3.zero : TargetProduct.pickProductArea.position;
                return;

            case MarketProduct.ProductType.YOUGOURT:
                TargetProduct = marketStock.Yougourts[Utils.RandomInt(0, marketStock.Yougourts.Count)].GetComponent<MarketProduct>();
                AreasPos["Product"] = TargetProduct.pickProductArea == null ? Vector3.zero : TargetProduct.pickProductArea.position;
                return;


            case MarketProduct.ProductType.PINNEAPPLE:
                TargetProduct = marketStock.PinneApples[Utils.RandomInt(0, marketStock.PinneApples.Count)].GetComponent<MarketProduct>();
                AreasPos["Product"] = TargetProduct.pickProductArea == null ? Vector3.zero : TargetProduct.pickProductArea.position;
                return;

            case MarketProduct.ProductType.ORANGE:
                Debug.Log("Customer Wants Orange");
                TargetProduct = marketStock.Oranges[Utils.RandomInt(0, marketStock.Oranges.Count)].GetComponent<MarketProduct>();
                AreasPos["Product"] = TargetProduct.pickProductArea == null ? Vector3.zero : TargetProduct.pickProductArea.position;
                return;

            case MarketProduct.ProductType.RED_APPLE:
                TargetProduct = marketStock.RedApples[Utils.RandomInt(0, marketStock.RedApples.Count)].GetComponent<MarketProduct>();
                AreasPos["Product"] = TargetProduct.pickProductArea == null ? Vector3.zero : TargetProduct.pickProductArea.position;
                return;

            case MarketProduct.ProductType.GREEN_APPLE:
                TargetProduct = marketStock.GreenApples[Utils.RandomInt(0, marketStock.GreenApples.Count)].GetComponent<MarketProduct>();
                AreasPos["Product"] = TargetProduct.pickProductArea == null ? Vector3.zero : TargetProduct.pickProductArea.position;
                return;


            case MarketProduct.ProductType.LEMON:
                TargetProduct = marketStock.Lemons[Utils.RandomInt(0, marketStock.Lemons.Count)].GetComponent<MarketProduct>();
                AreasPos["Product"] = TargetProduct.pickProductArea == null ? Vector3.zero : TargetProduct.pickProductArea.position;
                return;


            case MarketProduct.ProductType.BANANA:
                TargetProduct = marketStock.Bananas[Utils.RandomInt(0, marketStock.Bananas.Count)].GetComponent<MarketProduct>();
                AreasPos["Product"] = TargetProduct.pickProductArea == null ? Vector3.zero : TargetProduct.pickProductArea.position;
                return;

            case MarketProduct.ProductType.BEER_BOTTLE:
                TargetProduct = marketStock.BeerBottles[Utils.RandomInt(0, marketStock.BeerBottles.Count)].GetComponent<MarketProduct>();
                AreasPos["Product"] = TargetProduct.pickProductArea == null ? Vector3.zero : TargetProduct.pickProductArea.position;
                return;

            case MarketProduct.ProductType.BEER_CAN:
                TargetProduct = marketStock.BeerCans[Utils.RandomInt(0, marketStock.BeerCans.Count)].GetComponent<MarketProduct>();
                AreasPos["Product"] = TargetProduct.pickProductArea == null ? Vector3.zero : TargetProduct.pickProductArea.position;
                return;

            
            case MarketProduct.ProductType.WATER:
                Debug.Log("Customer Wants Water");
                TargetProduct = marketStock.Waters[Utils.RandomInt(0, marketStock.Waters.Count)].GetComponent<MarketProduct>();
                AreasPos["Product"] = TargetProduct.pickProductArea == null ? Vector3.zero : TargetProduct.pickProductArea.position;
                return;


            case MarketProduct.ProductType.WHISKYEY:
                TargetProduct = marketStock.Whiskeys[Utils.RandomInt(0, marketStock.Whiskeys.Count)].GetComponent<MarketProduct>();
                AreasPos["Product"] = TargetProduct.pickProductArea == null ? Vector3.zero : TargetProduct.pickProductArea.position;
                return;

            case MarketProduct.ProductType.WINE:
                TargetProduct = marketStock.Wines[Utils.RandomInt(0, marketStock.Wines.Count)].GetComponent<MarketProduct>();
                AreasPos["Product"] = TargetProduct.pickProductArea == null ? Vector3.zero : TargetProduct.pickProductArea.position;
                return;


            case MarketProduct.ProductType.TOILET_PAPER:
                TargetProduct = marketStock.ToiletPapers[Utils.RandomInt(0, marketStock.ToiletPapers.Count)].GetComponent<MarketProduct>();
                AreasPos["Product"] = TargetProduct.pickProductArea == null ? Vector3.zero : TargetProduct.pickProductArea.position;
                return;

            case MarketProduct.ProductType.HAND_SOAP:
                TargetProduct = marketStock.HandSoaps[Utils.RandomInt(0, marketStock.HandSoaps.Count)].GetComponent<MarketProduct>();
                AreasPos["Product"] = TargetProduct.pickProductArea == null ? Vector3.zero : TargetProduct.pickProductArea.position;
                return;

            case MarketProduct.ProductType.CLEANING_SPRAY:
                TargetProduct = marketStock.CleaningSprays[Utils.RandomInt(0, marketStock.CleaningSprays.Count)].GetComponent<MarketProduct>();
                AreasPos["Product"] = TargetProduct.pickProductArea == null ? Vector3.zero : TargetProduct.pickProductArea.position;
                return;


            case MarketProduct.ProductType.HANDS_CREAM:
                TargetProduct = marketStock.HandsCreams[Utils.RandomInt(0, marketStock.HandsCreams.Count)].GetComponent<MarketProduct>();
                AreasPos["Product"] = TargetProduct.pickProductArea == null ? Vector3.zero : TargetProduct.pickProductArea.position;
                return;

            case MarketProduct.ProductType.TOOTHPASTE:
                TargetProduct = marketStock.ToothPastes[Utils.RandomInt(0, marketStock.ToothPastes.Count)].GetComponent<MarketProduct>();
                AreasPos["Product"] = TargetProduct.pickProductArea == null ? Vector3.zero : TargetProduct.pickProductArea.position;
                return;

            default:
                Debug.LogError("Invalid product type selected: " + productType);
                AreasPos["Product"] = TargetProduct.pickProductArea == null ? Vector3.zero : TargetProduct.pickProductArea.position;
                TargetProduct = null;
                return;
        }
      }
}
