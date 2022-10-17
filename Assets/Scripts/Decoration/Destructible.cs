using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private VisualEffectConfig vfx;

    public void Destroy()
    {
        vfx.Spawn(transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnTriggerEnter()
    {
        Destroy();
    }

    private void OnCollisionEnter()
    {
        Destroy();
    }
}
