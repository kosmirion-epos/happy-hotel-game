using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ExtendedBehaviour : MonoBehaviour
{
    public Coroutine WithDelay(float delay, UnityAction action)
    {
        return StartCoroutine(_withDelayImpl(delay, action));
    }

    private IEnumerator _withDelayImpl(float delay, UnityAction action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    public Coroutine AtEndOfFrame(UnityAction action)
    {
        return StartCoroutine(_atEndOfFrameImpl(action));
    }

    public IEnumerator _atEndOfFrameImpl(UnityAction action)
    {
        yield return new WaitForEndOfFrame();
        action();
    }
}
