using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class RandomNavMeshSpawner : MonoBehaviour
{
    private GameObject prefab => Resources.Load<GameObject>("Prefabs/Character/Bot");
    public float spawnRadius = 20f;
    public int maxAttempts = 30;
    public int numberBots ;

    private void Start()
    {
        for (int number = 0; number < numberBots; number++)
        {
            Spawn();

        }
    }

    public void Spawn()
    {
        Vector3 randomPos = GetRandomPointOnNavMesh(transform.position, spawnRadius);
       
        if (randomPos != Vector3.zero)
        {
            Instantiate(prefab, randomPos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Không tìm được vị trí NavMesh hợp lệ để spawn!");
        }
    }

    private Vector3 GetRandomPointOnNavMesh(Vector3 center, float radius)
    {
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * radius;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPoint, out hit, 5f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return Vector3.zero; // không tìm thấy
    }
}

