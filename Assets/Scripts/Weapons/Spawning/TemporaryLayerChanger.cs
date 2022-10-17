using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryLayerChanger : ExtendedBehaviour
{
    [SerializeField] private List<GameObject> gameObjects;
    [SerializeField] private ScopedValue<float> restoreDelay;
    [Layer][SerializeField] private int layer;

    public void ChangeLayer()
    {
        foreach (var o in gameObjects)
        {
            var rememberedLayer = o.layer;

            o.layer = layer;

            WithDelay(
                restoreDelay.Value,
                () =>
                {
                    if (o.layer == layer)
                        o.layer = rememberedLayer;
                }
            );
        }
    }
}
