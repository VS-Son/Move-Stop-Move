using Project.Scripts.Anim;
using UnityEngine;

namespace Project.Scripts.Character.StateMachine
{
    public class AttackState : IState<Bot>
    {
        private readonly Transform _target;

        public AttackState(Transform enemy)
        {
            _target = enemy;
        }

        public void OnEnter(Bot t)
        {
            t.ChangeAnim(Constants.AnimAttack);
        }

        public void OnExecute(Bot t)
        {
            if (!t.HasEnemyInRange() || _target == null)
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