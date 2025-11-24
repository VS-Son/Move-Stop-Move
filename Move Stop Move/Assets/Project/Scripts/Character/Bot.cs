using System.Linq;
using Project.Scripts.Character.StateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.Character
{
    public class Bot : Character
    {
        private IState<Bot> _currentState;

        private void Update()
        {
            if (_currentState != null) _currentState.OnExecute(this);
        }

        protected override void OnInit()
        {
            _agent = GetComponent<NavMeshAgent>();
            ChangeState(new PatrolState());
        }

        public void ChangeState(IState<Bot> state)
        {
            if (_currentState != null) _currentState.OnExit(this);

            _currentState = state;

            if (_currentState != null) _currentState.OnEnter(this);
        }

        public Transform GetNearestEnemyGlobal()
        {
            var allEnemies = FindObjectsOfType<Character>().Where(b => b != this).ToArray();
            if (allEnemies.Length == 0) return null;

            var nearest = allEnemies.OrderBy(b => Vector3.Distance(transform.position, b.transform.position)).First();
            return nearest.transform;
        }
    }
}