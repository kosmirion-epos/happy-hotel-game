using UnityEngine;

public class Lifespan : ExtendedBehaviour
{
    [SerializeField] private ScopedValue<float> time;

    private void OnEnable() => WithDelay(time.Value, () => Destroy(gameObject));
}
