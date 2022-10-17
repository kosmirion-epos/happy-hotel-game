using Autohand;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Decides, whether or not <see cref="Weapon.TryFire"/> should be called, when a
/// <see cref="WeaponInteractable{T}"/> is triggered
/// </summary>
public abstract class WeaponInteractable<T> : ExtendedBehaviour where T : Weapon
{
    [Tooltip("The weapon to fire - leave empty to fetch reference automatically.")]
    [SerializeField] protected T weapon;

    /// <summary>
    /// Whether the callbacks of this class should be automatically hooked up
    /// to the events of the attached <see cref="Grabbable"/>
    /// </summary>
    [Tooltip("Whether to automatically hook into Grabbable events")]
    [SerializeField] private bool shouldSetUpDefaultEvents = true;



    [SerializeField] private List<GrabbableEventFilter> eventFilters;

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    protected void TryFire() => weapon.TryFire();

    protected void Awake()
    {
        if (!weapon)
            weapon = GetComponent<T>();

        if (shouldSetUpDefaultEvents)
            _setUpDefaultEvents();
    }

    private void _setUpDefaultEvents()
    {
        var grabbable = GetComponent<Grabbable>();

        grabbable.OnGrabEvent += OnGrabFiltered;
        grabbable.OnReleaseEvent += OnReleaseFiltered;
        grabbable.OnSqueezeEvent += OnSqueezeFiltered;
        grabbable.OnUnsqueezeEvent += OnUnsqueezeFiltered;
        grabbable.OnHighlightEvent += OnHighlightFiltered;
        grabbable.OnUnhighlightEvent += OnUnhighlightFiltered;
        grabbable.OnJointBreakEvent += OnForceReleaseFiltered;
    }

    public virtual void OnGrab(Hand hand, Grabbable grabbable) { }
    public virtual void OnRelease(Hand hand, Grabbable grabbable) { }
    public virtual void OnSqueeze(Hand hand, Grabbable grabbable) { }
    public virtual void OnUnsqueeze(Hand hand, Grabbable grabbable) { }
    public virtual void OnHighlight(Hand hand, Grabbable grabbable) { }
    public virtual void OnUnhighlight(Hand hand, Grabbable grabbable) { }
    public virtual void OnForceRelease(Hand hand, Grabbable grabbable) { }

    public void OnGrabFiltered(Hand hand, Grabbable grabbable)
    {
        if (!GrabbableEventFilter.AnyRejectsGrab(hand, grabbable, eventFilters))
            OnGrab(hand, grabbable);
    }

    public void OnReleaseFiltered(Hand hand, Grabbable grabbable)
    {
        if (!GrabbableEventFilter.AnyRejectsRelease(hand, grabbable, eventFilters))
            OnRelease(hand, grabbable);
    }

    public void OnSqueezeFiltered(Hand hand, Grabbable grabbable)
    {
        if (!GrabbableEventFilter.AnyRejectsSqueeze(hand, grabbable, eventFilters))
            OnSqueeze(hand, grabbable);
    }

    public void OnUnsqueezeFiltered(Hand hand, Grabbable grabbable)
    {
        if (!GrabbableEventFilter.AnyRejectsUnsqueeze(hand, grabbable, eventFilters))
            OnUnsqueeze(hand, grabbable);
    }

    public void OnHighlightFiltered(Hand hand, Grabbable grabbable)
    {
        if (!GrabbableEventFilter.AnyRejectsHighlight(hand, grabbable, eventFilters))
            OnHighlight(hand, grabbable);
    }

    public void OnUnhighlightFiltered(Hand hand, Grabbable grabbable)
    {
        if (!GrabbableEventFilter.AnyRejectsUnhighlight(hand, grabbable, eventFilters))
            OnUnhighlight(hand, grabbable);
    }

    public void OnForceReleaseFiltered(Hand hand, Grabbable grabbable)
    {
        if (!GrabbableEventFilter.AnyRejectsJointBreak(hand, grabbable, eventFilters))
            OnForceRelease(hand, grabbable);
    }
}
