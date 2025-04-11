using UnityEngine;
using UnityEngine.InputSystem;

public class SubTask : MonoBehaviour, ISubject
{   
    protected enum SubTaskType
    {
        Interact,
        Grab,
    }

    [SerializeField]
    private GameObject subTaskIconPrefab;

    private IObserver[] observers;

    protected GameObject subTaskGameObject = null;

    protected SubTaskType type;

    private GameObject subTaskIcon = null;


    public void MoveIconToSubTask()
    {
        if (subTaskGameObject == null)
        {
            subTaskGameObject = gameObject;
        }

        Vector3 subTaskPos = subTaskGameObject.transform.position;

        const float ICON_DISTANCE = 1.5f;

        Vector3 subTaskIconPos = subTaskPos + transform.forward * ICON_DISTANCE;

        subTaskIcon = Instantiate(subTaskIconPrefab, subTaskIconPos, Quaternion.identity);
    }


    protected void SubTaskCompleted()
    {
        NotifyObservers();
        RemoveObservers();

        Destroy(subTaskIcon);

        enabled = false;
    }

    public void AddObservers(IObserver[] observers)
    {
        this.observers = observers;
    }


    public void NotifyObservers(object data = null)
    {
        foreach (IObserver observer in observers)
        {
            observer.UpdateObserver(data);
        }
    }

    public void RemoveObservers()
    {
        observers = null;
    }
}
