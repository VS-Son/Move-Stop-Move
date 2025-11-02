using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    void Update()
    {

        if (Input.GetMouseButton(0))
        {

            Vector3 nextPoint = JoystickControl.direct * speed * Time.deltaTime + TF.position;


            TF.position = CheckGround(nextPoint);
            if (JoystickControl.direct != Vector3.zero)
            {
                skin.forward = JoystickControl.direct;
            }

            ChangeAnim("run");
        }

        if (Input.GetMouseButtonUp(0))
        {
            ChangeAnim("idle");
        }


    }
}
