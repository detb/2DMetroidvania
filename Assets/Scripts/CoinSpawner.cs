using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CoinSpawner : MonoBehaviour
{
    [Header("Coin amount and prefab")]
        [SerializeField]
        private int minimumCount = 3;
        [SerializeField]
        private int maximumCount = 5;
        [SerializeField]
        private GameObject prefab = null;

    public void Spawn()
    {
        // Randomly pick the count of prefabs to spawn.
        int count = Random.Range(minimumCount, maximumCount);
        // Spawn them!
        for (int i = 0; i < count; ++i)
        {
            float randomX = Random.Range(-0.1f, 0.1f);
            float randomY = Random.Range(0.05f, 0.15f);
            var position = transform.position;
            Vector3 spawnPosition = new Vector3(position.x + randomX, position.y + randomY);
            Instantiate(prefab, spawnPosition, Quaternion.identity);
        }
    }
}