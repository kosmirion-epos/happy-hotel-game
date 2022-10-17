using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalEventMultiListener : ExtendedBehaviour
{
    [SerializeField] private ScopedValue<List<GlobalEvent>> globalEvents;
    [SerializeField] private UnityEvent onTrigger;
    [SerializeField] private ScopedValue<float> delay;

    private UnityEvent registered;

    private void OnEnable() => Register();

    private void OnDisable() => Deregister();

    public void Register()
    {
        if (onTrigger == null)
            return;

        if (delay.Value <= 0)
        {
            registered = onTrigger;
        }
        else
        {
            registered = new();
            registered.AddListener(() => WithDelay(delay.Value, onTrigger.Invoke));
        }

        foreach (var g in globalEvents.Value)
            g?.AddListener(registered);
    }

    public void Deregister()
    {
        if (registered == null)
            return;

        foreach (var g in globalEvents.Value)
            g?.RemoveListener(registered);
    }
}
