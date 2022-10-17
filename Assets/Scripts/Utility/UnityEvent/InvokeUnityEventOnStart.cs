using UnityEngine;
using UnityEngine.Events;

public class InvokeUnityEventOnStart : MonoBehaviour
{
    [SerializeField] private UnityEvent startEvent;
    private void Start()
    {
        startEvent.Invoke();
    }
}
