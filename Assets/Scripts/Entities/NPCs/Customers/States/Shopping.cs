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


        if (NormalCustomer() && BecamesThief())
        {
            fSM.ChangeState("BecameThief");
            return;
        }
    }

    public override void Execute()
    {
        base.Execute();

        if (customerMovement.WasAttacked)
        {
            fSM.ChangeState("Attacked");
            return;
        }

        if (customerMovement.DestinationReached)
        {   
            if (customerMovement is KarenMovement)
            {
               fSM.ChangeState("ProductFound");

                return;
            }

            PickProduct();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }


    private bool BecamesThief()
    {
        float randomValue = Utils.RandomFloat(0f, 1f);

        float probBecamingThief = PlayerPrefs.GetFloat("CustomerBecameThiefProb");

        return randomValue < probBecamingThief;
    }

    private void PickProduct()
    {   
        if (customerMovement is AnnoyingKidMovement annoyingKid)
        {
            annoyingKid.HoldsProduct = true;
        }

        StartCoroutine(Utils.WaitAndExecute(Utils.RandomFloat(minTimeToPickProduct, maxTimeToPickProduct), () => fSM.ChangeState("ProductPicked")));
    }

    private bool NormalCustomer()
    {
       return name.Contains("NormalCustomer");
    }
}
