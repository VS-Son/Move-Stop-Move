using System;
using System.Collections.Generic;
using Project.Scripts.Character;
using Project.Scripts.Utility;
using Project.Scripts.Character.ScriptableObject;
using Project.Scripts.Character.StateMachine;
using Project.Scripts.Characters;
using Project.Scripts.Characters.StateMachine;
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
        readonly List<ColorType> _colorTypes = new List<ColorType>() { ColorType.Default, ColorType.Black, ColorType.Red, ColorType.Blue, ColorType.Green, ColorType.Yellow, ColorType.Orange, ColorType.Brown, ColorType.Violet };

        [SerializeField] private List<SpawnCharacterData> spawnCharacterData = new();
        [SerializeField] private int characterAmount;
        [SerializeField] private float minDistance;
        [SerializeField] private float sampleRadius;
        [SerializeField] private int totalAlive;
        private List<ColorType> ShuffledColors => Utilities.SortOrder(_colorTypes, characterAmount);
        private readonly Vector3 _center = Vector3.zero;
        private int _colorIndex = 0;

        private readonly Dictionary<SpawnCharacterType, string> _charactersType = new();
        public readonly List<Characters.Character> ListBot = new();
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
            OnInit();
            SpawnPrefab();
        }

        private void OnInit()
        {
            UIManager.Instance.OpenUI<MainMenu>();
            foreach (var data in spawnCharacterData)
                if (!_charactersType.ContainsKey(data.spawnCharacterType))
                    _charactersType.Add(data.spawnCharacterType, data.character);
           
        }

        private void SpawnPrefab()
        {
            GenerateCharacter(SpawnCharacterType.Player);
            var botNeeded = characterAmount - 1;
            for (var i = 0; i < botNeeded; i++) GenerateCharacter(SpawnCharacterType.Bot);
        }

        public void GenerateCharacter(SpawnCharacterType type)
        {
            if (!_charactersType.TryGetValue(type, out var value)) return;
            var prefab = Resources.Load<Characters.Character>("Prefabs/Character/" + value);
            if (TryGetValidPosition(out var pos))
            {
                var character = SimplePool.Spawn<Characters.Character>(prefab, pos, Quaternion.identity);
                var randomColors = ShuffledColors;
                if (_colorIndex >= randomColors.Count)
                    _colorIndex = 0;                 

                var pickedColor = randomColors[_colorIndex];
                _colorIndex++;
                character.ChangeColor(pickedColor);
                ListBot.Add(character);
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

                if (NavMesh.SamplePosition(randomPoint, out var hit, 5f, NavMesh.AllAreas))
                {
                    var candidate = hit.position;

                    var valid = true;

                    // Check ALL bot positions currently alive
                    foreach (var bot in ListBot)
                    {
                        if (bot == null || !bot.gameObject.activeSelf) continue;

                        if (Vector3.Distance(candidate, bot.transform.position) < minDistance)
                        {
                            valid = false;
                            break;
                        }
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


        public void OnResetPlayZone()
        {
            foreach (var bot in ListBot)
            {
                SimplePool.Despawn(bot);
            }
            ListBot.Clear();
            SpawnPrefab();
        }

        public void OnStartGame()
        {
            foreach (var character in ListBot)
                if (character is Bot bot)
                    bot.ChangeState(new PatrolState());
        }

        public void RemoveBot(Characters.Character character)
        {
            ListBot.Remove(character);
            Debug.Log("list bot" + ListBot.Count);
        }
    }

    
}