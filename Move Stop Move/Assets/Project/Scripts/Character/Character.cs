   using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : GameUnit
{
   [SerializeField] private LayerMask layerMask;
   [SerializeField] private Animator animator;
   public Transform skin;
   public float speed;
   private string _currentAnim;
   protected Vector3 CheckGround(Vector3 nextPoint)
   {
      RaycastHit hit;

      if (Physics.Raycast(nextPoint, Vector3.down, out hit, 2f, layerMask))
      {
         return hit.point + Vector3.up * 1.1f;
      }

      return TF.position;
   }
   protected bool CanMove(Vector3 nextPoint)
   {
      
      bool isCanMove = true;
      return isCanMove;
   }
   protected void ChangeAnim(string animName)
   {
      if (_currentAnim != animName)
      {
         animator.ResetTrigger(_currentAnim);
         _currentAnim = animName;
         animator.SetTrigger(_currentAnim);
      }
   }
}
