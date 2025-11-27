using System;
using System.Collections.Generic;
using Project.Scripts.Anim.EventAnim;
using Project.Scripts.Character;
using Project.Scripts.Character.StateMachine;
using Project.Scripts.Pool;
using Project.Scripts.UI.Manager;
using Project.Scripts.UI.Screen;
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
    public class RandomNavMeshSpawner : Singleton<RandomNavMeshSpawner>
    {
        [SerializeField] private List< SpawnCharacterData> spawnCharacterData = new List<SpawnCharacterData>();
        [SerializeField] private int spawnCount;
        [SerializeField] private float minDistance;
        [SerializeField] private float sampleRadius;
        [SerializeField] private int totalAlive = 10;

        private readonly Dictionary<SpawnCharacterType, string> _charactersType = new();

        private readonly Vector3 _center = Vector3.zero;
        private readonly List<Character.Character> _listBot = new();
        public List<Vector3> SpawnedPositions { get; } = new();

        public int TotalAlive
        {
            get => totalAlive;
            set => totalAlive = value;
        }


        private void Awake()
        {
        }

        private void Start()
        {
            UIManager.Instance.OpenUI<MainMenu>();
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
            {
                GenerateCharacter(SpawnCharacterType.Bot);
            }
                
        }

        public void GenerateCharacter(SpawnCharacterType type)
        {
            if (TotalAlive < spawnCount) return;
            if (!_charactersType.TryGetValue(type, out var value)) return;
            var prefab = Resources.Load<Character.Character>("Prefabs/Character/" + value);
            if (TryGetValidPosition(out var pos))
            {
                var bot = SimplePool.Spawn<Bot>(prefab, pos, Quaternion.identity);
                SpawnedPositions.Add(pos);
                _listBot.Add(bot);
            }


        }

        private bool TryGetValidPosition(out Vector3 result)
        {
            var maxTry = 50;

            while (maxTry > 0)
            {
                maxTry--;


                var randomPoint = _center + new Vector3(
                    Random.Range(-sampleRadius, sampleRadius),
                    0,
                    Random.Range(-sampleRadius, sampleRadius)
                );

                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 5f, NavMesh.AllAreas))
                {
                    var candidate = hit.position;

                    var valid = true;
                    foreach (var pos in SpawnedPositions)
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
            foreach (var bot in _listBot) SimplePool.Despawn(bot);
            _listBot.Clear();
            SpawnedPositions.Clear();
            SpawnPrefab();
        }

        public void OnStartGame()
        {
            foreach (var character in _listBot)
            {
                if (character is Bot bot)
                    bot.ChangeState(new PatrolState());
            }

        }
    }
}