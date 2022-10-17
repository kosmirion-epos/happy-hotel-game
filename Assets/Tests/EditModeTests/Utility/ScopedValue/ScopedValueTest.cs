using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using System;

public class ScopedValueTest
{
    private class ScopedValueTestObject : ScriptableObject
    {
        public ScopedValue<float> ScopedValue = new();
    }

    [Test(Description =
        "A scoped value initially has both isGlobal and isRuntimeConstant set to false.\n" +
        "The local encapsulated value is default initialized and the global reference is null.")]
    public void ScopedValueDefaults()
    {
        var container = ScriptableObject.CreateInstance<ScopedValueTestObject>();
        var serializedContainer = new SerializedObject(container);
        var serializedScopedValue = serializedContainer.FindProperty("ScopedValue");

        Assert.AreEqual(false, serializedScopedValue.FindPropertyRelative("isGlobal").boolValue);
        Assert.AreEqual(false, serializedScopedValue.FindPropertyRelative("isRuntimeConstant").boolValue);
        Assert.AreEqual(0, serializedScopedValue.FindPropertyRelative("local").floatValue);
        Assert.AreEqual(null, serializedScopedValue.FindPropertyRelative("global").objectReferenceValue);

        serializedContainer.Dispose();
        UnityEngine.Object.DestroyImmediate(container);
    }

    [Test(Description =
        "Accessing a ScopedValue's encapsulated value should use the locally stored value\n" +
        "while isGlobal is false and isRuntimeConstant is false.")]
    [TestCase(1)]
    public void ScopedValueLocalVariable(float setTo)
    {
        var container = ScriptableObject.CreateInstance<ScopedValueTestObject>();
        var serializedContainer = new SerializedObject(container);
        var serializedScopedValue = serializedContainer.FindProperty("ScopedValue");

        Assert.DoesNotThrow(() => container.ScopedValue.Value = setTo);
        Assert.AreEqual(setTo, container.ScopedValue.Value);

        serializedContainer.Update();

        Assert.AreEqual(setTo, serializedScopedValue.FindPropertyRelative("local").floatValue);

        serializedContainer.Dispose();
        UnityEngine.Object.DestroyImmediate(container);
    }

    [Test(Description =
        "Accessing a ScopedValue's encapsulated value should throw an InvalidOperationException\n" +
        "while isGlobal is false and isRuntimeConstant is true.")]
    [TestCase(1)]
    public void ScopedValueLocalConstant(float setTo)
    {
        var container = ScriptableObject.CreateInstance<ScopedValueTestObject>();
        var serializedContainer = new SerializedObject(container);
        var serializedScopedValue = serializedContainer.FindProperty("ScopedValue");

        serializedScopedValue.FindPropertyRelative("isRuntimeConstant").boolValue = true;
        serializedContainer.ApplyModifiedProperties();

        Assert.Throws<InvalidOperationException>(() => container.ScopedValue.Value = setTo);
        Assert.AreEqual(0, container.ScopedValue.Value);

        serializedContainer.Update();

        Assert.AreEqual(0, serializedScopedValue.FindPropertyRelative("local").floatValue);

        serializedContainer.Dispose();
        UnityEngine.Object.DestroyImmediate(container);
    }

    [Test(Description =
        "Accessing a ScopedValue's encapsulated value should forward\n" +
        "to the referenced GlobalValue while isGlobal is true.")]
    [TestCase(1)]
    public void ScopedValueGlobalVariable(float setTo)
    {
        var container = ScriptableObject.CreateInstance<ScopedValueTestObject>();
        var global = ScriptableObject.CreateInstance<GlobalFloat>();
        var serializedContainer = new SerializedObject(container);
        var serializedScopedValue = serializedContainer.FindProperty("ScopedValue");

        serializedScopedValue.FindPropertyRelative("isGlobal").boolValue = true;
        serializedScopedValue.FindPropertyRelative("global").objectReferenceValue = global;
        serializedContainer.ApplyModifiedProperties();

        Assert.DoesNotThrow(() => container.ScopedValue.Value = setTo);
        Assert.AreEqual(setTo, container.ScopedValue.Value);
        Assert.AreEqual(setTo, global.Value);

        serializedContainer.Dispose();
        UnityEngine.Object.DestroyImmediate(container);
        UnityEngine.Object.DestroyImmediate(global);
    }

    [Test(Description =
        "Accessing a ScopedValue's encapsulated value should forward to the referenced GlobalValue\n" +
        "while isGlobal is true even while isRuntimeConstant is true as well.")]
    [TestCase(1)]
    public void ScopedValueGlobalConstant(float setTo)
    {
        var container = ScriptableObject.CreateInstance<ScopedValueTestObject>();
        var global = ScriptableObject.CreateInstance<GlobalFloat>();
        var serializedContainer = new SerializedObject(container);
        var serializedScopedValue = serializedContainer.FindProperty("ScopedValue");

        serializedScopedValue.FindPropertyRelative("isGlobal").boolValue = true;
        serializedScopedValue.FindPropertyRelative("isRuntimeConstant").boolValue = true;
        serializedScopedValue.FindPropertyRelative("global").objectReferenceValue = global;
        serializedContainer.ApplyModifiedProperties();

        Assert.DoesNotThrow(() => container.ScopedValue.Value = setTo);
        Assert.AreEqual(setTo, container.ScopedValue.Value);
        Assert.AreEqual(setTo, global.Value);

        serializedContainer.Dispose();
        UnityEngine.Object.DestroyImmediate(container);
        UnityEngine.Object.DestroyImmediate(global);
    }
}
