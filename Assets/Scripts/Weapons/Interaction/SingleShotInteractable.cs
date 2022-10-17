using Autohand;

public class SingleShotInteractable : WeaponInteractable<Weapon>
{
    public override void OnSqueeze(Hand hand, Grabbable grabbable)
    {
        TryFire();
    }
}
