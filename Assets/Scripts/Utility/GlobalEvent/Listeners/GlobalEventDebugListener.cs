using UnityEngine;
using UnityEngine.Events;

public class GlobalEventDebugListener : MonoBehaviour
{
    [SerializeField] private GlobalEvent globalEvent;

    private UnityEvent debugEvent;

    private void OnEnable()
    {
        debugEvent = new();
        debugEvent.AddListener(() => Debug.Log(globalEvent.name + " invoked!"));
        globalEvent.AddListener(debugEvent);
    }

    private void OnDisable()
    {
        globalEvent.RemoveListener(debugEvent);
    }
}
