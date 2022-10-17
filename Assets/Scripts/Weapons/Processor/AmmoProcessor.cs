using UnityEngine;

public class AmmoProcessor : ActionProcessor
{
    [SerializeField] private Ammo ammo;

    protected override void HandleAction() => ammo.RemoveBullet();
}
