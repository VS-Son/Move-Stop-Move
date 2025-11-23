using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Anim.EventAnim
{
    public enum TypeEventsAnim
    {
        Throw,
        EndThrow
    }

    public class EventsAnimManager : MonoBehaviour
    {
        private static readonly Dictionary<Animator, EventsAnimManager> EventsAnimManagers = new();
        private readonly Dictionary<TypeEventsAnim, List<Action>> _eventTable = new();
        private Animator Animator => GetComponent<Animator>();

        private void Start()
        {
            if (Animator != null && !EventsAnimManagers.ContainsKey(Animator)) EventsAnimManagers.Add(Animator, this);
        }

        public static EventsAnimManager Get(Animator animator)
        {
            if (animator != null && EventsAnimManagers.TryGetValue(animator, out var manager)) return manager;

            return null;
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