using UnityEngine;

namespace Project.Scripts.Character
{
    public class Bot : Character
    {
        private void Update()
        {
            if (HasEnemyInRange())
            {
                ChangeAnim("attack");
                var direction = (target.position - transform.position).normalized;
                if (direction != Vector3.zero) skin.forward = direction;
            }
            else
            {
                ChangeAnim("idle");
            }
        }
    }
}