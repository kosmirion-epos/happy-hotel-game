using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class ChargeableWeapon : Weapon
{
    [Foldout("Limiters")][SerializeField] private List<ActionLimiter> chargeLimiters;
    [Foldout("Processors")][SerializeField] private List<ActionProcessor> chargeProcessors;
    [Foldout("General")][Required][SerializeField] private Charge charge;
    [Foldout("General")][Required][SerializeField] private Weapon defaultWeapon;
    [Foldout("General")][Required][SerializeField] private Weapon chargedWeapon;

    public void TryCharge()
    {
        if (chargeLimiters.Any(l => !l.AllowsAction()))
            return;

        foreach (var p in chargeProcessors)
            p.ProcessAction();
    }

    protected override void Fire()
    {
        if (charge.FullyCharged)
            chargedWeapon.TryFire();
        else
            defaultWeapon.TryFire();
    }
}
