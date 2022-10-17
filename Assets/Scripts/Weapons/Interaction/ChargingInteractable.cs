using Autohand;
using NaughtyAttributes;

public class ChargingInteractable : WeaponInteractable<ChargeableWeapon>
{
    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void TryCharge() => weapon.TryCharge();

    public override void OnSqueeze(Hand hand, Grabbable grabbable)
    {
        TryCharge();
    }

    public override void OnUnsqueeze(Hand hand, Grabbable grabbable)
    {
        TryFire();
    }
}
