using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// The TaskManager class is responsible for generating tasks, show their names and status in the UI.
/// It implements the IObserver interface to be notified when a task is completed.
/// </summary>
public class TaskManager : MonoBehaviour, IObserver
{
    /// <summary>
    /// The taskTogglePrefab property stores a reference to the prefab of the task toggle.
    /// </summary>
    [SerializeField]
    private Toggle taskTogglePrefab;

    /// <summary>
    /// The taskContainerTransform property stores a reference to the transform of the task container.
    /// This container will show the active tasks in the UI.
    /// </summary>
    [SerializeField]
    private Transform taskContainerTransform;

    /// <summary>
    /// The fixFuseBox and cleanFloor properties are responsible for storing references 
    /// to the game objects that represent the tasks in the game.
    /// </summary>
    [SerializeField]
    private GameObject fixFuseBox, cleanFloor;

    /// <summary>
    /// The activeTaskToggles property is responsible for storing the active tasks toggles in the game.
    /// </summary>
    private readonly Dictionary<int, Toggle> activeTaskToggles = new ();

    /// <summary>
    /// The tasksTypes property is responsible for storing the types of tasks in the game.
    /// </summary>
    private readonly Dictionary<int, string> tasksTypes = new()
    {   { 0, "Fix Fuse Box" },
        { 1, "Clean Floor" }
    };

    /// <summary>
    /// The allTypeOfTasksActivated property is a flag which indicates if all types of tasks are activated.
    /// </summary>
    private bool allTypeOfTasksActivated = false;

    /// <summary>
    /// The addTaskTime and timer properties are responsible for storing the time to add a new task and the timer to
    /// count the time to add a new task, respectively.
    /// </summary>
    private float addTaskTime, timer;

    /// <summary>
    /// The maxTasks and tasksCompleted properties are responsible for storing the maximum number of tasks
    /// and the number of tasks completed, respectively.
    /// </summary>
    private int maxTasks, tasksCompleted = 0;

    /// <summary>
    /// The Awake method is called when the script instance is being loaded (Unity Callback).
    /// In this method, the timer is initialized the number of max tasks, the time to add a new task and the task timer.
    /// </summary>
    private void Awake()
    {   maxTasks = PlayerPrefs.GetInt("NumberOfTasks");

        addTaskTime = maxTasks / PlayerPrefs.GetFloat("ShiftDurationIRL") * 0.1f;

        timer = Time.time + addTaskTime;
    }

    /// <summary>
    /// The Update method is called every frame (Unity Callback).
    /// In this method, we are checking if a task can be generated.
    /// To do that, we are checking if the tasks completed are less than the maximum number of tasks, if all types of tasks are not activated,
    /// and if the its time to add a new task (timer).
    /// </summary>
    private void Update()
    {
        if (tasksCompleted < maxTasks && !allTypeOfTasksActivated && Time.time >= timer)
        {
            GenerateTask();

            // Reset the timer for the next task generation
            timer = Time.time + addTaskTime;
        }
    }

 
    /// <summary>
    /// The GenerateTask method is responsible for generating a new task.
    /// </summary>
    /// <remarks>
    ///  This method performs the following steps:
    ///     1. It creates a list of available tasks by checking if the activeTaskToggles does not contain a task number.
    ///     2. If this list is empty, the allTypeOfTasksActivated is set to true and the method stops right there otherwise it continues.
    ///     3. It selects a random task number from the available tasks list and creates a new toggle 
    ///     4. This new toggle will have the as its text the name of the task which is obtained with its number from the tasksTypes dictionary.
    ///     5. After that the ActivateTask method is called to activate the task and adds the new toggle to the game.
    ///     6. Finally, it checks if all types of tasks are activated and sets the allTypeOfTasksActivated flag accordingly. 
    /// </remarks>
    private void GenerateTask()
    {
        var availableTasks = new List<int>();

        foreach (int taskId in tasksTypes.Keys)
        {
            if (!activeTaskToggles.ContainsKey(taskId))
            {
                availableTasks.Add(taskId);
            }
        }

        if (availableTasks.Count == 0)
        {
            allTypeOfTasksActivated = true;
            return;
        }

        int taskNumber = GetRandomTaskNumber(availableTasks);

        Toggle newToggle = Instantiate(taskTogglePrefab, taskContainerTransform);

        Text toggleText = newToggle.GetComponentInChildren<Text>();

        toggleText.text = tasksTypes[taskNumber];
        
        ActivateTask(taskNumber);
    
        activeTaskToggles.Add(taskNumber, newToggle);

        allTypeOfTasksActivated = activeTaskToggles.Count == tasksTypes.Count;
    }


    /// <summary>
    /// ´tHE GetRandomTaskNumber method is responsible for generating a random task number based on the available tasks.
    /// </summary>
    /// <remarks>
    /// It reads the probabilities of each task from PlayerPrefs and generates a random number.
    /// Then iterates through the task probabilities and checks if the random number is less than or equal to the task probability, 
    /// and checks if the task is available.
    /// If it is, it returns the task number, otherwise it returns the first available task.
    /// </remarks>
    /// <param name="availableTasks">The available tasks.</param>
    /// <returns>The number of an avaible task</returns>
    private int GetRandomTaskNumber(List<int> availableTasks)
    {
        float fixFuseBoxProb = PlayerPrefs.GetFloat("FixFuseBoxProb");
        float cleanFloorProb = PlayerPrefs.GetFloat("CleanFloorProb");
        float randomValue = Utils.RandomFloat(0f, 1f);

        var taskProbabilities = new Dictionary<int, float>
       {
           { 0, fixFuseBoxProb },
           { 1, cleanFloorProb }
       };

        foreach (var task in taskProbabilities.OrderBy(t => t.Value))
        {
            if (availableTasks.Contains(task.Key) && task.Value > 0 && randomValue <= task.Value)
            {
                return task.Key;
            }
        }

        return availableTasks.First();
    }

    /// <summary>
    /// The ActivateTask method is responsible for activating a task base on its number.
    /// </summary>
    /// <remarks>
    ///  For now the active task can be 2 types base on the task number:
    ///     0 - Fix Fuse Box
    ///     1 - Clean Floor
    ///  After activating the task, its properties are initialized.
    /// </remarks>
    /// <param name="taskNumber">The task number.</param>
    private void ActivateTask(int taskNumber)
    {   
        Transform taskObjectTransform;

        switch (taskNumber)
        {
            case 0:
                taskObjectTransform = fixFuseBox.transform;
                taskObjectTransform.GetComponent<FixFuseBox>().enabled = true;

                break;

            case 1:
                taskObjectTransform = cleanFloor.transform;
                taskObjectTransform.GetComponent<CleanFloor>().enabled = true;
                break;

            default:
                return;
        }

        Task task =  taskObjectTransform.gameObject.GetComponent<Task>();
        task.AddObservers(new IObserver[] { this });
        task.Number = taskNumber;
    }

    /// <summary>
    /// The OnNotify (IObserver method) method is responsible for updating the observer (this game object), when a subject notifies it.
    /// In this method, a start coroutine is called to handle the task completion (TaskDone method).
    /// </summary>
    /// <param name="data">The number of the task completed</param>
    public void OnNotify(object data)
    {
        StartCoroutine(TaskDone((int) data));
    }

    /// <summary>
    /// The TaskDone method is responsible for handling the task completion.
    /// </summary>
    /// <remarks>
    /// This method turns the toggle on for 3 seconds, then turns it off and destroys it.
    /// After destroying the allTypeOfTasksActivated flag and the timer
    /// are reseted.
    /// </remarks>
    /// <param name="taskNumber">The task number.</param>
    /// <returns>An IEnumerator that can be used in a coroutine to wait and execute the specified method.</returns>
    private IEnumerator TaskDone(int taskNumber)
    {
        if (activeTaskToggles.TryGetValue(taskNumber, out Toggle toggle))
        {
            toggle.isOn = true;

            yield return new WaitForSeconds(3f);

            activeTaskToggles.Remove(taskNumber);
            allTypeOfTasksActivated = false;
            toggle.isOn = false;
            Destroy(toggle.gameObject);
        }

        allTypeOfTasksActivated = false;

        // Notify the event manager that a task is completed
        EventManager.GetInstance().OnTaskCompleted();
        tasksCompleted++;
        timer = Time.time + addTaskTime;
    }
}