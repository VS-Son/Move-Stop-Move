using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Anim.EventAnim
{
    public enum TypeEventsAnim
    {
        Throw,
        EndThrow,
        EndDead
    }

    public class EventsAnimManager : MonoBehaviour
    {
        private static readonly Dictionary<Animator, EventsAnimManager> SEventsAnimManagers = new();
        private readonly Dictionary<TypeEventsAnim, List<Action>> _eventTable = new();
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            if (_animator != null && !SEventsAnimManagers.ContainsKey(_animator))
                SEventsAnimManagers.Add(_animator, this);
        }

        public void OnRegister(TypeEventsAnim eventType, Action callback)
        {
            if (!_eventTable.ContainsKey(eventType)) _eventTable[eventType] = new List<Action>();
            _eventTable[eventType].Add(callback);
        }

        public void OnAnimEvent(TypeEventsAnim eventType)
        {
            if (_eventTable.TryGetValue(eventType, out var actions))
                foreach (var action in actions)
                    action?.Invoke();
        }
    }
}