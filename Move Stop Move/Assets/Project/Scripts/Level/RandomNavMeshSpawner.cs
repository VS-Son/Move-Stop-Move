using System.Collections.Generic;
using Project.Scripts.Character;
using Project.Scripts.Pool;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.Level
{
    public class RandomNavMeshSpawner : MonoBehaviour
    {
        public float spawnRadius = 20f;
        public float minDistance = 100000;
        public int maxAttempts = 40;
        public int numberBots;

        private readonly List<Transform> spawnedBots = new();

        private Bot Prefab => Resources.Load<Bot>("Prefabs/Character/Bot");

        private void Start()
        {
            for (var i = 0; i < numberBots; i++)
                Spawn();
        }

        private void Spawn()
        {
            for (var attempts = 0; attempts < maxAttempts; attempts++)
            {
                var randomPos = GetRandomPointOnNavMesh(transform.position, spawnRadius);

                if (randomPos != Vector3.zero && IsFarEnough(randomPos))
                {
                    var bot = SimplePool.Spawn<Bot>(Prefab, randomPos, Quaternion.identity);

                    if (bot.gameObject.activeInHierarchy)
                        spawnedBots.Add(bot.transform);

                    return;
                }
            }

            Debug.LogWarning("Cannot find valid spawn point with required spacing.");
        }

        private bool IsFarEnough(Vector3 pos)
        {
            spawnedBots.RemoveAll(b => b == null || !b.gameObject.activeInHierarchy);

            foreach (var bot in spawnedBots)
            {
                var a = new Vector3(bot.position.x, 0, bot.position.z);
                var b = new Vector3(pos.x, 0, pos.z);

                if (Vector3.Distance(a, b) < minDistance)
                    return false;
            }

            return true;
        }

        private Vector3 GetRandomPointOnNavMesh(Vector3 center, float radius)
        {
            for (var i = 0; i < maxAttempts; i++)
            {
                var randomPoint = center + Random.insideUnitSphere * radius;
                NavMeshHit hit;

                if (NavMesh.SamplePosition(randomPoint, out hit, 5f, NavMesh.AllAreas))
                    return hit.position;
            }

            return Vector3.zero;
        }
    }
}