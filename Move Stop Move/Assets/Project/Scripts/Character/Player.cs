using Cinemachine;
using Packages.JoystickPack.Scripts;
using Project.Scripts.Anim;
using UnityEngine;

namespace Project.Scripts.Character
{
    public class Player : Character
    {
        private CinemachineVirtualCamera _cameraFollow;
        private bool _isMoving;

        private void Update()
        {
            if (Input.GetMouseButton(0)) _isMoving = true;

            if (Input.GetMouseButtonUp(0))
            {
                _isMoving = false;
                if (HasEnemyInRange())
                {
                    ChangeAnim(Constants.AnimAttack);
                    var direction = (target.position - transform.position).normalized;
                    if (direction != Vector3.zero) skin.forward = direction;
                }
                else
                {
                    ChangeAnim(Constants.AnimIdle);
                }
            }
        }

        private void FixedUpdate()
        {
            if (_isMoving)
            {
                var nextPoint = JoystickControl.direct * speed * Time.fixedDeltaTime + transform.position;
                transform.position = CheckGround(nextPoint);
                if (JoystickControl.direct != Vector3.zero)
                {
                    skin.forward = JoystickControl.direct;
                    ChangeAnim(Constants.AnimRun);
                }
            }
        }
    }
}