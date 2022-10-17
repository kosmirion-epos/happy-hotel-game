using UnityEngine;

public class ChargeLimiter : ActionLimiter
{
    [SerializeField] private Charge charge;

    protected override bool LimitsAction() => !charge.Charging;
}
