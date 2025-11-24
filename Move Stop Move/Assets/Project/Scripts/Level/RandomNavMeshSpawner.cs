using System;
using System.Collections.Generic;
using Project.Scripts.Character;
using Project.Scripts.Pool;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum SpawnCharacterType
{
    Bot,
    Player
}

[Serializable]
public class SpawnCharacterData
{
    public SpawnCharacterType spawnCharacterType;
    public string character;
}

namespace Project.Scripts.Level
{
    public class RandomNavMeshSpawner : MonoBehaviour
    {
        public SpawnCharacterData[] spawnCharacterData = new SpawnCharacterData[2];
        public Character.Character _prefab;
        public int spawnCount;
        public float minDistance;
        public float sampleRadius;

        private readonly Dictionary<SpawnCharacterType, string> _charactersType = new();

        private readonly Vector3 center = Vector3.zero;
        private readonly List<Character.Character> listBot = new();
        private readonly List<Vector3> spawnedPositions = new();


        private void Start()
        {
            foreach (var data in spawnCharacterData)
                if (!_charactersType.ContainsKey(data.spawnCharacterType))
                    _charactersType.Add(data.spawnCharacterType, data.character);
            SpawnPrefab();
        }

        private void SpawnPrefab()
        {
            GenerateCharacter(SpawnCharacterType.Player);
            var botNeeded = spawnCount - 1;

            for (var i = 0; i < botNeeded; i++)
                GenerateCharacter(SpawnCharacterType.Bot);
        }

        private void GenerateCharacter(SpawnCharacterType type)
        {
            if (!_charactersType.ContainsKey(type)) return;
            var prefab = Resources.Load<Character.Character>("Prefabs/Character/" + _charactersType[type]);
            if (TryGetValidPosition(out var pos))
            {
                var bot = SimplePool.Spawn<Bot>(prefab, pos, Quaternion.identity);
                spawnedPositions.Add(pos);
                listBot.Add(bot);
            }
        }

        private bool TryGetValidPosition(out Vector3 result)
        {
            var maxTry = 50;

            while (maxTry > 0)
            {
                maxTry--;


                var randomPoint = center + new Vector3(
                    Random.Range(-sampleRadius, sampleRadius),
                    0,
                    Random.Range(-sampleRadius, sampleRadius)
                );

                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 5f, NavMesh.AllAreas))
                {
                    var candidate = hit.position;

                    var valid = true;
                    foreach (var pos in spawnedPositions)
                        if (Vector3.Distance(candidate, pos) < minDistance)
                        {
                            valid = false;
                            break;
                        }

                    if (valid)
                    {
                        result = candidate;
                        return true;
                    }
                }
            }

            result = Vector3.zero;
            return false;
        }

        public void OnResetRandom()
        {
            foreach (var bot in listBot) SimplePool.Despawn(bot);
            listBot.Clear();
            spawnedPositions.Clear();
            SpawnPrefab();
        }
    }
}