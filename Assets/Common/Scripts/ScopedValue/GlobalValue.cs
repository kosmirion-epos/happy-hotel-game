using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Encapsulates a generic value as a <see cref="ScriptableObject"/>.
/// Derive from this class for every concrete type you wish to encapsulate.
/// </summary>
/// <typeparam name="T"> Type of the encapsulated <see cref="value"/> </typeparam>
public abstract class GlobalValue<T> : ScriptableObject, ISerializationCallbackReceiver
{
    /// <summary>Whether or not the encapsulated <see cref="value"/> can be modified at runtime</summary>
    [Label("")]
#if UNITY_EDITOR
    [OnValueChanged(nameof(_onIsRuntimeConstantChanged))]
#endif
    [Dropdown("_constantMap")]
    [AllowNesting]
    [SerializeField] private bool isRuntimeConstant = true;

    public bool IsRuntimeConstant => isRuntimeConstant;

    /// <summary>The encapsulated <see cref="value"/>'s initial state.</summary>
    [OnValueChanged(nameof(_onInitialValueChanged))]
    [AllowNesting]
    [SerializeField] private T initialValue;

    public T InitialValue => initialValue;

    /// <summary> The encapsulated value </summary>
    [HideIf(nameof(_shouldHideValueInEditor))]
    [AllowNesting]
    [SerializeField] private T value;

    /// <summary> When <see cref="Value"/> is set, <see cref="UnityEvent{T}.Invoke(T)"/> is called on these </summary>
    private HashSet<UnityEvent<T>> listeners = new HashSet<UnityEvent<T>>();

    /// <summary> A readonly view of the registered <see cref="listeners"/> </summary>
    public IReadOnlyCollection<UnityEvent<T>> Listeners => listeners;

    private DropdownList<bool> _constantMap = new() { { "Constant", true }, { "Variable", false } };
#if UNITY_EDITOR
    private void _onIsRuntimeConstantChanged() { EditorUtility.SetDirty(this); _initialize(); }
#endif
    private void _onInitialValueChanged() { if (isRuntimeConstant) _initialize(); }
    private bool _shouldHideValueInEditor => isRuntimeConstant || !Application.isPlaying;

    /// <summary>
    /// Accessor for the encapsulated <see cref="value"/>.
    /// If <see cref="isRuntimeConstant"/> is true, using the setter will result in an <see cref="InvalidOperationException"/>.
    /// </summary>
    public T Value
    {
        get => value;
        set
        {
            if (isRuntimeConstant)
                throw new InvalidOperationException("Attempted to modify runtime constant.");

            this.value = value;

            foreach (var l in listeners)
                l.Invoke(Value);
        }
    }

    /// <summary> Registers a <see cref="UnityEvent{T}"/> to be invoked, when <see cref="Value"/> is set </summary>
    public void AddListener(UnityEvent<T> listener) => listeners.Add(listener);

    /// <summary> Deregisters a <see cref="UnityEvent{T}"/> so it won't be invoked, when <see cref="Value"/> is set </summary>
    public void RemoveListener(UnityEvent<T> listener) => listeners.Remove(listener);

    /// <summary> Produces a <typeparamref name="T"/> with the same field values as the supplied <typeparamref name="T"/>. </summary>
    protected abstract T GetEquivalentInstance(T t);

    /// <summary> (Re)sets the encapsulated <see cref="value"/> to the <see cref="InitialValue"/> </summary>
    private void _initialize() => value = GetEquivalentInstance(initialValue);

    /// <summary> Empty implementation as required by <see cref="ISerializationCallbackReceiver"/> </summary>
    public void OnBeforeSerialize() { }

    /// <summary> Calls <see cref="_initialize"/> </summary>
    public void OnAfterDeserialize() => _initialize();
}
