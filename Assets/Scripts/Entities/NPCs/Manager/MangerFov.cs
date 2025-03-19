using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MangerFov : MonoBehaviour, IEventListener
{   
    private int strikes = 0;

    private EventDispatcher eventDispatcher;

    [SerializeField]
    private LayerMask targetsMask, obstructionMask;

    public float radius;

    [Range(0, 360)]
    public float angle;

    public bool TargetsSeen { get; private set; }

    public GameObject Player { get; private set; }

    public List<GameObject> CustomersHitted { get; private set; }

    /// <summary>
    /// The VIEWDISTNACE constant is used to determine the maximum number of colliders that can be detected by raycasting
    /// </summary>
    private const int VIEWDISTANCE = 10;

    private void Awake()
    {
      eventDispatcher = EventDispatcher.GetInstance();
      eventDispatcher.AddListener("CustomerAttacked", this);

      Player = GameObject.FindGameObjectWithTag("Player");

      CustomersHitted = new List<GameObject>();

      TargetsSeen = false;

      StartCoroutine(FovRoutine());
    }

    private void OnDestroy()
    {
        eventDispatcher.RemoveListener("CustomerAttacked", this);
    }

    private IEnumerator FovRoutine()
    {
        float delay = 0.2f;

        var wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            FOVCheck();
        }
    }

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
                    CustomersHitted.Add(target.gameObject);
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

    private void CheckIfTargetsAreInFov(List<Vector3> directionsToTargets)
    {
        RaycastHit[] hits = new RaycastHit[VIEWDISTANCE]; 

        foreach (Vector3 direction in directionsToTargets)
        {
            if (Vector3.Angle(transform.forward, direction) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, direction);

                // Using RaycastNonAlloc to avoid memory allocation
                int hitCount = Physics.RaycastNonAlloc(transform.position, direction, hits, distanceToTarget, obstructionMask);

                if (hitCount > 0)
                {
                    ResetTargetsAtributes();
                    return;
                }
            }
        }

        TargetsSeen = true;
    }

    private void ResetTargetsAtributes()
    {
        TargetsSeen = false;   
        CustomersHitted.Clear();
    }

    public void OnEvent(string eventType, GameObject target)
    {
        if (eventType == "CustomerAttacked" && CustomersHitted.Count > 0 && CustomersHitted.Contains(target))
        {   
            GameObject custumerHitted = CustomersHitted.Find(c => c == target);

            eventDispatcher.RemoveEventDispatched("CustomerAttacked", custumerHitted);

            strikes++;

            const int MAXSTRIKES = 3;

            if (strikes >= MAXSTRIKES)
            {
                Debug.Log("You're fired!");

                return;
            }

            Debug.Log("Strike Nº "+ strikes);
        }
    }
}
