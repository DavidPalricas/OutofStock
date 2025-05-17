using UnityEngine;

public class Running : CustomerBaseState
{
    [SerializeField]
    [Range(1, 1440)]
    private float timeToDecideToGrabAProduct;

    [SerializeField]
    [Range(0, 1)]
    private float probToGrabAProduct;

    private float timerToDecideToGrabAProduct = 0f;

    private bool holdsProduct = false;


    protected override void Awake()
    {
        base.Awake();
        stateName = GetType().Name;
    }


    public override void Enter()
    {
        base.Enter();


        timerToDecideToGrabAProduct = 0f;

        // Corrected the issue by calling a valid method or property on customerMovement  
        if (customerMovement is AnnoyingKidMovement annoyingKid)
        { 
            annoyingKid.Run();

            holdsProduct = annoyingKid.HoldsProduct;
        }
        else
        {
            Debug.LogError("This state must be used only in the Annoying Kid prefab");
        }
    }


    public override void Execute()
    {
        base.Execute();

        if (!holdsProduct)
        {
            timerToDecideToGrabAProduct += Time.deltaTime;

            if (timerToDecideToGrabAProduct >= timeToDecideToGrabAProduct * 60 && DecidedToGrabAProduct())
            {
                fSM.ChangeState("Pick a product");

                return;
            }
        }

        if (customerMovement.DestinationReached && customerMovement is AnnoyingKidMovement annoyingKid)
        {
            annoyingKid.Run();
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

