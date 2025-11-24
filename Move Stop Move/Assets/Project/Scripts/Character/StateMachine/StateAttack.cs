using Project.Scripts.Anim;
using UnityEngine;

namespace Project.Scripts.Character.StateMachine
{
    public class StateAttack : IState<Bot>
    {
        private Transform _target;

        public StateAttack(Transform enemy)
        {
            _target = enemy;
        }

        public void OnEnter(Bot t)
        {
            //if (t.HasEnemyInRange()) t.ChangeAnim(Constants.AnimIdle);
        }

        public void OnExecute(Bot t)
        {
            var currentTarget = t.target;
            if (!t.HasEnemyInRange())
            {
                t.ChangeState(new PatrolState());
                return;
            }

            if (currentTarget == null)
            {
                t.ChangeState(new PatrolState());
                return;
            }

            t.ChangeAnim(Constants.AnimAttack);
        }

        public void OnExit(Bot t)
        {
        }
    }
}