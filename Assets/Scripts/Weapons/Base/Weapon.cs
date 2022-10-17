using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Responsible for providing the <see cref="TryFire"/> Method for <see cref="WeaponInteractable"/> to call
/// </summary>
public abstract class Weapon : ExtendedBehaviour
{
    [Foldout("Limiters")][SerializeField] protected List<ActionLimiter> fireLimiters;
    [Foldout("Processors")][SerializeField] protected List<ActionProcessor> fireProcessors;
    [Foldout("Events")][SerializeField] private UnityEvent onFire;
    public event UnityAction OnFireEvent;

    protected abstract void Fire();

    public void TryFire()
    {
        if (fireLimiters.Any(l => !l.AllowsAction()))
            return;

        Fire();

        foreach (var p in fireProcessors)
            p.ProcessAction();

        onFire.Invoke();
        OnFireEvent?.Invoke();
    }
}
