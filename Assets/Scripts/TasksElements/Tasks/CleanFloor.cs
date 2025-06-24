using System.Linq;
using UnityEngine;

/// <summary>
/// The CleanFloor class is responsible for representing a task that requires the player to clean the floor.
/// </summary>
public class CleanFloor : Task
{
    /// <summary>
    /// The grabBroomSubTask property is responsible for storing a reference to the subtask that requires the player to grab a broom.
    /// </summary>
    [SerializeField]
    private GameObject grabBroomSubTask;

    /// <summary>
    /// The cleanSitesTransform property is responsible for storing a reference to the transform that contains the clean sites.
    /// </summary>
    [SerializeField]
    private Transform cleanSitesTransform;

    /// <summary>
    /// The broomSubTaskDone property is a flag which indicates whether the broom subtask has been completed or not.
    /// </summary>
    /// <remarks>
    /// This subtask is the first one to be completed in the task, and the others subtasks can be only activade if
    /// this one is completed.
    // </remarks>
    private bool broomSubTaskDone= false;


    private int cleanSites = 0;

    /// <summary>
    /// The OnEnable method is called when the script is enabled. (Unity Callback).
    /// In this method, we add the subtasks to the task , enable the grab broom subtask and initialize its properties.
    /// </summary>
    private void OnEnable()
    {
        AddSubTasks();

        GrabSubtask grabSubTask = grabBroomSubTask.GetComponent<GrabSubtask>();
        grabSubTask.enabled = true;
        grabSubTask.AddObservers(new IObserver[] { this });
        grabSubTask.MoveIconToSubtask();
    }

    /// <summary>
    /// The Update method is called every frame (Unity Callback).
    /// In this method we are checking if the first substask (grab broom) is completed 
    /// or all sunbtasks are completed.
    /// </summary>
    /// <remarks>
    /// If the grab broom subtask is completed, the clean sites subtasks are activated.
    /// If the all subtasks are completed, the TaskCompleted method is called to handle its completion, and
    /// the script is disabled.
    /// </remarks>
    private void Update()
    {
        // The player Grabbed the broom
        if (!broomSubTaskDone && completedSubtasks == 1)
        {
            ActivateCleanSitesSubTasks();
            broomSubTaskDone = true;
        }

        if (completedSubtasks == cleanSites + 1)
        {
            TaskCompleted();
            enabled = false;
            broomSubTaskDone = false;
        }
    }

    /// <summary>
    /// The AddSubTasks method is responsible for adding the grabBroomSubTask and the clean sites subtasks to the task.
    /// </summary>
    private void AddSubTasks()
    {
        GameObject[] cleanSites = Utils.GetChildren(cleanSitesTransform);

        subtasks = new GameObject[cleanSites.Length + 1];

        subtasks[0] = grabBroomSubTask;

        for (int i = 0; i < cleanSites.Length; i++)
        {
            subtasks[i + 1] = cleanSites[i];
        }
    }

    /// <summary>
    /// The ActivateCleanSitesSubTasks method is responsible for activating the clean sites subtasks.
    /// and initializing their properties.
    /// </summary>
    private void ActivateCleanSitesSubTasks()
    {
        // - 1 to ignore the bloom subtask
        cleanSites = Utils.RandomInt(1, subtasks.Length - 1);

        for  (int i = 1; i <= cleanSites; i++)
        {   
            GameObject cleanSite = subtasks[i];

            cleanSite.SetActive(true);

            InteractSubtask subTask = cleanSite.GetComponent<InteractSubtask>();
            subTask.enabled = true;
            subTask.AddObservers(new IObserver[] { this });
            subTask.MoveIconToSubtask();
            subTask.CanBeVanish = true;
        }

    }
}