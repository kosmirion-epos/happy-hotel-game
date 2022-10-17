using UnityEngine;
using UnityEngine.Events;

public class GlobalEventPropagator : ExtendedBehaviour
{
    [SerializeField] private GlobalEvent triggeringEvent;
    [SerializeField] private GlobalEvent triggeredEvent;
    [SerializeField] private ScopedValue<float> delay;

    private UnityEvent registered;

    public void Register()
    {
        if (triggeredEvent == null)
            return;

        registered = new();
        registered.AddListener(delay.Value <= 0 ? triggeredEvent.Invoke : () => WithDelay(delay.Value, triggeredEvent.Invoke));
        triggeringEvent?.AddListener(registered);
    }

    public void Deregister() => triggeringEvent?.RemoveListener(registered);

    private void OnEnable() => Register();

    private void OnDisable() => Deregister();
}
