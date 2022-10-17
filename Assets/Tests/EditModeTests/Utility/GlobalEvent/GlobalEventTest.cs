using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;

public class GlobalEventTest
{
    [Test(Description = "UnityEvents can be added and will be invoked themselves on the GlobalEvent being invoked")]
    public void GlobalEventAddListener()
    {
        var globalEvent = ScriptableObject.CreateInstance<GlobalEvent>();
        var unityEvent = new UnityEvent();

        unityEvent.AddListener(() => Assert.Pass());
        globalEvent.AddListener(unityEvent);

        globalEvent.Invoke();

        Assert.Fail();
    }

    [Test(Description = "UnityEvents can be removed again and will not be invoked on the GlobalEvent being invoked")]
    public void GlobalEventRemoveListener()
    {
        var globalEvent = ScriptableObject.CreateInstance<GlobalEvent>();
        var unityEvent = new UnityEvent();

        unityEvent.AddListener(() => Assert.Fail());
        globalEvent.AddListener(unityEvent);
        globalEvent.RemoveListener(unityEvent);

        globalEvent.Invoke();
    }
}
