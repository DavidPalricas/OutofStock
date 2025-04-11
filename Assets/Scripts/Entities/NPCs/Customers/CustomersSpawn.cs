using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The CustomersSpawn class is responsible for spawning the customers in the market, simulating the customers entering the market.
/// It implements the IObserver interface to be notified when a customer (subject) leaves the market
/// </summary>
public class CustomersSpawn : MonoBehaviour, IObserver
{
    /// <summary>
    /// The customerPrefab attribute represents the customer prefab.
    /// </summary>
    [SerializeField]
    private GameObject customerPrefab;

    /// <summary>
    /// The maximumCustomersInMarket attribute represents the maximum number of customers in the market.
    /// </summary>
    [SerializeField]
    private int maxCustomersInMarket;

    /// <summary>
    /// The paymentAreasTransform attribute represents the payment areas transform.
    /// </summary>
    [SerializeField]
    private Transform paymentAreasTransform;

    /// <summary>
    /// The customersSpawned attribute represents the number of customers spawned.
    /// </summary>
    private int customersSpawned = 0;

    /// <summary>
    /// The targetItemsList attribute represents the list of  the customers target items in the market.
    /// </summary>
    private  List<GameObject> targetItemsList = new ();

    /// <summary>
    /// The paymentAreas attribute represents the payment areas in the market.
    /// </summary>
    private GameObject[] paymentAreas;

    /// <summary>
    /// The Awake method is called when the script instance is being loaded (Unity Callback).
    /// In this method, the items are added to the targetItemsList by calling the AddItems method.
    /// </summary>
    private void Awake()
    { 
        targetItemsList = GameObject.FindGameObjectsWithTag("Item").ToList();
        paymentAreas = Utils.GetChildren(paymentAreasTransform);
    }

    /// <summary>
    /// The Update method is called every frame (Unity Callback).
    /// In this method, the customers are spawned in the market by calling the SpawnCustomer method.
    /// </summary>
    /// <remarks>
    /// To call the SpawnCustomer method, the market must have the items the clients are looking for (targetItemsList.Count > 0) and the number of customers spawned must be less than the maximum number of customers in the market (customersSpawned < maxCustomersInMarket).
    /// </remarks>
    private void Update()
    {
        if (targetItemsList.Count > 0 && customersSpawned < maxCustomersInMarket)
        {
            SpawnCustomer();
        }
    }

    /// <summary>
    ///The SpawnCustomer method is responsible for spawning the customers in the market.
    /// </summary>
    /// <remarks>
    /// The SpawnCustomer method instantiates the customer prefab in a random position of the spawn area (GetCustomerPos method).
    /// After that, it sets the customer target item randomly from the targetItemsList (items in the market), 
    /// this item area position is passed to the customer movement script (AreasPos dictionary) and this script is added
    /// as an observer of the item (ItemLogic class).
    /// The payment area is also set randomly from the paymentAreas array (payment areas in the market), and 
    /// the customer movement script is enable also added as an observer of the customers spawn (this class) 
    // to notify when the customer exits the market.
    /// </remarks>
    private void SpawnCustomer()
    {    
        GameObject customer = Instantiate(customerPrefab, GetCustomerPos(), Quaternion.identity);

        CustomerMovement customerMovement = customer.GetComponent<CustomerMovement>();

        GameObject targetItem = targetItemsList[Utils.RandomInt(0, targetItemsList.Count)];

        targetItemsList.Remove(targetItem);

        customerMovement.TargetItem = targetItem;

        targetItem.GetComponent<MarketProduct>().AddObservers(new IObserver[] { customerMovement });

        Transform pickItemArea = targetItem.GetComponent<MarketProduct>().pickProductArea;

        customerMovement.AreasPos["PickItem"] = pickItemArea == null ? Vector3.zero : pickItemArea.position;
        customerMovement.AreasPos["MarketExit"] = transform.position;
        customerMovement.AreasPos["Payment"] = paymentAreas[Utils.RandomInt(0, paymentAreas.Length)].transform.position;

        customerMovement.AddObservers(new IObserver[] { this });

        customerMovement.enabled = true;

        customersSpawned++;
    }

    /// <summary>
    /// The GetCustomerPos method is responsible for getting a random position in the spawn area (plane).
    /// The Y coordinted of the customers is fixed (const CUSTOMERPOSY).
    /// </summary>
    /// <returns>The customer spawn position</returns>
    private Vector3 GetCustomerPos()
    {
        Vector3 planePos = transform.position;

        Vector3 planeSize = transform.localScale;

        float minX = planePos.x - planeSize.x / 2;
        float maxX = planePos.x + planeSize.x / 2;
        float minZ = planePos.z - planeSize.z / 2;
        float maxZ = planePos.z + planeSize.z / 2;

        float randomX = Utils.RandomFloat(minX, maxX);
        float randomZ = Utils.RandomFloat(minZ, maxZ);

        const float CUSTOMERPOSY = 3f;

        return new Vector3(randomX, CUSTOMERPOSY, randomZ);
    }

    /// <summary>
    /// The CustomerExitMarket method is responsible for removing a customer from the market.
    /// By decrementing the customersSpawned attribute, it triggers in the Update method to spawn a new customer.
    /// </summary>
    private void CustomerExitMarket()
    {
        customersSpawned--;
    }

    /// <summary>
    /// The UpdateObserver method is responsible for updating the observer (IObserver interface method).
    /// In this case, it calls the CustomerExitMarket method after a random time between 1 and 5 seconds to simulate the customer leaving the market.
    /// </summary>
    /// <param name="data">Any argument to be sent to the observer, in this case no argument is specified (null)</param>
    public void UpdateObserver(object data = null)
    {   StartCoroutine(Utils.WaitAndExecute(Utils.RandomFloat(1f, 5f),()=> CustomerExitMarket()));
        
    }
}

