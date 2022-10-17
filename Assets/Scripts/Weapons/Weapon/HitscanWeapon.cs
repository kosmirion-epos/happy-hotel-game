using NaughtyAttributes;
using UnityEngine;

public class HitscanWeapon : Weapon
{
    [SerializeField][Foldout("Hit Detection")][Tag] protected string enemyBodyTag;
    [SerializeField][Foldout("Hit Detection")][Tag] protected string enemyHeadTag;
    [SerializeField][Foldout("Hit Detection")][Tag] protected string destructibleTag;
    [SerializeField][Foldout("Hit Detection")] private LayerMask raycastMask;
    [SerializeField][Foldout("Physics")] private ScopedValue<float> impactForce;
    [SerializeField][Foldout("Physics")] private ScopedValue<Vector3> recoilForce;
    [SerializeField][Foldout("Physics")][Required] private Transform bulletEject;
    [SerializeField][Foldout("Sound")] private GlobalFMODSoundManager sm;
    [SerializeField][Foldout("Sound")] private int impactSound;
    [SerializeField][Foldout("VFX")] private VisualEffectConfig fireVFX;
    [SerializeField][Foldout("VFX")] private VisualEffectConfig impactVFX;
    [SerializeField][Foldout("VFX")] private ScopedValue<bool> impactVFXRicochet;
    [SerializeField][Foldout("VFX")] private VisualEffectConfig trailVFX;
    [SerializeField][Foldout("VFX")] private Transform vfxOrigin;

    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    protected override void Fire()
    {
        if (fireVFX)
            fireVFX.Spawn(vfxOrigin.position, vfxOrigin.rotation);

        if (Physics.Raycast(bulletEject.position, bulletEject.forward, out RaycastHit hit, float.PositiveInfinity, raycastMask))
        {
            var hitBody = hit.transform.GetComponent<Rigidbody>();

            if (trailVFX)
                trailVFX.Spawn(
                    vfxOrigin.position,
                    vfxOrigin.rotation
                ).SetFloat("ShotDistance", Vector3.Distance(hit.point, vfxOrigin.position));

            GameObject vfxObj = null;

            if (impactVFX)
                vfxObj = impactVFX.Spawn(
                    hit.point,
                    Quaternion.LookRotation(
                        impactVFXRicochet.Value
                            ? Vector3.Reflect(bulletEject.forward, hit.normal)
                            : hit.normal,
                        Vector3.up
                    )
                ).gameObject;

            if (vfxObj && sm)
                sm.Value.PlaySound(impactSound, vfxObj);

            if (hitBody)
                hitBody.AddForceAtPosition(impactForce.Value * (hit.point - bulletEject.position).normalized, hit.point, ForceMode.Impulse);

            if (hit.transform.CompareTag(enemyHeadTag))
                hit.transform.GetComponentInParent<IShootable>()?.HitHead(hit.point);
            else if (hit.transform.CompareTag(enemyBodyTag))
                hit.transform.GetComponentInParent<IShootable>()?.Hit(hit.point);
            else if (hit.transform.CompareTag(destructibleTag))
                hit.transform.GetComponent<Destructible>()?.Destroy();

            Debug.DrawRay(bulletEject.position, hit.point - bulletEject.position, Color.green, 3);
        }
        else
        {
            if (trailVFX)
                trailVFX.Spawn(
                    vfxOrigin.position,
                    Quaternion.LookRotation(bulletEject.forward, vfxOrigin.up)
                ).SetFloat("ShotDistance", 100);
            Debug.DrawRay(bulletEject.position, bulletEject.forward * 1000, Color.red, 1);
        }

        rigidbody.AddForce(transform.TransformDirection(recoilForce.Value), ForceMode.Impulse);
    }
}
