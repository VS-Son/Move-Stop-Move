using System;
using UnityEngine;
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
            ChangeAnim(HasEnemyInRange() ? "attack" : "idle");
        }

    
    }

    private void FixedUpdate()
    {
        if (_isMoving)
        {
            Vector3 nextPoint = JoystickControl.direct * speed * Time.fixedDeltaTime + TF.position;
            TF.position = CheckGround(nextPoint);
            if (JoystickControl.direct != Vector3.zero)
            {
                skin.forward = JoystickControl.direct;
                ChangeAnim("run");
            }
        }
       
    }
}
