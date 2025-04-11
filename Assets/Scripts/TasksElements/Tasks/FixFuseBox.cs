using UnityEngine;

public class FixFuseBox : Task
{
    [SerializeField]
    private GameObject fuseBox;


    private void OnEnable()
    {
        subTasks = new GameObject[] { fuseBox };

        InteractSubTask subTask = subTasks[0].GetComponent<InteractSubTask>();

        subTask.enabled = true;
        subTask.AddObservers(new IObserver[] { this });
        subTask.MoveIconToSubTask();
        subTask.CanBeReseted = true;
    }

    private void Update()
    {
        if (completedSubTasks >= subTasks.Length)
        {
            TaskCompleted();
            enabled = false;
        }
    }
}
