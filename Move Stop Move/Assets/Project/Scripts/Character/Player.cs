namespace Project.Scripts.Character
{
    public class Player : Character
    {
        // private bool _isMoving;
        //
        // private void Update()
        // {
        //     if (Input.GetMouseButton(0) && StateUI.Instance.IsState(StateType.Gameplay)) _isMoving = true;
        //     if (Input.GetMouseButtonUp(0))
        //     {
        //         _isMoving = false;
        //         if (HasEnemyInRange())
        //         {
        //             var direction = (Target.position - skin.position).normalized;
        //             if (direction != Vector3.zero) skin.forward = direction;
        //             ChangeAnim(Constants.AnimAttack);
        //         }
        //         else
        //         {
        //             ChangeAnim(Constants.AnimIdle);
        //         }
        //     }
        //
        //     if (!_isMoving)
        //     {
        //         if (HasEnemyInRange())
        //         {
        //             var direction = (Target.position - skin.position).normalized;
        //             if (direction != Vector3.zero) skin.forward = direction;
        //             ChangeAnim(Constants.AnimAttack);
        //         }
        //         else
        //         {
        //             ChangeAnim(Constants.AnimIdle);
        //         }
        //     }
        // }
        //
        // private void FixedUpdate()
        // {
        //     
        //     if (_isMoving)
        //     {
        //         var nextPoint = JoystickControl.direct * speed * Time.fixedDeltaTime + transform.position;
        //         transform.position = CheckGround(nextPoint);
        //         if (JoystickControl.direct != Vector3.zero)
        //         {
        //             skin.forward = JoystickControl.direct;
        //             ChangeAnim(Constants.AnimRun);
        //         }
        //     }
        // }
        //
        //
        // public override void OnHit()
        // {
        //     base.OnHit();
        //     StateUI.Instance.ChangeState(StateType.Revive);
        // }
    }
}