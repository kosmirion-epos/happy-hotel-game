using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

/// <summary>
/// Aggregates <see cref="UnityEvent"/>s, which can be invoked by calling <see cref="Invoke"/>.
/// </summary>
[CreateAssetMenu(menuName = "Global Event")]
public class GlobalEvent : ScriptableObject
{
    /// <summary> The registered listeners </summary>
    private HashSet<UnityEvent> listeners = new();

    /// <summary> A readonly view of the registered <see cref="listeners"/> </summary>
    public IReadOnlyCollection<UnityEvent> Listeners => listeners;

    /// <summary>
    /// Registers a <see cref="UnityEvent"/> to be invoked,
    /// when <see cref="Invoke"/> is called
    /// </summary>
    public void AddListener(UnityEvent e) => listeners.Add(e);

    /// <summary>
    /// Deregisters a <see cref="UnityEvent"/> so it won't be invoked,
    /// when <see cref="Invoke"/> is called
    /// </summary>
    public void RemoveListener(UnityEvent e) => listeners.Remove(e);

    /// <summary> Invokes all registered <see cref="listeners"/> </summary>
    [Button(enabledMode: EButtonEnableMode.Playmode)]
    public void Invoke()
    {
        foreach (var e in new HashSet<UnityEvent>(listeners))
            e.Invoke();
    }
}
