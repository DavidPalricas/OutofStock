using System.Linq;
using UnityEngine;

public class Shelf : MonoBehaviour, ISubject
{
    private int maxProducts, currentProducts;

    private IObserver[] observers;

    private void Awake()
    {
        maxProducts = GameObject.FindGameObjectsWithTag("Item")
           .Count(item => item.TryGetComponent(out MarketProduct product) && product.shelf == gameObject);
        maxProducts = GameObject.FindGameObjectsWithTag("Item").Where(item => item.GetComponent<MarketProduct>() != null && item.GetComponent<MarketProduct>().shelf == gameObject ).Count();

        currentProducts = maxProducts;

        IObserver taskManager = GameObject.FindGameObjectWithTag("TaskManager").GetComponent<TaskManager>();

        AddObservers(new IObserver[] { taskManager });
    }

    public void ProductRemoved()
    {
        currentProducts--;

        if (currentProducts <= 0)
        {
            NotifyObservers(gameObject);
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
