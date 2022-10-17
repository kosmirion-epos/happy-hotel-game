using UnityEngine;
using UnityEngine.Events;

public class GlobalValueDebugListener<T> : MonoBehaviour
{
    [SerializeField] private GlobalValue<T> globalValue;

    private UnityEvent<T> debugEvent;

    private void OnEnable()
    {
        debugEvent = new();
        debugEvent.AddListener((t) => Debug.Log(globalValue.name + " invoked!"));
        globalValue.AddListener(debugEvent);
    }

    private void OnDisable()
    {
        globalValue.RemoveListener(debugEvent);
    }
}
