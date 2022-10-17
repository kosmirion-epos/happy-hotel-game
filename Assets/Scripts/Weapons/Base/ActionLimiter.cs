using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Allows modular limiting of weapon fire - e.g. Ammo, Energy, Overheat
/// </summary>
public abstract class ActionLimiter : ExtendedBehaviour
{
    [Tooltip("If true, this limiter does nothing")]
    [SerializeField] private ScopedValue<bool> circumvented;
    [Foldout("Events")][SerializeField] private UnityEvent onActionLimited;

    public event UnityAction OnActionLimitedEvent;

    protected abstract bool LimitsAction();

    public bool AllowsAction()
    {
        if (circumvented.Value || !LimitsAction())
            return true;

        onActionLimited.Invoke();
        OnActionLimitedEvent?.Invoke();

        return false;
    }
}