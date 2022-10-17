using UnityEngine;

public class FireRateLimiter : ActionLimiter
{
    [SerializeField] private LastShotTime lastShotTime;
    [SerializeField] private ScopedValue<float> fireRate;

    protected override bool LimitsAction() => !lastShotTime.ExceedsDuration(1 / fireRate.Value);
}
