using UnityEngine;

/// <summary>
///  The MarketProduct class is responsible for handling the logic of the products in the market.
///  It implements the ISubject interface to notify its observers (customer tha want this product) when the product is grabbed by the player.
/// </summary>
public class MarketProduct : Item, ISubject
{
    /// <summary>
    /// The observers attribute is used to store the observers of the product.
    /// In this case there is only one observer, the customer who is looking for the product.
    /// </summary>
    private IObserver[] observers;

    /// <summary>
    /// The pickProductArea attribute is the area where the custumers can pick up the product.
    /// </summary>
    public Transform pickProductArea;

    /// <summary>
    /// The AddObserver method is responsible for adding observers to the customer (ISubject interface method).
    /// </summary>
    /// <param name="data">Any argument to be sent to the observer, in this case no argument is specified (null)</param>s
    public void AddObservers(IObserver[] observers)
    {
        this.observers = observers;
    }

    /// <summary>
    /// The NotifyObservers method is responsible for notifying the customer observers (ISubject interface method).
    /// </summary>
    /// <param name="data">Any argument to be sent to the observer, in this case no argument is specified (null)</param>
    public void NotifyObservers(object data = null)
    {
        foreach (IObserver observer in observers)
        {
            observer.OnNotify();
        }

        RemoveObservers();
    }

    /// <summary>
    /// The RemoveObservers method is responsible for removing the observers from the subject (ISubject interface method).
    /// </summary>
    public void RemoveObservers()
    {
        observers = null;
    }

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

        if (observers != null)
        {
            NotifyObservers();
        }
    }
}
