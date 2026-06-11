using UnityEngine;
using System.Collections.Generic;

public class CleanManager : MonoBehaviour
{
    public static CleanManager Instance;

    public GameObject trashPrefab;

    private List<Trash> trashList = new List<Trash>();

    // 0 = limpio, 100 = muy sucio
    public float dirtLevel = 0f;

    void Awake()
    {
        Instance = this;
    }

    public void SpawnTrash(Vector3 position)
    {
        GameObject t = Instantiate(trashPrefab, position, Quaternion.identity);
        Trash trash = t.GetComponent<Trash>();
        trashList.Add(trash);
        dirtLevel = Mathf.Min(100f, dirtLevel + 5f);
    }

    public void RemoveTrash(Trash trash)
    {
        trashList.Remove(trash);
        dirtLevel = Mathf.Max(0f, dirtLevel - 5f);
    }
}