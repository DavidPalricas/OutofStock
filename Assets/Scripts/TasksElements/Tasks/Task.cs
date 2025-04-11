using UnityEngine;

public class Task : MonoBehaviour, ISubject, IObserver
{
    protected IObserver[] observers;

    protected int completedSubTasks = 0;

    protected GameObject[] subTasks;


    public int Number { get; set; }

    protected void TaskCompleted()
    {
        NotifyObservers();
        enabled = false;
    }

  

    public void UpdateObserver(object data = null)
    {
        completedSubTasks++;
    }

    public void AddObservers(IObserver[] observers)
    {
        this.observers = observers;
    }

    public void NotifyObservers(object data = null)
    {
        foreach (IObserver observer in observers)
        {
            observer.UpdateObserver(Number);
        }
    }

    public void RemoveObservers()
    {
        observers = null;
    }
}
