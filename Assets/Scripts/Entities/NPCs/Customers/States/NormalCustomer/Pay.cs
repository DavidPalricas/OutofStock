using UnityEngine;
/// <summary>
///  The Pay class is responsible for handling the payment state of the customer.
///  This state is exclusively for the Normal Customer Stereotype.
/// </summary>
public class Pay : CustomerBaseState
{
    /// <summary>
    /// The minimumTimeToPay and maximumTimeToPay attributes are the time that the customer will take to pay for the product.
    /// </summary>
    [SerializeField]
    private float minTimeToPay, maxTimeToPay;
    /// <summary>
    /// The timer attribute is used to store the time when the customer started paying for the product.
    /// </summary>
    private float timer;
    private PaymentLines paymentLines;
    private bool isPaying = false;
    private bool isInPaymentLine = false;
    private Vector3 paymentAreaPos;

    /// <summary>
    /// The Awake Method is called when the script instance is being loaded (Unity Callback).
    /// It calls the base class Awake method and sets the stateName to the name of the current class.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        StateName = GetType().Name;
        paymentLines = GameObject.FindGameObjectWithTag("PaymentLines").GetComponent<PaymentLines>();
    }

    private void Start()
    {
        paymentAreaPos = customerMovement.AreasPos["Payment"];
    }

    /// <summary>
    /// The Enter method is called when the state is entered.
    /// It calls the base class Enter method and sets the customer destination to the payment area.
    /// </summary>
    public override void Enter()
    {
        base.Enter();
        // Não definimos destino logo aqui, primeiro verificamos a fila
        isPaying = false;
        isInPaymentLine = false;
        timer = 0f;
    }

    /// <summary>
    /// The Execute method is called when the state is executed, to perform the actions of the state.
    /// It calls the base class Execute method.
    /// <remarks>
    /// This methods checks for possible conditions to change the state, otherwise it continues the states actions.
    /// The possible transitions are:
    ///    1. Attacked Transition: If the customer is attacked, it changes to the Knocked state.
    ///    2. ProductPaid Transition: When the customer reaches its destination, it starts a timer to simulate the time it takes to pay for the product, after the timer ends, it changes to the Go Home state.
    ///    
    /// If the none of these conditions are met, nothing happens until the customer reaches its destination or is attacked.
    /// </remarks>
    /// </summary>
    public override void Execute()
    {
        base.Execute();

        // Verifica se foi atacado
        if (customerMovement.WasAttacked)
        {
            fSM.ChangeState("Attacked");
            return;
        }

        // Se ainda não entrou na fila
        if (!isInPaymentLine)
        {
            // Verifica se a fila não está vazia, se não estiver, muda para o estado de espera
            if (!paymentLines.IsLineEmpty(paymentAreaPos))
            {
                fSM.ChangeState("WaitToPay");
                return;
            }

            // Se a fila estiver vazia, adiciona-se à fila e marca como na fila
            paymentLines.AddCustomerToLine(gameObject, paymentAreaPos);
            isInPaymentLine = true;
            // Não precisamos definir destino, AddCustomerToLine já faz isso
        }

        // Verifica se é o cliente da frente e pode pagar
        if (!isPaying && paymentLines.ReadyToPay(gameObject, paymentAreaPos) && customerMovement.DestinationReached)
        {
            // Inicia o pagamento
            isPaying = true;
            timer = Time.time + Utils.RandomFloat(minTimeToPay, maxTimeToPay);
            Debug.Log($"Cliente {gameObject.name} iniciando pagamento. Tempo estimado: {timer - Time.time:F1}s");
        }

        // Se está pagando e o tempo de pagamento acabou
        if (isPaying && Time.time >= timer)
        {
            paymentLines.CustomerPaid(paymentAreaPos);
            fSM.ChangeState("ProductPaid");
        }
    }

    /// <summary>
    /// The Exit method is called when the state is exited, to handle its final actions.
    /// It calls the base class Exit method.
    /// </summary>
    public override void Exit()
    {
        base.Exit();
        isPaying = false;

        // Se o cliente saiu do estado sem pagar (foi para WaitToPay por exemplo)
        // não precisamos remover da fila, pois ele continuará nela
    }
}