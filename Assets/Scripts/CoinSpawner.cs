using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CoinSpawner : MonoBehaviour
{
    public int minimumCount = 3;
    public int maximumCount = 5;
    public GameObject prefab = null;

    public void Spawn()
    {
        // Randomly pick the count of prefabs to spawn.
        int count = Random.Range(minimumCount, maximumCount);
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