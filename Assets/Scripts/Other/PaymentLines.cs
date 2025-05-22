using UnityEngine;
using System.Collections;
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

    // Controle de ocupação das posições de pagamento
    private GameObject customerAtPayment1 = null;
    private GameObject customerAtPayment2 = null;

    private void Awake()
    {   
       paymentArea1Pos = paymentArea1Transform.position;
    }

    // Move os clientes para frente na fila
    private void MoveCustomers(GameObject customer)
    {
        Vector3 customerPos = customer.transform.position;

        if (customer.TryGetComponent<CustomerMovement>(out var movement))
        {
            movement.SetDestination(new Vector3(
                customerPos.x,
                customerPos.y,
                customerPos.z + DISTANCE_BETWEEN_CUSTOMERS
            ));
        }
    }

    // Verifica se a posição de pagamento está ocupada
    private bool IsPaymentPositionOccupied(Vector3 paymentArea)
    {
        if (paymentArea == paymentArea1Pos)
        {
            return customerAtPayment1 != null;
        }
       
        return customerAtPayment2 != null;
    }

    // Reserva a posição de pagamento para um cliente
    private void ReservePaymentPosition(GameObject customer, Vector3 paymentArea)
    {
        if (paymentArea == paymentArea1Pos)
            customerAtPayment1 = customer;
        else
            customerAtPayment2 = customer;
    }

    // Libera a posição de pagamento
    private void ReleasePaymentPosition(Vector3 paymentArea)
    {
        if (paymentArea == paymentArea1Pos)
            customerAtPayment1 = null;
        else
            customerAtPayment2 = null;
    }

    // Adiciona um cliente à fila especificada
    public void AddCustomerToLine(GameObject customer, Vector3 paymentArea)
    {
        List<GameObject> paymentLine = paymentArea == paymentArea1Pos ? paymentLine1 : paymentLine2;

        // Se o cliente já está na fila, não adicione novamente
        if (paymentLine.Contains(customer))
        {
            return;
        }

        // Adiciona o cliente à lista
        paymentLine.Add(customer);

        // Se for o primeiro cliente e a posição de pagamento estiver livre
        if (paymentLine.Count == 1 && !IsPaymentPositionOccupied(paymentArea))
        {
            ReservePaymentPosition(customer, paymentArea);
            CustomerMovement customerMovement = customer.GetComponent<CustomerMovement>();
            if (customerMovement != null)
            {
                customerMovement.SetDestination(paymentArea);
            }
            Debug.Log($"Cliente {customer.name} adicionado como primeiro da fila para pagamento.");
        }
        else
        {
            // Posiciona o cliente atrás do último da fila
            GameObject lastCustomerInLine = paymentLine.Count > 1 ? paymentLine[paymentLine.Count - 2] : null;
            
            if (customer.TryGetComponent<CustomerMovement>(out var customerMovement))
            {
                Vector3 alignPos;

                if (lastCustomerInLine != null)
                {
                    // Posicionar atrás do último cliente
                    Vector3 lastPos = lastCustomerInLine.transform.position;
                    alignPos = new Vector3(
                        lastPos.x,
                        customer.transform.position.y,
                        lastPos.z - DISTANCE_BETWEEN_CUSTOMERS
                    );
                }
                else
                {
                    // Se for o primeiro da fila mas a posição está ocupada,
                    // posicionar atrás da área de pagamento
                    alignPos = new Vector3(
                        paymentArea.x,
                        customer.transform.position.y,
                        paymentArea.z - DISTANCE_BETWEEN_CUSTOMERS
                    );
                }

                customerMovement.SetDestination(alignPos);
            }

            Debug.Log($"Cliente {customer.name} adicionado na posição {paymentLine.Count} da fila.");
        }
    }

    // Verifica se o cliente está na frente da fila e pronto para pagar
    public bool ReadyToPay(GameObject customer, Vector3 paymentArea)
    {
        List<GameObject> paymentLine = paymentArea == paymentArea1Pos ? paymentLine1 : paymentLine2;

        // O cliente está na frente da fila e na posição de pagamento?
        if (paymentLine.Count > 0 && paymentLine[0] == customer)
        {
            // Verificar se o cliente está na posição de pagamento
            GameObject currentPayingCustomer = paymentArea == paymentArea1Pos ? customerAtPayment1 : customerAtPayment2;
            return currentPayingCustomer == customer;
        }

        return false;
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

            // Libera a posição de pagamento
            ReleasePaymentPosition(paymentArea);

            Debug.Log($"Cliente {customer.name : null} removido após pagamento. Restantes na fila: {paymentLine.Count}");

            // Se houver mais clientes na fila, o próximo avança para a posição de pagamento
            if (paymentLine.Count > 0)
            {
                GameObject nextCustomer = paymentLine[0];
                ReservePaymentPosition(nextCustomer, paymentArea);

                CustomerMovement movement = nextCustomer.GetComponent<CustomerMovement>();
                if (movement != null)
                {
                    // Pequeno atraso antes de mover o próximo cliente para a posição de pagamento
                    StartCoroutine(Utils.WaitAndExecute(MOVE_DELAY, () => {
                        movement.SetDestination(paymentArea);
                    }));
                }

                // Move os outros clientes da fila para frente
                for (int i = 1; i < paymentLine.Count; i++)
                {
                    GameObject currentCustomer = paymentLine[i];
                    GameObject customerAhead = paymentLine[i - 1];

                    CustomerMovement currentMovement = currentCustomer.GetComponent<CustomerMovement>();
                    if (currentMovement != null && customerAhead != null)
                    {
                        StartCoroutine(Utils.WaitAndExecute(MOVE_DELAY * (i + 1), () => {
                            Vector3 aheadPos = customerAhead.transform.position;
                            Vector3 newPos = new Vector3(
                                aheadPos.x,
                                currentCustomer.transform.position.y,
                                aheadPos.z - DISTANCE_BETWEEN_CUSTOMERS
                            );
                            currentMovement.SetDestination(newPos);
                        }));
                    }
                }
            }
        }
    }

    // Remove um cliente específico da fila (caso ele desista, por exemplo)
    public void RemoveCustomer(GameObject customer, Vector3 paymentArea)
    {
        List<GameObject> paymentLine = paymentArea == paymentArea1Pos ? paymentLine1 : paymentLine2;
        int index = paymentLine.IndexOf(customer);

        if (index >= 0)
        {
            // Se o cliente estava na posição de pagamento, libere-a
            if (index == 0)
            {
                ReleasePaymentPosition(paymentArea);
            }

            paymentLine.RemoveAt(index);
            Debug.Log($"Cliente {customer.name} removido da fila. Restantes: {paymentLine.Count}");

            // Se o cliente removido era o primeiro e ainda há clientes na fila
            if (index == 0 && paymentLine.Count > 0)
            {
                // O próximo cliente avança para a posição de pagamento
                GameObject nextCustomer = paymentLine[0];
                ReservePaymentPosition(nextCustomer, paymentArea);

                CustomerMovement movement = nextCustomer.GetComponent<CustomerMovement>();
                if (movement != null)
                {
                    movement.SetDestination(paymentArea);
                }
            }

            // Reorganizar a fila após a remoção
            for (int i = index; i < paymentLine.Count; i++)
            {
                GameObject currentCustomer = paymentLine[i];
                GameObject customerAhead = i > 0 ? paymentLine[i - 1] : null;

                CustomerMovement currentMovement = currentCustomer.GetComponent<CustomerMovement>();
                if (currentMovement != null)
                {
                    Vector3 newPos;

                    if (i == 0)
                    {
                        // Primeiro cliente vai para a área de pagamento
                        newPos = paymentArea;
                    }
                    else if (customerAhead != null)
                    {
                        // Os outros se alinham atrás do cliente à frente
                        Vector3 aheadPos = customerAhead.transform.position;
                        newPos = new Vector3(
                            aheadPos.x,
                            currentCustomer.transform.position.y,
                            aheadPos.z - DISTANCE_BETWEEN_CUSTOMERS
                        );
                    }
                    else
                    {
                        // Fallback
                        newPos = new Vector3(
                            paymentArea.x,
                            currentCustomer.transform.position.y,
                            paymentArea.z - DISTANCE_BETWEEN_CUSTOMERS * (i + 1)
                        );
                    }

                    StartCoroutine(Utils.WaitAndExecute(MOVE_DELAY * (i + 1), () => {
                        currentMovement.SetDestination(newPos);
                    }));
                }
            }
        }
    }
}
