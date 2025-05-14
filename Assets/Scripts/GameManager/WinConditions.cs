using UnityEngine;

public class WinConditions : MonoBehaviour
{
    private int customersHitted = 0;

    private int maxCustomersHitted;

    private void Awake()
    {
        maxCustomersHitted = PlayerPrefs.GetInt("CustomersToSend", 10);
    }

    private void Start()
    {
        EventManager.GetInstance().CustomerHitted += CustomerHitted;
    }

    private void CustomerHitted()
    {
        customersHitted++;

        Debug.Log("Customer Hitted: " + customersHitted);
    }


    public bool PlayerWon()
    {
        return customersHitted >= maxCustomersHitted;
    }
}

