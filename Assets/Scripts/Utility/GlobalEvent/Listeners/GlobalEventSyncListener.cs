using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GlobalEventSyncListener : ExtendedBehaviour
{
    [Serializable]
    private class EventCondition
    {
        public GlobalEvent GlobalEvent;
        public bool startSatisfied;
    }

    [Foldout("Events")][SerializeField] private ScopedValue<List<EventCondition>> globalEvents;
    [HorizontalLine]
    [Foldout("Events")][SerializeField] private UnityEvent onTrigger;

    [Foldout("Additional Settings")][SerializeField] private ScopedValue<float> delay;
    [Foldout("Additional Settings")][SerializeField] private ScopedValue<bool> resetOnRepeatedCondition;

    private Dictionary<GlobalEvent, UnityEvent> eventMap;
    private HashSet<GlobalEvent> calledConditions;

    private void Awake()
    {
        eventMap = new();
    }

    private void OnEnable()
    {
        calledConditions = new(globalEvents.Value.Where((e) => !e.startSatisfied).Select((e) => e.GlobalEvent));

        foreach (var c in globalEvents.Value)
            _addCondition(c.GlobalEvent);
    }

    private void OnDisable()
    {
        foreach (var c in globalEvents.Value)
            _removeCondition(c.GlobalEvent);
    }

    private void _onCondition(GlobalEvent e)
    {
        if (resetOnRepeatedCondition.Value && !calledConditions.Contains(e))
        {
            ResetConditions();
        }
        else
        {
            calledConditions.Remove(e);

            if (!calledConditions.Any())
            {
                if (delay.Value > 0)
                {
                    WithDelay(
                        delay.Value,
                        () =>
                        {
                            onTrigger.Invoke();
                            ResetConditions();
                        }
                    );
                }
                else
                {
                    onTrigger.Invoke();
                    ResetConditions();
                }
            }
        }
    }

    private void _addCondition(GlobalEvent e)
    {
        UnityEvent u = new();
        u.AddListener(() => _onCondition(e));
        e.AddListener(u);
        eventMap[e] = u;
    }

    private void _removeCondition(GlobalEvent e)
    {
        e.RemoveListener(eventMap[e]);
        eventMap.Remove(e);
    }

    public void ResetConditions()
    {
        HashSet<GlobalEvent> conditionSet = new(globalEvents.Value.Select((e) => e.GlobalEvent));
        var missing = conditionSet.Except(eventMap.Keys);
        var excess = eventMap.Keys.Except(conditionSet);

        foreach (var e in excess)
            _removeCondition(e);

        foreach (var m in missing)
            _addCondition(m);

        calledConditions = conditionSet;
    }
}
