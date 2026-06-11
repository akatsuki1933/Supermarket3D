using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Customer : MonoBehaviour
{
    [Header("Movement")]
    public float stoppingDistance = 1f;

    private int state = 0;
    private int queueIndex = -1;
    private bool waitingInQueue = false;
    private bool arrivedAtQueue = false;
    private bool isPaying = false;
    private bool isCollecting = false;
    private bool isShelfShopping = false;

    private int cartValue = 0;
    private int targetProducts;
    private int collectedProducts = 0;

    private int trashDropped = 0;
    private int maxTrashDrops;
    private bool willDropTrash;

    private Transform entrance;
    private Transform marketPoint;
    private Transform exit;

    private Shelf targetShelf;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        willDropTrash = Random.value < 0.10f;
        maxTrashDrops = Random.Range(1, 3);

        entrance = GameObject.Find("Entrance").transform;
        marketPoint = GameObject.Find("MarketPoint").transform;
        exit = GameObject.Find("Exit").transform;

        targetProducts = Random.Range(2, 6);

        // Empezar desde el spawner hacia la entrada
        NextState();
    }

    void Update()
    {
        CheckArrival();
        TryDropTrash();
        CheckQueue();
    }

    void SetTarget(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    void CheckArrival()
    {
        if (agent.pathPending) return;
        if (agent.remainingDistance > stoppingDistance) return;
        if (!agent.hasPath) return;

        if (state == 4 && waitingInQueue)
        {
            arrivedAtQueue = true;
            return;
        }

        if (state == 3) return; // OnTriggerEnter maneja el state 3

        agent.ResetPath();
        StartCoroutine(WaitBeforeNextState());
    }

    IEnumerator WaitBeforeNextState()
    {
        yield return null;
        NextState();
    }

    void NextState()
    {
        state++;

        switch (state)
        {
            case 1:
                SetTarget(entrance.position);
                break;

            case 2:
                SetTarget(marketPoint.position);
                break;

            case 3:
                FindShelf();
                break;

            case 4:
                if (collectedProducts == 0)
                {
                    state = 5; // saltar directo a salida
                    SetTarget(exit.position);
                    return;
                }
                queueIndex = CheckoutManager.Instance.JoinQueue(this);
                MoveToQueuePosition(queueIndex);
                waitingInQueue = true;
                break;

            case 5:
                SetTarget(exit.position);
                break;

            default:
                Destroy(gameObject);
                break;
        }
    }

    void FindShelf()
    {
        Shelf[] shelves = FindObjectsOfType<Shelf>();

        if (shelves.Length == 0)
        {
            SetTarget(exit.position);
            return;
        }

        targetShelf = shelves[Random.Range(0, shelves.Length)];
        SetTarget(targetShelf.transform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (targetShelf == null) return;
        if (isCollecting) return;

        Shelf shelf = other.GetComponent<Shelf>();

        if (shelf != null && shelf == targetShelf)
        {
            Debug.Log("Tocó shelf - collectedProducts: " + collectedProducts + " | targetProducts: " + targetProducts);
            
            isCollecting = true;

            bool success = shelf.TakeProduct();
            Debug.Log("TakeProduct success: " + success);
            
            if (success)
            {
                cartValue += shelf.productPrice;
                collectedProducts++;
            }

            targetShelf = null;
            isCollecting = false;

            if (!success)
            {
                FindShelf();
                return;
            }

            if (collectedProducts >= targetProducts)
                NextState();
            else
                FindShelf();
        }
    }

    public void MoveToQueuePosition(int index)
    {
        queueIndex = index;
        arrivedAtQueue = false;
        Transform point = CheckoutManager.Instance.GetQueuePoint(index);
        SetTarget(point.position);
    }

    void CheckQueue()
    {
        if (!waitingInQueue) return;
        if (isPaying) return;
        if (!arrivedAtQueue) return;
        Debug.Log("Intentando pagar - CanPay: " + CheckoutManager.Instance.CanPay(this));
        if (CheckoutManager.Instance.CanPay(this))
            StartCoroutine(PayRoutine());
    }

    void TryDropTrash()
    {
        if (!willDropTrash) return;
        if (state != 3) return; // state 3 es cuando buscan shelves
        if (trashDropped >= maxTrashDrops) return;

        if (Random.value < 0.001f)
        {
            CleanManager.Instance.SpawnTrash(transform.position);
            trashDropped++;
        }
    }

    IEnumerator PayRoutine()
    {
        isPaying = true;
        CheckoutManager.Instance.SetBusy(true);

        yield return new WaitForSeconds(2f);

        MoneyManager.Instance.AddMoney(cartValue);
        cartValue = 0;
        waitingInQueue = false;
        CheckoutManager.Instance.LeaveQueue(this);
        CheckoutManager.Instance.SetBusy(false);
        isPaying = false;

        NextState();
    }
}