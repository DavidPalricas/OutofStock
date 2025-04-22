using UnityEngine;

public class Shopping : CustomerBaseState
{
    [SerializeField]
    private float minTimeToPickProduct, maxTimeToPickProduct;

    [Range(0f, 1f)]
    [SerializeField]
    private float probBecamingThief;

    protected override void OnEnable()
    {   
        base.OnEnable();
        stateName = GetType().Name;
        Debug.Log(customerMovement);
    }

    public override void Enter()
    {
        base.Enter();
        customerMovement.SetAgentDestination(customerMovement.AreasPos["Product"]);


        if (custumerType == CustumersTypes.Normal && BecamesThief())
        {
            fSM.ChangeState("BecameThief");
            return;
        }
    }

    public override void Execute()
    {
        base.Execute();

        if (customerMovement.GoalReached)
        {
            PickProduct();
        }
    }
    public override void Exit()
    {
        base.Exit();
    }


    private bool BecamesThief()
    {
        float randomValue = Random.Range(0f, 1f);

        return randomValue < probBecamingThief;
    }

    private void PickProduct()
    {
        StartCoroutine(Utils.WaitAndExecute(Utils.RandomFloat(minTimeToPickProduct, maxTimeToPickProduct), () => fSM.ChangeState("ProductPicked")));
    }
}
