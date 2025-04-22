using UnityEngine;

public class Shopping : CustomerBaseState
{
    [SerializeField]
    private float minTimeToPickProduct, maxTimeToPickProduct;

    protected override void Awake()
    {   
        base.Awake();
        stateName = GetType().Name;
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

        if (customerMovement.DestinationReached)
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

        float probBecamingThief = PlayerPrefs.GetFloat("CustomerBecameThiefProb");

        return randomValue < probBecamingThief;
    }

    private void PickProduct()
    {
        StartCoroutine(Utils.WaitAndExecute(Utils.RandomFloat(minTimeToPickProduct, maxTimeToPickProduct), () => fSM.ChangeState("ProductPicked")));
    }
}
