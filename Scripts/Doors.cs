using UnityEngine;

public class Door : MonoBehaviour
{
    public float openDistance = 2.5f;
    public float openSpeed = 3f;
    public bool isLeftDoor = true;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private Transform player;

    void Start()
    {
        closedPosition = transform.position;

        // Izquierda va a la izquierda, derecha va a la derecha
        float direction = isLeftDoor ? 1f : -1f;
        openPosition = closedPosition + new Vector3(direction * 1.5f, 0, 0);

        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        // Buscar cualquier objeto cercano (jugador o cliente)
        Collider[] nearby = Physics.OverlapSphere(transform.position, openDistance);
        bool shouldOpen = false;

        foreach (Collider col in nearby)
        {
            if (col.CompareTag("Player") || col.CompareTag("Customer"))
            {
                shouldOpen = true;
                break;
            }
        }

        Vector3 target = shouldOpen ? openPosition : closedPosition;

        transform.position = Vector3.Lerp(
            transform.position,
            target,
            openSpeed * Time.deltaTime
        );
    }
}