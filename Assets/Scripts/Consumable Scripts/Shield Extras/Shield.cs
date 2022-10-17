using UnityEngine;
using UnityEngine.Events;

public class Shield : MonoBehaviour
{
    [SerializeField] private VisualEffectConfig hitVFX;

    public UnityEvent ShieldHit;

    private void OnTriggerEnter(Collider other)
    {
        hitVFX.Spawn(other.transform.position, Quaternion.LookRotation(transform.forward, Vector3.up));
        ShieldHit.Invoke();
    }
}
