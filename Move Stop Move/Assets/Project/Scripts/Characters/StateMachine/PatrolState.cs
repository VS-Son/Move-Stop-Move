using Project.Scripts.Anim;
using Project.Scripts.Character.StateMachine;
using UnityEngine;

namespace Project.Scripts.Characters.StateMachine
{
    public class PatrolState : IState<Bot>
    {
        private readonly float _moveDistance = 10f;
        private Transform _currentTarget;
        private float _idleTimer;
        private bool _isIdle;
        private Vector3 _moveDirection;
        private float _timer;
        private float MoveDuration => Random.Range(2f, 4f);
        private float IdleTime => Random.Range(1f, 3f);

        public void OnEnter(Bot t)
        {
            _timer = MoveDuration;
            _isIdle = false;
            _idleTimer = 0f;
            SelectTarget(t);
            t.ChangeAnim(Constants.AnimRun);
        }

        public void OnExecute(Bot t)
        {
            if (t.HasEnemyInRange())
            {
                t.ChangeState(new AttackState(t.GetNearestEnemy()));
                return;
            }

            if (_isIdle)
            {
                _idleTimer -= Time.deltaTime;
                t.ChangeAnim(Constants.AnimIdle);
                if (_idleTimer <= 0f)
                {
                    _isIdle = false;
                    _timer = MoveDuration;
                    SelectTarget(t);
                }

                return;
            }

            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                _isIdle = true;
                _idleTimer = IdleTime;
                t.agent.ResetPath();
                return;
            }

            if (_currentTarget != null && t.agent != null && t.agent.isActiveAndEnabled)
            {
                if (_moveDirection != Vector3.zero)
                {
                    var targetRotation = Quaternion.LookRotation(_moveDirection);
                    t.transform.rotation = Quaternion.Slerp(t.transform.rotation, targetRotation, Time.deltaTime * 10f);
                }

                t.ChangeAnim(Constants.AnimRun);
            }
        }

        public void OnExit(Bot t)
        {
            if (t.agent != null && t.agent.isActiveAndEnabled)
                t.agent.ResetPath();
        }

        private void SelectTarget(Bot t)
        {
            var target = t.GetRandomEnemyGlobal();
            if (target != null)
            {
                _currentTarget = target;
                _moveDirection = (_currentTarget.position - t.transform.position).normalized;
                var destination = t.transform.position + _moveDirection * _moveDistance;
                t.agent.SetDestination(destination);
            }
            else
            {
                _currentTarget = null;
            }
        }
    }
}