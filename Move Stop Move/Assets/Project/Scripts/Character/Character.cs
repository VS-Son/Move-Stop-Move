using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Character: Singleton<Character>
{
   [SerializeField] protected Transform skin;
   [SerializeField] protected float speed;
   [SerializeField] protected float throwRangeSize;
    public Transform throwRange;
   [SerializeField] protected Transform throwPos;
   [SerializeField] protected ThrowItem throwItemPrefab;
   [SerializeField] protected Animator animator;
   private EventsAnimManager eventsAnimManager => EventsAnimManager.Get(animator);
   private Collider[] hits =>  Physics.OverlapSphere(throwRange.position, rangeSize, LayerMask.GetMask("Enemy"));
  private Queue<ThrowItem> listThrowItems = new Queue<ThrowItem>(); 
   public float rangeSize
   {
      get => throwRangeSize;
      set
      {
         throwRangeSize = value;
         UpdateThrowRange();
      }
      
   }

   private string _currentAnim;

   private void Start()
   {
      OnInit();
      UpdateThrowRange();
      
   }

   private void OnInit()
   {
      if (eventsAnimManager != null)
      {
         eventsAnimManager.OnRegister(TypeEventsAnim.Throw, OnThrow);
         eventsAnimManager.OnRegister(TypeEventsAnim.EndThrow, EndThrow);
      }
   }

   private void EndThrow()
   {

   }

#if UNITY_EDITOR
   private void OnValidate()
   {
      UpdateThrowRange();
   }

#endif
   private void UpdateThrowRange()
   {
      throwRange.localScale = new Vector3(throwRangeSize * 2, throwRangeSize * 2, 1f);
   }

   protected bool HasEnemyInRange()
   {
      // return Physics2D.OverlapCircle(ThrowRange.position, AttackRangeSize, 0, LayerMask.GetMask("Enemy"));
      // Collider[] hits = Physics.OverlapSphere(throwRange.position, rangeSize, LayerMask.GetMask("Enemy"));
      return hits.Length > 0;
   }

   private void OnDrawGizmosSelected()
   {
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(throwRange.position, rangeSize);
   }

   protected Vector3 CheckGround(Vector3 nextPoint)
   {
      if (Physics.Raycast(nextPoint, Vector3.down, out var hit, 2f,
             LayerMask.GetMask("Ground")))
      {
         return hit.point + Vector3.up * 1.1f;
      }

      return transform.position;
   }

   protected void ChangeAnim(string animName)
   {
      if (_currentAnim != animName)
      {
         if (!string.IsNullOrEmpty(_currentAnim))
         {
            animator.ResetTrigger(_currentAnim);
         }
         _currentAnim = animName;
         animator.SetTrigger(_currentAnim);
      }
   }

   protected Transform GetNearestEnemy()
   {
      if (hits.Length == 0) return null;
      Collider nearest = hits.OrderBy(e => Vector3.Distance(throwPos.position, e.transform.position)).First();
      return nearest.transform;
   }

   private void OnThrow()
   {
      Transform target = GetNearestEnemy();
      var position = throwPos.position;
      if (target == null) return;
      ThrowItem item = Instantiate(throwItemPrefab, position, Quaternion.identity);
      Vector3 direction = (target.position - position).normalized;
      listThrowItems.Enqueue(item);
      StartCoroutine(HideItemThrow());
      if (item.rigidbody != null)
      {
         item.rigidbody.velocity = direction * 10;
      }
   }

 

   IEnumerator HideItemThrow()
   {
      foreach (var t in listThrowItems)
      {
         if (Vector3.Distance(t.transform.position, throwRange.position) > rangeSize )
         {
            t.gameObject.SetActive(false);
         }
      }

      yield return null;
   }
}

