using System;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Anim.EventAnim;
using Project.Scripts.Character.Attack;
using Project.Scripts.Level;
using Project.Scripts.Pool;
using UnityEngine;
using UnityEngine.AI;

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

        public NavMeshAgent agent;

        private readonly Queue<ThrowItem> _listThrowItems = new();
        private string _currentAnim;

        private Collider[] Hits => Physics.OverlapSphere(throwRange.position, RangeSize, LayerMask.GetMask("Neutral"))
            .Where(col => col.transform != transform).ToArray();

        protected Transform Target => GetNearestEnemy();

        protected float RangeSize
        {
            get => throwRangeSize;
            set
            {
                throwRangeSize = value;
                UpdateThrowRange();
            }
        }


        private void Start()
        {
            OnInit();
            UpdateThrowRange();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(throwRange.position, RangeSize);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateThrowRange();
        }

#endif
        public static event Action<Character> SetNumberAlive;

        protected virtual void OnInit()
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
            throwRange.localScale = new Vector3(RangeSize * 2, RangeSize * 2, 1f);
        }

        public bool HasEnemyInRange()
        {
            return Hits.Length > 0;
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

        public Transform GetNearestEnemy()
        {
            if (Hits.Length == 0) return null;
            var nearest = Hits.OrderBy(e => Vector3.Distance(throwRange.position, e.transform.position)).First();
            return nearest.transform;
        }

        private void OnThrow()
        {
            if (Target == null) return;
            var throwPosition = throwPos.position;
            var item = SimplePool.Spawn<ThrowItem>(throwItemPrefab, throwPosition, Quaternion.identity);
            _listThrowItems.Enqueue(item);
            var dir = (Target.position - throwPosition).normalized;
            var rb = item.GetComponent<Rigidbody>();
            if (rb != null) rb.velocity = dir * 10f;
            rb.freezeRotation = true;
        }


        public virtual void OnHit()
        {
            SetNumberAlive?.Invoke(this);
            SimplePool.Despawn(this);
            RandomNavMeshSpawner.Instance.RemoveBot(this);
        }
    }
}