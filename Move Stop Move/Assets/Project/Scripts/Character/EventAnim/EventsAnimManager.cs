using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeEventsAnim
{
    Throw,
    EndThrow
}
public class EventsAnimManager : MonoBehaviour
{
    private static readonly Dictionary<Animator, EventsAnimManager> s_EventsAnimManagers = new();
    private  readonly Dictionary<TypeEventsAnim, List<Action>> _eventTable = new();
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator != null && !s_EventsAnimManagers.ContainsKey(_animator))
        {
            s_EventsAnimManagers.Add(_animator, this);
        }
    }

    public static EventsAnimManager Get(Animator animator)
    {
        if (animator != null && s_EventsAnimManagers.TryGetValue(animator, out var manager))
        {
            return manager;
        }

        return null;
    }
    public void OnRegister(TypeEventsAnim eventType,  Action callback)
    {
        if (!_eventTable.ContainsKey(eventType))
        {
            _eventTable[eventType] = new List<Action>();
        }
        _eventTable[eventType].Add(callback);
    }

    public void OnAnimEvent(TypeEventsAnim eventType)
    {
        if (_eventTable.TryGetValue(eventType,out var actions))
        {
            foreach (var action in actions)
            {
                action?.Invoke();
            }
            
        }
    }
}
