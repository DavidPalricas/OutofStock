using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The CustomersSpawn class is responsible for spawning the customers in the market, simulating the customers entering the market.
/// </summary>
public class CustomersSpawn : MonoBehaviour, IObserver
{
    /// <summary>
    /// The customerPrefab attribute represents the customer prefab.
    /// </summary>
    [SerializeField]
    private GameObject customerPrefab;

    /// <summary>
    /// The targetItems attribute represents the target items in the market.
    /// </summary>
    [SerializeField]
    private Transform targetItems;

    /// <summary>
    /// The maximumCustomersInMarket attribute represents the maximum number of customers in the market.
    /// </summary>
    [SerializeField]
    private int maxCustomersInMarket;

    /// <summary>
    /// The customersSpawned attribute represents the number of customers spawned.
    /// </summary>
    private int customersSpawned = 0;

    /// <summary>
    /// The targetItemsList attribute represents the list of  the customers target items in the market.
    /// </summary>
    private readonly List<GameObject> targetItemsList = new ();

    /// <summary>
    /// The Awake method is called when the script instance is being loaded (Unity Callback).
    /// In this method, the items are added to the targetItemsList by calling the AddItems method.
    /// </summary>
    private void Awake()
    { 
        AddItems();
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
    /// The AddItems method is responsible for adding the items to the targetItemsList.
    /// </summary>
    private void AddItems()
    {   
        foreach (Transform item in targetItems)
        {   
            targetItemsList.Add(item.gameObject);
        }
    }

    /// <summary>
    ///The SpawnCustomer method is responsible for spawning the customers in the market.
    /// </summary>
    /// <remarks>
    /// The SpawnCustomer method instantiates the customer prefab in a random position of the spawb area (GetCustomerPos method).
    /// After that, it sets the customer target item randomly from the targetItemsList (items in the market), 
    /// adds the location of the exit, adds this class as an obersever of the customer and enables its movement script.
    /// </remarks>
    private void SpawnCustomer()
    {    
        GameObject customer = Instantiate(customerPrefab, GetCustomerPos(), Quaternion.identity);

        CustomerMovement customerMovement = customer.GetComponent<CustomerMovement>();

        GameObject targetItem = targetItemsList[new System.Random().Next(targetItemsList.Count)];

        targetItemsList.Remove(targetItem);

        customerMovement.TargetItem = targetItem;
        customerMovement.MarketExitPos = transform.position;
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

        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);

        const float CUSTOMERPOSY = 3f;

        return new Vector3(randomX, CUSTOMERPOSY, randomZ);
    }

    /// <summary>
    /// The SpawnCustomerAfterDelay method is responsible for spawning the customers after a random delay.
    /// </summary>
    /// <returns>IEnumerator to handle the delay before spawning the next customer.</returns>
    private IEnumerator SpawnCustomerAfterDelay()
    {
        yield return new WaitForSeconds(Random.Range(1f, 5f));
        customersSpawned--;
    }

    /// <summary>
    /// The UpdateObserver method is responsible for updating the observer (IObserver interface method).
    /// </summary>
    public void UpdateObserver()
    {   StartCoroutine(SpawnCustomerAfterDelay());
        
    }
}

