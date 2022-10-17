using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TestTools;

public class GlobalEventListenerTest
{
    /// <summary>
    /// A <see cref="GlobalEventListener"/> automatically <see cref="GlobalEventListener.Register"/>s
    /// <see cref="GlobalEventListener.OnTrigger"/> with <see cref="GlobalEventListener.GlobalEvent"/>
    /// </summary>
    [UnityTest]
    public IEnumerator GlobalEventListenerRegister()
    {
        var root = Object.Instantiate(new GameObject());
        var listener = root.AddComponent<GlobalEventListener>();

        listener.enabled = false;

        listener.GlobalEvent = ScriptableObject.CreateInstance<GlobalEvent>();
        listener.OnTrigger = new UnityEvent();
        listener.OnTrigger.AddListener(() => Assert.Pass());

        listener.enabled = true;

        listener.GlobalEvent.Invoke();

        Assert.Fail();
        yield return null;
    }

    /// <summary>
    /// A <see cref="GlobalEventListener"/> automatically <see cref="GlobalEventListener.Deregister"/>s
    /// <see cref="GlobalEventListener.OnTrigger"/> from <see cref="GlobalEventListener.GlobalEvent"/>
    /// </summary>
    [UnityTest]
    public IEnumerator GlobalEventListenerDeregister()
    {
        var root = Object.Instantiate(new GameObject());
        var listener = root.AddComponent<GlobalEventListener>();

        listener.GlobalEvent = ScriptableObject.CreateInstance<GlobalEvent>();
        listener.OnTrigger = new UnityEvent();
        listener.OnTrigger.AddListener(() => Assert.Fail());

        listener.enabled = false;

        listener.GlobalEvent.Invoke();
        yield return null;
    }
}
