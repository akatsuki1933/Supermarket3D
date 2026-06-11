using UnityEngine;
using System.Collections.Generic;

public class CheckoutManager : MonoBehaviour
{
    public static CheckoutManager Instance;

    public Transform[] queuePoints;

    private List<Customer> queue = new List<Customer>();
    private bool checkoutBusy = false;

    void Awake()
    {
        Instance = this;
    }

    public int JoinQueue(Customer customer)
    {
        queue.Add(customer);

        return queue.Count - 1;
    }

    public void LeaveQueue(Customer customer)
    {
        queue.Remove(customer);

        // Reordenar clientes
        for (int i = 0; i < queue.Count; i++)
        {
            queue[i].MoveToQueuePosition(i);
        }
    }

    public Transform GetQueuePoint(int index)
    {
        if (index >= queuePoints.Length)
        {
            return queuePoints[queuePoints.Length - 1];
        }

        return queuePoints[index];
    }

    public bool IsFirst(Customer customer)
    {
        return queue.Count > 0 && queue[0] == customer;
    }

    public bool CanPay(Customer customer)
    {
        Debug.Log("Queue count: " + queue.Count + " | IsFirst: " + (queue.Count > 0 && queue[0] == customer) + " | Busy: " + checkoutBusy);
        return
            queue.Count > 0 &&
            queue[0] == customer &&
            !checkoutBusy;
    }

    public void SetBusy(bool busy)
    {
        checkoutBusy = busy;
    }
}
