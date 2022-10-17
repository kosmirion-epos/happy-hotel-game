using NaughtyAttributes;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(menuName = "Visual Effect Config")]
public class VisualEffectConfig : ScriptableObject
{
    [SerializeField] private VisualEffectAsset asset;
    [SerializeField] private bool useLifespan = true;
    [ShowIf("useLifespan")][SerializeField] private ScopedValue<float> lifespan;
    [SerializeField] private bool useDestroyDelay = false;
    [ShowIf("useDestroyDelay")][SerializeField] private ScopedValue<float> destroyDelay;

    /// <summary>
    /// <see cref="lifespan"/>'s <see cref="ScopedValue{T}.Value"/>
    /// if <see cref="useLifespan"/>, otherwise <c>-1</c>
    /// </summary>
    public float Lifespan => useLifespan ? lifespan.Value : -1;

    /// <summary>
    /// <see cref="destroyDelay"/>'s <see cref="ScopedValue{T}.Value"/>
    /// if <see cref="useDestroyDelay"/>, otherwise <c>-1</c>
    /// </summary>
    public float DestroyDelay => useDestroyDelay ? destroyDelay.Value : -1;

    public void Destroy(VisualEffect effect)
    {
        if (useDestroyDelay)
        {
            effect.Stop();
            Destroy(effect.gameObject, destroyDelay.Value);
        }
        else
        {
            Destroy(effect.gameObject);
        }
    }

    public VisualEffect Spawn()
    {
        GameObject obj = new();

        obj.name = name;

        var effect = obj.AddComponent<VisualEffect>();
        effect.visualEffectAsset = asset;
        effect.Play();

        if (useLifespan)
            Destroy(obj, lifespan.Value);

        return effect;
    }

    public VisualEffect Spawn(Transform parent)
    {
        var effect = Spawn();

        effect.transform.parent = parent;
        effect.transform.localPosition = Vector3.zero;
        effect.transform.localRotation = Quaternion.identity;
        effect.transform.localScale = Vector3.one;

        return effect;
    }

    public VisualEffect Spawn(Transform parent, bool instantiateInWorldSpace)
    {
        if (!instantiateInWorldSpace)
            return Spawn(parent);

        var effect = Spawn();

        effect.transform.parent = parent;

        return effect;
    }

    public VisualEffect Spawn(Vector3 position, Quaternion rotation)
    {
        var effect = Spawn();

        effect.transform.position = position;
        effect.transform.rotation = rotation;

        return effect;
    }

    public VisualEffect Spawn(Vector3 position, Quaternion rotation, Transform parent)
    {
        var effect = Spawn(position, rotation);

        effect.transform.parent = parent;

        return effect;
    }
}