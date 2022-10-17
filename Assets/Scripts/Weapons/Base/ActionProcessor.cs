using UnityEngine;

public abstract class ActionProcessor : ExtendedBehaviour
{
    [Tooltip("If true, this limiter does nothing")]
    [SerializeField] private ScopedValue<bool> circumvented;

    protected abstract void HandleAction();

    public void ProcessAction()
    {
        if (!circumvented.Value)
            HandleAction();
    }
}