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
   [SerializeField] protected Transform throwRange;
   [SerializeField] protected Transform throwPos;
   [SerializeField] protected ThrowItem throwItemPrefab;
   [SerializeField] protected Animator animator;
   private EventsAnimManager eventsAnimManager => EventsAnimManager.Get(animator);
   private Collider[] hits =>  Physics.OverlapSphere(throwRange.position, rangeSize, LayerMask.GetMask("Enemy"));
   protected  Transform target => GetNearestEnemy();
   private Queue<ThrowItem> listThrowItems = new Queue<ThrowItem>(); 
   private Coroutine _hideCoroutine;

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

   private Transform GetNearestEnemy()
   {
      if (hits.Length == 0) return null;
      Collider nearest = hits.OrderBy(e => Vector3.Distance(throwPos.position, e.transform.position)).First();
      return nearest.transform;
   }

   private void OnThrow()
   {
     
      var position = throwPos.position;
      if (target == null) return;

      ThrowItem item = SimplePool.Spawn<ThrowItem>(throwItemPrefab, position, Quaternion.identity);
      listThrowItems.Enqueue(item);

      Rigidbody rb = item.GetComponent<Rigidbody>();
      if (rb != null)
      {
         rb.velocity = skin.forward * 10;
      }

      if (_hideCoroutine == null)
      {
         _hideCoroutine = StartCoroutine(HideWeaponThrow());
      }
   }
   
   IEnumerator HideWeaponThrow()
   {
      while (listThrowItems.Count > 0)
      {
         foreach (var t in listThrowItems.ToArray())
         {
            if (Vector3.Distance(t.transform.position, throwRange.position) > rangeSize + 0.5f)
            {
               SimplePool.Despawn(t);
            }
         }

         yield return null;
      }
      _hideCoroutine = null;
   }

   
}

