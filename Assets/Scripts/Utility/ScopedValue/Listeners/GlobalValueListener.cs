using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// (De-)registers <see cref="UnityEvent{T}"/>s to a <see cref="GlobalValue"/> when
/// <see cref="OnEnable"/> and <see cref="OnDisable"/> are called respectively.
/// </summary>
public class GlobalValueListener<T> : ExtendedBehaviour
{
    /// <summary> The <see cref="GlobalValue{T}"/> to register <see cref="onChanged"/> for </summary>
    [SerializeField] private GlobalValue<T> globalValue;

    /// <summary>
    /// Accessor for <see cref="globalValue"/>. Using the setter will <see cref="Deregister"/>
    /// from the old <see cref="globalValue"/> and <see cref="Register"/> to the new one./>
    /// </summary>
    public GlobalValue<T> GlobalValue
    {
        get => globalValue;
        set
        {
            if (isActiveAndEnabled)
            {
                Deregister();
                globalValue = value;
                Register();
            }
            else
            {
                globalValue = value;
            }
        }
    }

    /// <summary> The <see cref="UnityEvent{T}"/> to be registered for <see cref="globalValue"/> </summary>
    [SerializeField] private UnityEvent<T> onChanged;

    /// <summary>
    /// Accessor for <see cref="onChanged"/>. Using the setter will <see cref="Deregister"/>
    /// the old <see cref="onChanged"/> and <see cref="Register"/> the new one./>
    /// </summary>
    public UnityEvent<T> OnChanged
    {
        get => onChanged;
        set
        {
            if (isActiveAndEnabled)
            {
                Deregister();
                onChanged = value;
                Register();
            }
            else
            {
                onChanged = value;
            }
        }
    }

    /// <summary>
    /// The delay from <see cref="globalValue"/> firing to <see cref="onChanged"/> being called.
    /// Modifying this field through the inspector at runtime will not update the registered
    /// event and may thus cause bugs.
    /// </summary>
    [SerializeField] private ScopedValue<float> delay;

    /// <summary>
    /// Accessor for <see cref="onChanged"/>. Using the setter will <see cref="Deregister"/>
    /// the old <see cref="onChanged"/> and <see cref="Register"/> the new one./>
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

    private UnityEvent<T> registered;

    /// <summary>
    /// Registers <see cref="onChanged"/> for <see cref="globalValue"/> with the
    /// given <see cref="delay"/>
    /// </summary>
    public void Register()
    {
        if (onChanged == null)
            return;

        if (delay.Value <= 0)
        {
            registered = onChanged;
        }
        else
        {
            registered = new();
            registered.AddListener((t) => WithDelay(delay.Value, () => onChanged.Invoke(t)));
        }

        globalValue?.AddListener(registered);
    }

    /// <summary> Deregisters <see cref="onChanged"/> for <see cref="globalValue"/> </summary>
    public void Deregister() => globalValue?.RemoveListener(registered);

    /// <summary> Automatically registers <see cref="onChanged"/> for <see cref="globalValue"/> </summary>
    private void OnEnable() => Register();

    /// <summary> Automatically deregisters <see cref="onChanged"/> for <see cref="globalValue"/> </summary>
    private void OnDisable() => Deregister();
}
