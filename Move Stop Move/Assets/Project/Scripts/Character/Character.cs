   using System;
   using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : GameUnit
{
   [SerializeField] protected Transform skin;
   [SerializeField] protected float speed;
   [SerializeField] protected float attackRangeSize;
   [SerializeField] protected Animator animator;
   [SerializeField] protected Transform attackRange;

   protected float AttackRangeSize
   {
      get => attackRangeSize;
      set
      {
         attackRangeSize = value;
         UpdateAttackRange();
      }
   }
   private string _currentAnim;

   private void Start()
   {
      UpdateAttackRange();
     
   }

#if UNITY_EDITOR
   private void OnValidate()
   {
      UpdateAttackRange(); 
   }

#endif
   private void UpdateAttackRange()
   {
      attackRange.localScale = new Vector3(attackRangeSize * 2, attackRangeSize * 2,1f);
   }

  

   protected bool HasEnemyInRange()
   {
       return Physics2D.OverlapCircle(attackRange.position, AttackRangeSize ,0, LayerMask.GetMask("Enemy"));
   }
   private void OnDrawGizmos()
   {
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(attackRange.position, AttackRangeSize);
   }
   protected Vector3 CheckGround(Vector3 nextPoint)
   {
      if (Physics.Raycast(nextPoint, Vector3.down, out var hit, 2f, 
             LayerMask.GetMask("Ground")))
      {
         return hit.point + Vector3.up * 1.1f;
      }

      return TF.position;
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
