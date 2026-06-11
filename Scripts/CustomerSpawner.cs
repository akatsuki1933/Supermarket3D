using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab;
    public float spawnTime = 3f;

    void Start()
    {
        InvokeRepeating("SpawnCustomer", 1f, spawnTime);
    }

    void SpawnCustomer()
    {
        Instantiate(customerPrefab, transform.position, Quaternion.identity);
    }
}