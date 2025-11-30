using System.Linq;
using Project.Scripts.Character.StateMachine;
using Project.Scripts.UI.Manager;
using UnityEngine;

namespace Project.Scripts.Characters
{
    public class Bot : Character
    {
        private IState<Bot> _currentState;

        private void Update()
        {
            if (StateUI.IsState(StateType.MainMenu)) return;
            if (_currentState != null) _currentState.OnExecute(this);
        }

        private void FixedUpdate()
        {
        }

        public void ChangeState(IState<Bot> state)
        {
            if (_currentState != null) _currentState.OnExit(this);

            _currentState = state;

            if (_currentState != null) _currentState.OnEnter(this);
        }

        public Transform GetRandomEnemyGlobal()
        {
            var allEnemies = FindObjectsOfType<Characters.Character>().Where(b => b != this).ToArray();
            if (allEnemies.Length == 0) return null;

            var randomIndex = Random.Range(0, allEnemies.Length);
            return allEnemies[randomIndex].transform;
        }


        public override void OnHit()
        {
            base.OnHit();
        }


        public new bool HasEnemyInRange()
        {
            var allEnemies = FindObjectsOfType<Characters.Character>().Where(b => b != this).ToArray();
            foreach (var enemy in allEnemies)
                if (Vector3.Distance(transform.position, enemy.transform.position) <= RangeSize)
                    return true;
            return false;
        }
    }
}