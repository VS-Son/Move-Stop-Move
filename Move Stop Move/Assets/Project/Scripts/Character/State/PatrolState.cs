using Project.Scripts.Anim;
using UnityEngine;

namespace Project.Scripts.Character.State
{
    public class PatrolState : IState<Bot>
    {
        private readonly float _moveDuration = 3f;
        private Transform _currentTarget;
        private float _timer;

        public void OnEnter(Bot t)
        {
            _timer = _moveDuration;
            SelectTarget(t);
            t.ChangeAnim(Constants.AnimRun);
        }

        public void OnExecute(Bot t)
        {
            _timer -= Time.deltaTime;
            if (_currentTarget == null || _timer <= 0f)
            {
                _timer = _moveDuration;
                SelectTarget(t);
            }

            if (_currentTarget != null)
            {
                // set đích NavMeshAgent về vị trí target
                if (t.Agent.isActiveAndEnabled)
                    t.Agent.SetDestination(_currentTarget.position);

                // nếu target vào range → chuyển sang AttackState
                var distance = Vector3.Distance(t.transform.position, _currentTarget.position);
                if (distance <= t.rangeSize) t.ChangeState(new StateAttack(_currentTarget));
            }
        }

        public void OnExit(Bot t)
        {
            if (t.Agent.isActiveAndEnabled)
                t.Agent.ResetPath();
        }

        private void SelectTarget(Bot t)
        {
            _currentTarget = t.GetNearestEnemyGlobal();
        }
    }
}