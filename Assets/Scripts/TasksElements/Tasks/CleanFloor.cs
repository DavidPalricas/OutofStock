using System.Linq;
using UnityEngine;

public class CleanFloor : Task
{
    [SerializeField]
    private GameObject grabBroomSubTask;

    [SerializeField]
    private Transform cleanSitesTransform;


    private void OnEnable()
    {

        AddSubTasks();

        GrabSubTask grabSubTask = grabBroomSubTask.GetComponent<GrabSubTask>();
        grabSubTask.enabled = true;
        grabSubTask.AddObservers(new IObserver[] { this });
        grabSubTask.MoveIconToSubTask();

    }

    private void AddSubTasks()
    {    
        GameObject[] cleanSites = Utils.GetChildren(cleanSitesTransform);

        subTasks = new GameObject[cleanSites.Length + 1];

        subTasks[0] = grabBroomSubTask;

        for (int i = 0; i < cleanSites.Length; i++)
        {
            subTasks[i + 1] = cleanSites[i];
        }
    }


    private void Update()
    {
        // The player Grabbed the broom
        if (completedSubTasks == 1)
        {
            ActivateCleanSitesSubTasks();
        }
        else if (completedSubTasks == subTasks.Length)
        {
            TaskCompleted();
            enabled = false;
        }
    }


    private void ActivateCleanSitesSubTasks()
    {
        foreach (GameObject cleanSite in subTasks.Skip(1))
        {   
            cleanSite.SetActive(true);

            InteractSubTask subTask = cleanSite.GetComponent<InteractSubTask>();
            subTask.enabled = true;
            subTask.AddObservers(new IObserver[] { this });
            subTask.MoveIconToSubTask();
            subTask.CanBeVanish = true;
        }
    }
}
