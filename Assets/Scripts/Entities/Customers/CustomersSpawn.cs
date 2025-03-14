using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomersSpawn : MonoBehaviour, IObserver
{
    [SerializeField]
    private GameObject customerPrefab;

    [SerializeField]
    private Transform targetItems;

    [SerializeField]
    private int maxCustomersInMarket;

    private int customersSpawned = 0;

    private readonly List<GameObject> targetItemsList = new ();

    private void Awake()
    { 
        AddItems();
    }

    private void Update()
    {
        if (targetItemsList.Count > 0 && customersSpawned < maxCustomersInMarket)
        {
            SpawnCustomer();
        }
    }

    private void AddItems()
    {   
        foreach (Transform item in targetItems)
        {   
            targetItemsList.Add(item.gameObject);
        }
    }

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

        const float CUSTOMERHEIGHT = 3f;

        return new Vector3(randomX, CUSTOMERHEIGHT, randomZ);
    }


    public void UpdateObserver()
    {   StartCoroutine(SpawnCustomerAfterDelay());
        
    }

    private IEnumerator SpawnCustomerAfterDelay()
    {
        yield return new WaitForSeconds(Random.Range(1f, 5f));
        customersSpawned--;
    }
}

