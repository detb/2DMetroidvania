using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CoinSpawner : MonoBehaviour
{
    [SerializeField] private int minimumCount = 3;
    [SerializeField] private int maximumCount = 5;
    [SerializeField] private GameObject prefab = null;

    public int MinimumCount
    {
        get { return minimumCount; }
        set { minimumCount = value; }
    }
    public int MaximumCount
    {
        get { return maximumCount; }
        set { maximumCount = value; }
    }
    public GameObject Prefab
    {
        get { return prefab; }
        set { prefab = value; }
    }

    public void Spawn()
    {
        // Randomly pick the count of prefabs to spawn.
        int count = Random.Range(MinimumCount, MaximumCount);
        // Spawn them!
        for (int i = 0; i < count; ++i)
        {
            float randomX = Random.Range(-0.1f, 0.1f);
            float randomy = Random.Range(0f, 0.15f);
            Vector3 position = new Vector3(transform.position.x + randomX, transform.position.y + randomy);
            Instantiate(prefab, position, Quaternion.identity);
        }
    }
}