using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour, IObserver
{
    private const int MAX_GAME_TIME = 10;

    [Range(1, MAX_GAME_TIME)]
    [SerializeField]
    private float addTaskTime;

    [SerializeField]
    private Toggle taskTogglePrefab; 

    [SerializeField]
    private Transform taskContainer;

    [SerializeField]
    private GameObject fixFuseBox, cleanFloor;

    private float timer;

    private readonly Dictionary<int, Toggle> activeTaskToggles = new ();

    private readonly Dictionary<int, string> tasksTypes = new()
    {   { 0, "Fix Fuse Box" },
        { 1, "Clean Floor" }
    };

    private bool allTypeOfTasksActivated = false;

    private void Awake()
    {
        timer = Time.time + addTaskTime;
    }

    private void Update()
    {
        if (!allTypeOfTasksActivated && Time.time >= timer)
        {
            TryGenerateTask();
            timer = Time.time + addTaskTime;
        }
    }

    private void TryGenerateTask()
    {
        float randomValue = Random.value;
        const float ADDTASKPROB = 0.4f;

        if (randomValue <= ADDTASKPROB)
        {
            GenerateTask();
        }
    }

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

        int taskNumber = availableTasks[Utils.RandomInt(0 , availableTasks.Count)];

        availableTasks.Remove(taskNumber);

        Toggle newToggle = Instantiate(taskTogglePrefab, taskContainer);

        Text toggleText = newToggle.GetComponentInChildren<Text>();

        if (toggleText != null)
        {
            toggleText.text = tasksTypes[taskNumber];
        }

        ActivateTask(taskNumber);
    
        activeTaskToggles.Add(taskNumber, newToggle);

        allTypeOfTasksActivated = activeTaskToggles.Count == tasksTypes.Count;
    }

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

    public void UpdateObserver(object data)
    {
        StartCoroutine(TaskDone((int) data));

        allTypeOfTasksActivated = false;
        timer = Time.time + addTaskTime;
    }


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
    }
}