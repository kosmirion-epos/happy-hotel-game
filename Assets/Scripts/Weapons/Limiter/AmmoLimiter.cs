using UnityEngine;

public class AmmoLimiter : ActionLimiter
{
    [SerializeField] private Ammo ammo;

    protected override bool LimitsAction() => !ammo.HasBullet();
}
