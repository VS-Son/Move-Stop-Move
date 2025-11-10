using UnityEngine;
using System.Linq;

public class Character : GameUnit
{
   [SerializeField] protected Transform skin;
   [SerializeField] protected float speed;
   [SerializeField] protected float throwRangeSize;
   [SerializeField] protected Transform throwRange;
   [SerializeField] protected Transform throwPos;
   [SerializeField] protected GameObject throwItemPrefab;
   [SerializeField] protected Animator animator;
   private EventsAnimManager _eventsAnimManager;
   private Collider[] Hits =>  Physics.OverlapSphere(throwRange.position, rangeSize, LayerMask.GetMask("Enemy"));

   private float rangeSize
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
      _eventsAnimManager = EventsAnimManager.Get(animator);
      if (_eventsAnimManager != null)
      {
         _eventsAnimManager.OnRegister(TypeEventsAnim.Throw, OnThrow);
         _eventsAnimManager.OnRegister(TypeEventsAnim.EndThrow, EndThrow);
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
      return Hits.Length > 0;
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

      return TF.position;
   }

   protected void ChangeAnim(string animName)
   {
      if (_currentAnim != animName)
      {
         // Chỉ reset trigger nếu _currentAnim không rỗng và hợp lệ
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
      if (Hits.Length == 0) return null;
      Collider nearest = Hits.OrderBy(e => Vector3.Distance(throwPos.position, e.transform.position)).First();
      return nearest.transform;
   }

   private void OnThrow()
   {
      Transform target = GetNearestEnemy();
      var position = throwPos.position;
      if (target == null) return;
      GameObject item = Instantiate(throwItemPrefab, position, Quaternion.identity);
      Vector3 direction = (target.position - position).normalized;
      Rigidbody rb = item.GetComponent<Rigidbody>();
      if (rb != null)
      {
         rb.velocity = direction * 10; 

      }

   }
}

