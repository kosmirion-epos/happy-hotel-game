using Autohand;
using NaughtyAttributes;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    [Foldout("Prefabs")][Required][SerializeField] private Projectile projectilePrefab;
    [Foldout("Physics")][Required][SerializeField] private Transform projectileEject;
    [Foldout("Physics")][SerializeField] private ScopedValue<float> ejectForce;
    [Foldout("Physics")][SerializeField] private ScopedValue<Vector3> recoilForce;
    [Foldout("Sound")][SerializeField] private Transform soundOrigin;
    [Foldout("Sound")][SerializeField] private AudioClip shootSound;
    [Foldout("Sound")][SerializeField] private ScopedValue<float> shootVolume;
    [Foldout("VFX")][SerializeField] private Transform vfxOrigin;
    [Foldout("VFX")][SerializeField] private VisualEffectConfig shootVFX;

    private Grabbable grabbable;
    private new Rigidbody rigidbody;

    private void Awake()
    {
        grabbable = GetComponent<Grabbable>();
        rigidbody = GetComponent<Rigidbody>();
    }

    protected override void Fire()
    {
        if (soundOrigin && shootSound)
            AudioSource.PlayClipAtPoint(shootSound, soundOrigin.position, shootVolume.Value);

        if (vfxOrigin && shootVFX)
            shootVFX.Spawn(vfxOrigin.position, vfxOrigin.rotation);

        var projectile = Instantiate(projectilePrefab, projectileEject.position, projectileEject.rotation);

        projectile.IgnoreObject(transform);

        foreach (var c in grabbable.GrabChildren)
            projectile.IgnoreObject(c.transform);

        projectile.GetComponent<Rigidbody>().AddForce(ejectForce.Value * projectileEject.forward, ForceMode.Impulse);

        rigidbody.AddForce(transform.TransformDirection(recoilForce.Value), ForceMode.Impulse);
    }
}
