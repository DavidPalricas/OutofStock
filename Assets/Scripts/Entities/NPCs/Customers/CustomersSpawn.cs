using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private GameObject normalCustomerPrefab, annoyingKidPrefab, karenPrefab;

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
    /// The paymentAreas attribute represents the payment areas in the market.
    /// </summary>
    private GameObject[] paymentAreas;

    /// <summary>
    /// The Awake method is called when the script instance is being loaded (Unity Callback).
    /// In this method, the items are added to the targetItemsList by calling the AddItems method.
    /// </summary>
    private void Awake()
    {
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
        if (customersSpawned < maxCustomersInMarket)
        {
            SpawnCustomer();
        }
    }

    /// <summary>
    ///The SpawnCustomer method is responsible for spawning the customers in the market.
    /// </summary>
    /// <remarks>
    /// The SpawnCustomer method instantiates the customer prefab in a random position of the spawn area (GetCustomerPos method).
    /// After that, it sets the customer target item randomly from the MarketProductsList (products in the market), 
    /// this product area position is passed to the customer movement script (AreasPos dictionary) and this script is added
    /// as an observer of the item (ItemLogic class).
    /// The payment area is also set randomly from the paymentAreas array (payment areas in the market), and 
    /// the customer movement script is enable also added as an observer of the customers spawn (this class) 
    // to notify when the customer exits the market.
    /// </remarks>
    private void SpawnCustomer()
    {   
        GameObject customerSterotype = SceneManager.GetActiveScene().buildIndex != 0? GetTypeOfCustomer() : normalCustomerPrefab;

        GameObject customer =  Instantiate(customerSterotype, GetCustomerPos(), Quaternion.identity);

        MarketProduct.ProductType[] productTypes = (MarketProduct.ProductType[])System.Enum.GetValues(typeof(MarketProduct.ProductType));

        MarketProduct.ProductType productType = productTypes[Utils.RandomInt(0, productTypes.Length)];

        MarketStock marketStock = GameObject.FindGameObjectWithTag("MarketStock").GetComponent<MarketStock>();

        if (marketStock.IsOutOfStock(productType))
        {
            return;
        }

        CustomerMovement customerMovement = customer.GetComponent<CustomerMovement>();

        customerMovement.SetTargetProduct(productType);
        customerMovement.AreasPos["MarketExit"] = transform.position;
        customerMovement.AreasPos["Payment"] = paymentAreas[Utils.RandomInt(0, paymentAreas.Length)].transform.position;

        customerMovement.AddObservers(new IObserver[] { this });

        customerMovement.enabled = true;

        customer.GetComponent<FSM>().enabled = true;

        customersSpawned++;
    }

    /// <summary>
    /// The GetTypeOfCustomer method is responsible for getting a random customer prefab based on the spawn probabilities.
    /// </summary>
    /// <remarks>
    /// This method works as follows:
    ///   1. Starts by getting the spawn probabilities of the customers stereotypes from the PlayerPrefs.
    ///   2. It calculates the normal customer spawn probability by subtracting the sum of the other customers spawn probabilities from 1.
    ///   3. Checks if stereotype probabilities are valid (sum <= 1).
    ///   4. Creates a list of KeyValuePair with the customer prefab and its spawn probability.
    ///   5. Generates a random value between 0 and 1.
    ///   6. Iterates through the list of customers and their spawn probabilities, checking if the random value is less than or equal to the spawn probability.
    ///   7. If it finds a match, it returns the corresponding customer prefab, otherwise returns the hightest probability prefab from the list.
    ///  
    /// </remarks>
    /// <returns> A prefab of a customer to spawn</returns>
    private GameObject GetTypeOfCustomer()
    {
        float karenSpawnProb = PlayerPrefs.GetFloat("KarenSpawnProb");
        float annoyinKidSpawnProb = PlayerPrefs.GetFloat("AnnoyingKidSpawnProb");
        float normalCustomerSpawnProb = 1f - (karenSpawnProb + annoyinKidSpawnProb);


        List<KeyValuePair<GameObject, float>> customersSpawnProbs = new()
        {
           new KeyValuePair<GameObject, float>(karenPrefab, karenSpawnProb),
           new KeyValuePair<GameObject, float>(annoyingKidPrefab, annoyinKidSpawnProb),
           new KeyValuePair<GameObject, float>(normalCustomerPrefab, normalCustomerSpawnProb),
        };

        customersSpawnProbs.OrderBy(customersSpawnProb => customersSpawnProb.Value);

        float randomValue = Utils.RandomFloat(0f, 1f);

        foreach (KeyValuePair<GameObject, float> customerSpawnProb in customersSpawnProbs)
        {    
            if (randomValue <= customerSpawnProb.Value) { 
                return customerSpawnProb.Key;
            }
        }

        return customersSpawnProbs[^1].Key;
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
    private void CustomerExitMarket(GameObject customerSent)
    {
        customersSpawned--;
        Destroy(customerSent);
    }

    /// <summary>
    /// The OnNotify (IObserver method) method is responsible for updating the observer (this game object), when a subject notifies it.
    /// In this case, it calls the CustomerExitMarket method after a random time between 1 and 5 seconds to simulate the customer leaving the market.
    /// </summary>
    /// <param name="data">Any argument to be sent to the observer, in this case no argument is specified (null)</param>
    public void OnNotify(object data = null)
    {   StartCoroutine(Utils.WaitAndExecute(Utils.RandomFloat(1f, 5f),()=> CustomerExitMarket(data as GameObject)));
        
    }
}

