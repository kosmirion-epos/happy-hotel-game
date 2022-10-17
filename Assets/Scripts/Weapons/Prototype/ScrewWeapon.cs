using Autohand;
using UnityEngine;

public class ScrewWeapon : AmmoWeapon
{
    [SerializeField] private PlacePoint placePoint;
    [SerializeField] private ScrewThread screwThread;
    [SerializeField] private string depletedIdentifier = "Depleted";

    protected override void UpdateBulletDisplay() { }

    protected override void OnBulletsDepleted()
    {
        Grabbable grabbable = placePoint.GetPlacedObject();

        grabbable.enabled = true;
        grabbable.gameObject.name = grabbable.gameObject.name + " (" + depletedIdentifier + ")";
    }
}
