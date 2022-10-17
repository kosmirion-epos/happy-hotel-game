using NaughtyAttributes;
using UnityEngine;

public class InvokeGlobalEventOnTriggerEnter : MonoBehaviour
{
    [Tag][SerializeField] private string enteringTag;
    [SerializeField] private GlobalEvent triggeredEvent;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(enteringTag))
            triggeredEvent?.Invoke();
    }
}
