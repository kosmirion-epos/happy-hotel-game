using System;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class GlobalValueTest
{
    [Test(Description =
        "A GlobalValue initially has isRuntimeConstant set to 0.\n" +
        "InitialValue and Value are default initialized.")]
    public void GlobalValueDefaults()
    {
        GlobalValue<float> globalValue = ScriptableObject.CreateInstance<GlobalFloat>();

        Assert.AreEqual(false, globalValue.IsRuntimeConstant);
        Assert.AreEqual(0, globalValue.InitialValue);
        Assert.AreEqual(0, globalValue.Value);

        UnityEngine.Object.DestroyImmediate(globalValue);
    }

    [Test(Description = "A GlobalValue's encapsulated value is freely accessible while isRuntimeConstant is false.")]
    public void GlobalValueVariable()
    {
        GlobalValue<float> globalValue = ScriptableObject.CreateInstance<GlobalFloat>();

        Assert.DoesNotThrow(() => globalValue.Value = 1);
        Assert.AreEqual(1, globalValue.Value);

        UnityEngine.Object.DestroyImmediate(globalValue);
    }

    [Test(Description = "A GlobalValue's encapsulated value cannot be set while isRuntimeConstant is true.")]
    public void GlobalValueConstant()
    {
        GlobalValue<float> globalValue = ScriptableObject.CreateInstance<GlobalFloat>();
        var serializedGlobal = new SerializedObject(globalValue);

        serializedGlobal.FindProperty("isRuntimeConstant").boolValue = true;
        serializedGlobal.ApplyModifiedProperties();

        Assert.Throws<InvalidOperationException>(() => globalValue.Value = 1);
        Assert.AreEqual(0, globalValue.Value);

        UnityEngine.Object.DestroyImmediate(globalValue);
    }
}