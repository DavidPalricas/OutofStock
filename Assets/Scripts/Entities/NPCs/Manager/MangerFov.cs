using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The ManagerFov class is used to represent the field of view of the manager by handling if detects a player hitting a customer 
/// </summary>
public class MangerFov : MonoBehaviour
{
    /// <summary>
    /// The strikes attribute is used to store the number of strikes the player has.
    /// </summary>
    private int strikes = 0;

    /// <summary>
    /// The targetsMask and obstructionMask attributes are used to store the layers that the manager can see and the layers that can obstruct the view, respectively.
    /// </summary>
    [SerializeField]
    private LayerMask targetsMask, obstructionMask;

    /// <summary>
    /// The VIEWDISTNACE constant is used to determine the maximum number of colliders that can be detected by raycasting
    /// </summary>
    private const int VIEWDISTANCE = 10;

    /// <summary>
    /// The radius atribute is used to store the radius of the field of view of the manager.
    /// </summary>
    public float radius;

    /// <summary>
    /// The angle attribute is used to store the angle of the field of view of the manager.
    /// It is set to a range of 0 to 360 degrees.
    /// </summary>
    [Range(0, 360)]
    public float angle;

    /// <summary>
    /// The TargetsSeen atribute is a flag to indicate if the manager has seen the "targets".
    /// These targets are the player or one or more customers.
    /// </summary>
    /// <value>
    ///   <c>true</c> if [targets seen]; otherwise, <c>false</c>.
    /// </value>
    public bool TargetsSeen { get; private set; }

    /// <summary>
    /// The Player attribute is used to store a reference to the player GameObject.
    /// </summary>
    /// <value>
    /// The reference to the player GameObject.
    /// </value>
    public GameObject Player { get; private set; }

    /// <summary>
    /// The CustomersHitted attribute is used to store a list of customers that have been hit by player and seen by the manager.
    /// </summary>
    /// <value>
    /// A list of references to the customers GameObjects who have been seen by the manager.
    /// </value>
    public List<GameObject> CustomersSeen { get; private set; }

    /// <summary>
    /// The Awake method is called when the script instance is being loaded (Unity callback).
    /// </summary>
    /// <remarks>
    /// In this method, the EventDispatcher instance is obtained and adds this class as a listener to the "CustomerAttacked" event.
    /// Other attributes are initialized, such as the Player attribute, the CustomersHitted list and the TargetsSeen flag.
    /// After all atributes are set, the FOVRoutine coroutine is started.
    /// </remarks>
    private void Awake()
    {
      // eventDispatcher = EventDispatcher.GetInstance();
       // eventDispatcher.AddListener("CustomerAttacked", this);

      Player = GameObject.FindGameObjectWithTag("Player");

      CustomersSeen = new List<GameObject>();

      TargetsSeen = false;

      StartCoroutine(FovRoutine());
    }

    /// <summary>
    /// the FovRoutine method is used to periodically check the field of view of the manager, it has a delay of 0.2 seconds.
    /// </summary>
    /// <returns> An IEnumerator that can be used in a coroutine to wait and execute the specified method.</returns> 
    private IEnumerator FovRoutine()
    {
        const float DELAY = 0.2f;

        var wait = new WaitForSeconds(DELAY);

        while (true)
        {
            yield return wait;
            FOVCheck();
        }
    }

    /// <summary>
    /// The FOVCheck method is used to check the field of view of the manager.
    /// </summary>
    private void FOVCheck()
    {
        /* To avoid memory allocation problems,a fixed size array is used to store the colliders
        /  It has a size of VIEWDISTANCE + 1 because the raycast can detect the manager itself
        */
        var targetsSeen = new Collider[VIEWDISTANCE + 1]; 

        int numTargetsSeen = Physics.OverlapSphereNonAlloc(transform.position, radius, targetsSeen, targetsMask);
       
        if (numTargetsSeen > 0)
        {      
            var directionsToTargets = new List<Vector3>();

            bool playerFound = false;

            for (int i = 0; i < numTargetsSeen; i++)
            {
                Transform target = targetsSeen[i].transform;

                if (target.CompareTag("Player"))
                {
                    playerFound = true;
                    directionsToTargets.Add((target.position - transform.position).normalized);
                }
                else if (target.CompareTag("Customer"))
                {
                    directionsToTargets.Add((target.position - transform.position).normalized);
                    CustomersSeen.Add(target.gameObject);
                }
            }

            if (directionsToTargets.Count >= 2 && playerFound)
            {   
                CheckIfTargetsAreInFov(directionsToTargets);
                return;
            }        
        }
     
        ResetTargetsAtributes();
    }

    /// <summary>
    /// The CheckIfTargetsAreInFov method is used to check if the targets are in the field of view of the manager.
    /// </summary>
    /// <remarks>
    /// For each target direction, this method starts by checking if the targets are in the field of view of the manager.
    /// After thar a raycast to the fov obstruction mask is casted with the direction and it maximum distance is the targets distance to the manager.
    /// If this raycast hits something, it means there is an obstruction and manager can't see the targets, so the targets atributes are reset (ResetTargetsAtributes method is called).
    /// Otherwise , the TargetsSeen flag is set to true.
    /// </remarks>
    /// <param name="directionsToTargets">A list with the targets directions to the manager</param>
    private void CheckIfTargetsAreInFov(List<Vector3> directionsToTargets)
    {
        // To avoid memory allocation problems,a fixed size array is used to store the raycast hits
       var hits = new RaycastHit[VIEWDISTANCE]; 

        foreach (Vector3 direction in directionsToTargets)
        {
            // Checks if the target is in the field of view of the manager
            if (Vector3.Angle(transform.forward, direction) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, direction);

                int hitCount = Physics.RaycastNonAlloc(transform.position, direction, hits, distanceToTarget, obstructionMask);

                if (hitCount > 0)
                {
                    ResetTargetsAtributes();
                    return;
                }
            }
        }

        TargetsSeen = true;
        CheckIfPlayerAttackedCustomer();
    }

    /// <summary>
    /// The ResetTargetsAtributes method is used to reset the attributes related to the targets.
    /// These atributes are the TargetsSeen flag and the CustomersHitted list.
    /// </summary>
    private void ResetTargetsAtributes()
    {
        TargetsSeen = false;   
        CustomersSeen.Clear();
    }



    private void CheckIfPlayerAttackedCustomer()
    {   
        GameObject customerAttacked = EventManager.GetInstance().CustomerAttacked;

        if ( customerAttacked != null && CustomersSeen.Contains(customerAttacked))
        {
            strikes++;
            EventManager.GetInstance().CustomerAttacked = null;

            const int MAXSTRIKES = 3;

            if (strikes >= MAXSTRIKES)
            {
                Debug.Log("You're fired!");

                return;
            }

            Debug.Log("Strike N� " + strikes);
        }
    }
}
