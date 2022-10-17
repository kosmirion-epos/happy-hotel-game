using NaughtyAttributes;
using UnityEngine;

public class InvokeGlobalEventOnTriggerExit : MonoBehaviour
{
    [Tag][SerializeField] private string leavingTag;
    [SerializeField] private GlobalEvent triggeredEvent;

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag(leavingTag))
            triggeredEvent?.Invoke();
    }
}
