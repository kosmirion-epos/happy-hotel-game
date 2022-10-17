using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// (De-)registers <see cref="UnityEvent"/>s to a <see cref="GlobalEvent"/> when
/// <see cref="OnEnable"/> and <see cref="OnDisable"/> are called respectively.
/// </summary>
public class GlobalEventListener : ExtendedBehaviour
{
    /// <summary> The <see cref="GlobalEvent"/> to register <see cref="onTrigger"/> for </summary>
    [SerializeField] private GlobalEvent globalEvent;

    /// <summary>
    /// Accessor for <see cref="globalEvent"/>. Using the setter will <see cref="Deregister"/>
    /// from the old <see cref="globalEvent"/> and <see cref="Register"/> to the new one./>
    /// </summary>
    public GlobalEvent GlobalEvent
    {
        get => globalEvent;
        set
        {
            if (isActiveAndEnabled)
            {
                Deregister();
                globalEvent = value;
                Register();
            }
            else
            {
                globalEvent = value;
            }
        }
    }

    /// <summary> The <see cref="UnityEvent"/> to be registered for <see cref="globalEvent"/> </summary>
    [SerializeField] private UnityEvent onTrigger;

    /// <summary>
    /// Accessor for <see cref="onTrigger"/>. Using the setter will <see cref="Deregister"/>
    /// the old <see cref="onTrigger"/> and <see cref="Register"/> the new one./>
    /// </summary>
    public UnityEvent OnTrigger
    {
        get => onTrigger;
        set
        {
            if (isActiveAndEnabled)
            {
                Deregister();
                onTrigger = value;
                Register();
            }
            else
            {
                onTrigger = value;
            }
        }
    }

    /// <summary>
    /// The delay from <see cref="globalEvent"/> firing to <see cref="onTrigger"/> being called.
    /// Modifying this field through the inspector at runtime will not update the registered
    /// event and may thus cause bugs.
    /// </summary>
    [SerializeField] private ScopedValue<float> delay;

    /// <summary>
    /// Accessor for <see cref="onTrigger"/>. Using the setter will <see cref="Deregister"/>
    /// the old <see cref="onTrigger"/> and <see cref="Register"/> the new one./>
    /// </summary>
    public float Delay
    {
        get => delay.Value;
        set
        {
            if (isActiveAndEnabled)
            {
                Deregister();
                delay.Value = value;
                Register();
            }
            else
            {
                delay.Value = value;
            }
        }
    }

    private UnityEvent registered;

    /// <summary>
    /// Registers <see cref="onTrigger"/> for <see cref="globalEvent"/> with the
    /// given <see cref="delay"/>
    /// </summary>
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

        globalEvent?.AddListener(registered);
    }

    /// <summary> Deregisters <see cref="onTrigger"/> for <see cref="globalEvent"/> </summary>
    public void Deregister() => globalEvent?.RemoveListener(registered);

    /// <summary> Automatically registers <see cref="onTrigger"/> for <see cref="globalEvent"/> </summary>
    private void OnEnable() => Register();

    /// <summary> Automatically deregisters <see cref="onTrigger"/> for <see cref="globalEvent"/> </summary>
    private void OnDisable() => Deregister();
}
