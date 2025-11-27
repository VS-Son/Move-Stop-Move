using Cinemachine;
using Packages.JoystickPack.Scripts;
using Project.Scripts.Anim;
using Project.Scripts.UI.Manager;
using Project.Scripts.UI.Screen;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Scripts.Character
{
    public class Player : Character
    {
        public CinemachineVirtualCamera cameraFollow;
        public Vector3 tranFollow;
        private bool _isMoving;
        private CinemachineTransposer _transposer;

        protected override void OnInit()
        {
             _transposer = cameraFollow.GetCinemachineComponent<CinemachineTransposer>();

        }
        private void Update()
        {
            if (Input.GetMouseButton(0) && StateUI.Instance.IsState(StateType.Gameplay)) _isMoving = true;
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
            var eulerCamera = cameraFollow.transform.eulerAngles;
            var eulerSkin = skin.transform.eulerAngles;
            if (StateUI.Instance.IsState(StateType.MainMenu))
            {
                _transposer.m_FollowOffset = new Vector3(0,1,-5);
                cameraFollow.transform.eulerAngles = new Vector3(12f, eulerCamera.y, eulerCamera.z);
                skin.transform.eulerAngles = new Vector3(eulerSkin.x, 180, eulerSkin.z);
                throwRange.gameObject.SetActive(false);
            }
            else
            {
                _transposer.m_FollowOffset = new Vector3(0,7,-5);
                cameraFollow.transform.eulerAngles = new Vector3(50, eulerCamera.y, eulerCamera.z);
                throwRange.gameObject.SetActive(true);

            }
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

        public override void OnHit()
        {
            base.OnHit();
            StateUI.Instance.ChangeState( StateType.Revive);
        }
    }
}