using UnityEngine;

public class GrabSubTask : SubTask
{

    [SerializeField]
    private Item item;


    private void OnEnable()
    {
        subTaskGameObject = gameObject;

        type = SubTaskType.Grab;
    }

    private void Update()
    {
        if (item.Grabbed)
        {
           SubTaskCompleted();
            enabled = false;
        }
    }

}
