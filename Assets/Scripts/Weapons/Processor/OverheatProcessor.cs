using UnityEngine;

public class OverheatProcessor : ActionProcessor
{
    [SerializeField] private Heat heat;
    [SerializeField] private ScopedValue<float> addedHeat;

    protected override void HandleAction() => heat.AddHeat(addedHeat.Value);
}
