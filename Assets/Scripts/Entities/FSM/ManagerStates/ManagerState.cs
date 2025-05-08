public class ManagerState : State
{
    protected ManagerMovement movement;

    protected float timetoWait;

    protected virtual void Awake()
    {
        movement = GetComponent<ManagerMovement>();
    }

    protected virtual void StayOnPoint(){}
}
