using UnityEngine;

public class FireRateProcessor : ActionProcessor
{
    [SerializeField] private LastShotTime lastShotTime;

    protected override void HandleAction() => lastShotTime.UpdateTime();
}
