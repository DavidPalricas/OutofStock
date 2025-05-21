using UnityEngine;
using System.Collections.Generic;

public class PaymentLines : MonoBehaviour
{
    [SerializeField]
    private Transform paymentArea1Transform; // Corrigido o erro de digitação

    private Vector3 paymentArea1Pos;

    private readonly List<GameObject> paymentLine1 = new();
    private readonly List<GameObject> paymentLine2 = new();

    private const float DISTANCE_BETWEEN_CUSTOMERS = 1.5f; // Aumentado para evitar sobreposição
    private const float MOVE_DELAY = 0.3f; // Tempo para iniciar movimento após remoção do cliente

    private void Awake()
    {
        // Inicializa a posição da área de pagamento
        if (paymentArea1Transform != null)
            paymentArea1Pos = paymentArea1Transform.position;
        else
            Debug.LogError("Payment Area 1 Transform não foi atribuído no Inspector!");
    }

    // Move os clientes para frente na fila
    private void MoveCustomers(GameObject customer)
    {
        Vector3 customerPos = customer.transform.position;
        CustomerMovement movement = customer.GetComponent<CustomerMovement>();
        if (movement != null)
        {
            movement.SetDestination(new Vector3(
                customerPos.x,
                customerPos.y,
                customerPos.z + DISTANCE_BETWEEN_CUSTOMERS
            ));
        }
    }

    // Adiciona um cliente à fila especificada
    public void AddCustomerToLine(GameObject customer, Vector3 paymentArea)
    {
        List<GameObject> paymentLine = paymentArea == paymentArea1Pos ? paymentLine1 : paymentLine2;

        if (paymentLine.Count == 0)
        {
            paymentLine.Add(customer);
            Debug.Log($"Cliente {customer.name} adicionado à fila. Total na fila: {paymentLine.Count}");
            return;
        }

        CustomerMovement customerMovement = customer.GetComponent<CustomerMovement>();
        if (customerMovement != null)
        {
            if (customerMovement.DestinationReached)
            {
                paymentLine.Add(customer);
                return;
            }

            Vector3 lastCustomerInLine = paymentLine[paymentLine.Count - 1].transform.position;
            var alignPos = new Vector3(
                lastCustomerInLine.x,
                customer.transform.position.y,
                lastCustomerInLine.z - DISTANCE_BETWEEN_CUSTOMERS
            );
            customerMovement.SetDestination(alignPos);
        }

        paymentLine.Add(customer);
        Debug.Log($"Cliente {customer.name} adicionado à fila. Total na fila: {paymentLine.Count}");
    }

    // Verifica se o cliente está na frente da fila e pronto para pagar
    public bool ReadyToPay(GameObject customer, Vector3 paymentArea)
    {
        List<GameObject> paymentLine = paymentArea == paymentArea1Pos ? paymentLine1 : paymentLine2;
        return paymentLine.Count > 0 && paymentLine[0] == customer;
    }

    // Verifica se uma fila está vazia
    public bool IsLineEmpty(Vector3 paymentArea)
    {
        List<GameObject> paymentLine = paymentArea == paymentArea1Pos ? paymentLine1 : paymentLine2;
        return paymentLine.Count == 0;
    }

    // Remove o primeiro cliente da fila após o pagamento e move os outros para frente
    public void CustomerPaid(Vector3 paymentArea)
    {
        List<GameObject> paymentLine = paymentArea == paymentArea1Pos ? paymentLine1 : paymentLine2;

        if (paymentLine.Count > 0)
        {
            GameObject customer = paymentLine[0];
            paymentLine.RemoveAt(0);
            Debug.Log($"Cliente {customer?.name} removido após pagamento. Restantes na fila: {paymentLine.Count}");

            // Move os clientes restantes para frente após um pequeno atraso
            foreach (GameObject remainingCustomer in paymentLine)
            {
                StartCoroutine(Utils.WaitAndExecute(MOVE_DELAY, () => MoveCustomers(remainingCustomer)));
            }
        }
    }

    // Remove um cliente específico da fila (caso ele desista, por exemplo)
    public void RemoveCustomer(GameObject customer, Vector3 paymentArea)
    {
        List<GameObject> paymentLine = paymentArea == paymentArea1Pos ? paymentLine1 : paymentLine2;

        if (paymentLine.Contains(customer))
        {
            paymentLine.Remove(customer);
            Debug.Log($"Cliente {customer.name} removido da fila. Restantes: {paymentLine.Count}");
        }
    }
}