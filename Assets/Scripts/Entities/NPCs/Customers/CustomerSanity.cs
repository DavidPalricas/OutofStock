using UnityEngine;
using UnityEngine.UI;

public class CustomerSanity : MonoBehaviour, IEventDispatcher
{
    [SerializeField]
    private Slider sanitySlider;

    [SerializeField]
    private GameObject sliderContainer;


    private Transform playerTransform;


    private Camera mainCamera;


    [Range(0, 100)]
    public float maxSanity;


    public float CurrentSanity { get; private set; }

    private void Awake()
    {
        CurrentSanity = maxSanity;
        mainCamera = Camera.main;
        sliderContainer.SetActive(false);
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void ShowSanityBar()
    {
       if (CurrentSanity > 0)
        {
            sliderContainer.transform.rotation = mainCamera.transform.rotation;
            sliderContainer.SetActive(true);
        } 
    }



    private void Update()
    {
        if (sliderContainer.activeSelf)
        {
            sliderContainer.transform.rotation = mainCamera.transform.rotation;

            if (Vector3.Distance(playerTransform.position, transform.position) > 5f)
            {
                sliderContainer.SetActive(false);
            }
        }
    }




    public void DecreasedSanity()
    {    
        if (CurrentSanity > 0)
        {
            CurrentSanity -= 50f;

            if (CurrentSanity <= 0)
            {   
                sliderContainer.gameObject.SetActive(false);
                DispatchEvents();
            }

            sanitySlider.value = CurrentSanity / maxSanity;
            return;
        }
    }

    /// <summary>
    /// The DispatchEvent method is responsible for dispatching the event when a customer exits the market.
    /// </summary>
    public void DispatchEvents()
    {
        EventManager.GetInstance().OnCustomerSent();
    }
}
