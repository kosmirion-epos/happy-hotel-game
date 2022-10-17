using NaughtyAttributes;
using System;
using UnityEngine;

/// <summary>
/// Encapsulates a serializable value either as is or as a <see cref="ScriptableObject"/>
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class ScopedValue<T>
{
    /// <summary>
    /// Determines, whether the encapsulated <see cref="Value"/> should be an
    /// individual only locally accessible value or a shared globally accessible value
    /// </summary>
    [SerializeField] private bool isGlobal;

    public bool IsGlobal => isGlobal;

    /// <summary>
    /// Determines whether the encapsulated value can be modified during runtime
    /// when <see cref="isGlobal"/> is <c>false</c>
    /// </summary>
    [SerializeField] private bool isRuntimeConstant = true;

    /// <summary> Whether or not the encapsulated value can be modified during runtime </summary>
    public bool IsRuntimeConstant => isGlobal ? global.IsRuntimeConstant : isRuntimeConstant;

    /// <summary> The locally accessible version of <see cref="Value"/> </summary>
    [Label("")]
    [AllowNesting]
    [SerializeField] private T local;

    /// <summary> The wrapper for the globally accessible version of <see cref="Value"/> </summary>
    [Label("")]
    [Expandable]
    [AllowNesting]
    [SerializeField] private GlobalValue<T> global;

    /// <summary>
    /// Accessor for the encapsulated value.
    /// Redirects either to <see cref="local"/> or <see cref="global"/>'s <see cref="GlobalValue{T}.value"/> depending on <see cref="isGlobal"/>.
    /// If <see cref="isRuntimeConstant"/> is <c>true</c> while <see cref="isGlobal"/> is <c>false</c>, using the setter will result in an <see cref="InvalidOperationException"/>.
    /// </summary>
    public T Value
    {
        get
        {
            if (isGlobal)
                return global.Value;
            else return local;
        }
        set
        {
            if (isGlobal)
                global.Value = value;
            else if (isRuntimeConstant)
                throw new InvalidOperationException("Attempted to modify runtime constant.");
            else
                local = value;
        }
    }

    /// <summary> Whether it's safe to access <see cref="Value"/> </summary>
    public bool IsValueAccessible => !isGlobal || global;

    //public static bool operator <(ScopedValue<T> a, ScopedValue<int> b)
    //{
    //	return (dynamic)a.Value < b.Value;
    //}

    //public static ScopedValue<int> operator <=(ScopedValue<int> a, ScopedValue<int> b)
    //	=>


    //public static bool operator >(ScopedValue<T> a, ScopedValue<int> b)
    //	=> a.Value > b.Value;

    //public static ScopedValue<int> operator >=(ScopedValue<int> a, ScopedValue<int> b)
    //	=>
}
