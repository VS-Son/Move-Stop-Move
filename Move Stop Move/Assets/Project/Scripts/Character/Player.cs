using UnityEngine;

namespace Project.Scripts.Character
{
    public class Player : Character
    {
        private bool _isMoving = false;
        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                _isMoving = true;
           
            
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isMoving = false;
                if (HasEnemyInRange())
                {
                    ChangeAnim("attack");
                    Vector3 direction = (target.position - transform.position).normalized;
                    if (direction != Vector3.zero)
                    {
                        skin.forward = direction;
                    }
                }
                else
                {
                    ChangeAnim("idle");
                }
            }
        }

        private void FixedUpdate()
        {
            if (_isMoving)
            {
                Vector3 nextPoint = JoystickControl.direct * speed * Time.fixedDeltaTime + transform.position;
                transform.position = CheckGround(nextPoint);
                if (JoystickControl.direct != Vector3.zero)
                {
                    skin.forward = JoystickControl.direct;
                    ChangeAnim("run");
                }
            }
       
        }
    }
}
