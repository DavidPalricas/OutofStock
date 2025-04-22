using UnityEngine;

public class Steal : CustomerBaseState
{
    [SerializeField]
    private float minTimeToSteal, maxTimeToSteal;

    [SerializeField]
    private Material thiefMaterial;

    protected override void Awake()
    {
        base.Awake();
        stateName = GetType().Name;
    }

    public override void Enter()
    {
        base.Enter();

        customerMovement.SetAgentDestination(customerMovement.AreasPos["Product"]);
    }
    public override void Execute()
    {
        base.Execute();

        if (customerMovement.DestinationReached)
        {   
            StealProduct();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void StealProduct()
    {
        DressThiefClothes();
        StartCoroutine(Utils.WaitAndExecute(Utils.RandomFloat(minTimeToSteal, maxTimeToSteal), () => fSM.ChangeState("ProductStealed")));
    }

    private void DressThiefClothes()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = thiefMaterial;
    }
}
