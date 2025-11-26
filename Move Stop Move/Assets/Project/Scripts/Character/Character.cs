using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Anim;
using Project.Scripts.Anim.EventAnim;
using Project.Scripts.Character.Attack;
using Project.Scripts.Pool;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Project.Scripts.Character
{
    public class Character : GameUnit
    {
        [SerializeField] protected Transform skin;
        [SerializeField] protected float speed;
        [SerializeField] protected float throwRangeSize;
        [SerializeField] protected Transform throwRange;
        [SerializeField] protected Transform throwPos;
        [SerializeField] protected ThrowItem throwItemPrefab;
        [SerializeField] protected Animator animator;
        [SerializeField] protected EventsAnimManager eventsAnimManager;

        private readonly Queue<ThrowItem> _listThrowItems = new();
        private string _currentAnim;
        private Coroutine _hideCoroutine;
        private float _hp;
        private bool IsDeath => 0 < _hp;

        private Collider[] hits => Physics.OverlapSphere(throwRange.position, rangeSize, LayerMask.GetMask("Enemy"))
            .Where(col => col.transform != transform).ToArray();

        public NavMeshAgent Agent { get; private set; }

        public Transform target => GetNearestEnemy();

        public float rangeSize
        {
            get => throwRangeSize;
            set
            {
                throwRangeSize = value;
                UpdateThrowRange();
            }
        }

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
        }


        private void Start()
        {
            OnInit();
            UpdateThrowRange();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(throwRange.position, rangeSize);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateThrowRange();
        }

#endif

        private void OnInit()
        {
            if (eventsAnimManager != null)
            {
                eventsAnimManager.OnRegister(TypeEventsAnim.Throw, OnThrow);
                //eventsAnimManager.OnRegister(TypeEventsAnim.EndThrow, EndThrow);
                eventsAnimManager.OnRegister(TypeEventsAnim.EndDead, EndDead);
            }
        }


        private void EndDead()
        {
            SimplePool.Despawn(this);
        }

        private void UpdateThrowRange()
        {
            throwRange.localScale = new Vector3(throwRangeSize * 2, throwRangeSize * 2, 1f);
        }

        public bool HasEnemyInRange()
        {
            return hits.Length > 0;
        }

        protected Vector3 CheckGround(Vector3 nextPoint)
        {
            if (Physics.Raycast(nextPoint, Vector3.down, out var hit, 2f,
                    LayerMask.GetMask("Ground")))
                return hit.point + Vector3.up * 1.1f;

            return transform.position;
        }

        public void ChangeAnim(string animName)
        {
            if (_currentAnim != animName)
            {
                if (!string.IsNullOrEmpty(_currentAnim)) animator.ResetTrigger(_currentAnim);
                _currentAnim = animName;
                animator.SetTrigger(_currentAnim);
            }
        }

        private Transform GetNearestEnemy()
        {
            if (hits.Length == 0) return null;
            var nearest = hits.OrderBy(e => Vector3.Distance(throwPos.position, e.transform.position)).First();
            return nearest.transform;
        }

        private void OnThrow()
        {
            Debug.Log("Attack");
            if (target == null) return;
            if (throwItemPrefab != null) Debug.Log("throwItemPrefab");
            var item = SimplePool.Spawn<ThrowItem>(throwItemPrefab, throwPos.position, Quaternion.identity);
            //Instantiate(throwItemPrefab, throwPos.position, Quaternion.identity);
            _listThrowItems.Enqueue(item);

            var rb = item.GetComponent<Rigidbody>();
            if (rb != null) rb.velocity = skin.forward * 10;

           // if (_hideCoroutine == null) _hideCoroutine = StartCoroutine(HideWeaponThrow());
        }

        private IEnumerator HideWeaponThrow()
        {
            while (_listThrowItems.Count > 0)
            {
                foreach (var t in _listThrowItems.ToArray())
                    if (Vector3.Distance(t.transform.position, throwRange.position) > rangeSize + 0.5f)
                        SimplePool.Despawn(t);

                yield return null;
            }

            _hideCoroutine = null;
        }
        public void OnHit()
        {
            SimplePool.Despawn(this);
        }
    }
  
}