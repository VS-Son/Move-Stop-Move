using UnityEngine;
public class Player : Character
{
   
    private void Update()
    {
        Transform enemy;
        if (HasEnemyInRange())
        {
            Debug.Log("Has Enemy in Range ");
        }
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
