using UnityEngine;

public class Running : CustomerBaseState
{
    [SerializeField]
    private float timeToDecideToGrabAProduct;

    [SerializeField]
    [Range(0, 1)]
    private float probToGrabAProduct;

    private float timer;

    private bool holdsProduct = false;

    private AnnoyingKidMovement annoyingKidMovement;

    protected override void Awake()
    {
        base.Awake();
        stateName = GetType().Name;
    }

    public override void Enter()
    {
        base.Enter();

        timer = Time.time + timeToDecideToGrabAProduct;

        if (customerMovement is AnnoyingKidMovement)
        {
            annoyingKidMovement = customerMovement as AnnoyingKidMovement;
            annoyingKidMovement.Run();

            holdsProduct = annoyingKidMovement.HoldsProduct;
        }
    }


    public override void Execute()
    {
        base.Execute();

        if (annoyingKidMovement.WasAttacked)
        {
            fSM.ChangeState("Attacked");
            return;
        }
 
       if (!holdsProduct && Time.time >= timer)
       {
         if (DecidedToGrabAProduct())
         {
            fSM.ChangeState("PickAProduct");
            return;
         }

         timer = Time.time + timeToDecideToGrabAProduct;
       }
        
        if (annoyingKidMovement.DestinationReached)
        {
            annoyingKidMovement.Run();
        }
    }


    public override void Exit()
    {
        base.Exit();
    }

    private bool DecidedToGrabAProduct()
    {
        return Utils.RandomFloat(0, 1) <= probToGrabAProduct;
    }
}

