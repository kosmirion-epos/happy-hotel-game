using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class Projectile : ExtendedBehaviour
{
    [Foldout("Events")][SerializeField] private UnityEvent onContact;
    [Foldout("Events")][SerializeField] private UnityEvent onDestroy;
    [Foldout("Tags")][Tag][SerializeField] private string bodyTag;
    [Foldout("Tags")][Tag][SerializeField] private string headTag;
    [Foldout("Behaviour")][SerializeField] private bool allowHeadshots;
    [Foldout("Behaviour")][SerializeField] private ImpulseType impulseType = ImpulseType.FromToDirection;
    [Foldout("Behaviour")][ShowIf("_appliesImpulse")][SerializeField] private float impulseAmount = 1;
    [Foldout("Behaviour")][SerializeField] private bool destroyOnContact = true;
    [Foldout("Behaviour")][ShowIf("destroyOnContact")][SerializeField] private bool stopMovementOnDeath = true;
    [Foldout("Behaviour")][ShowIf("destroyOnContact")][SerializeField] private ScopedValue<float> destroyDelay;
    [Foldout("Behaviour")][SerializeField] private ScopedValue<float> maxLifetime;
    [Foldout("Behaviour")][SerializeField] private bool explodeOnDeath;
    [Foldout("Behaviour")][ShowIf("explodeOnDeath")][SerializeField] private GameObject explosionPrefab;
    [Foldout("Behaviour")][ShowIf("explodeOnDeath")][SerializeField] private ScopedValue<float> explosionDelay;
    [Foldout("Sound")][SerializeField] private Transform soundOrigin;
    [Foldout("Sound")][SerializeField] private AudioClip impactSound;
    [Foldout("Sound")][SerializeField] private AudioClip destroySound;
    [Foldout("VFX")][SerializeField] private Transform vfxOrigin;
    [Foldout("VFX")][SerializeField] private VisualEffectConfig projectileVFX;
    [Foldout("VFX")][SerializeField] private VisualEffectConfig impactVFX;
    [Foldout("VFX")][SerializeField] private VisualEffectConfig destroyVFX;

    public event UnityAction OnContactEvent;
    public event UnityAction OnDestroyEvent;

    private Rigidbody _rigidbody;
    private HashSet<Transform> _ignoredObjects;
    private Dictionary<Transform, EnemyHitData> _hitMap;
    private bool _hasHitObject;
    private VisualEffect projectileVFXObj;

    private bool _appliesImpulse => impulseType != ImpulseType.None;

    private enum ImpulseType { None, InMoveDirection, FromToDirection, Explosion }

    private struct EnemyHitData
    {
        public EnemyHitData(Vector3 hitLocation, bool isHead)
        {
            this.hitLocation = hitLocation;
            this.isHead = isHead;
        }

        public Vector3 hitLocation;
        public bool isHead;

        public void Deconstruct(out Vector3 hitLocation, out bool isHead)
        {
            hitLocation = this.hitLocation;
            isHead = this.isHead;
        }
    }

    private interface ICollisionInfo
    {
        public Rigidbody rigidbody { get; }
        public Transform transform { get; }
        public Vector3 hitLocation { get; }
        public bool CompareTag(string tag) => transform.CompareTag(tag);
    }

    private struct CollisionInfo : ICollisionInfo
    {
        public Collision collision;
        public Rigidbody rigidbody => collision.rigidbody;
        public Transform transform => collision.transform;
        public Vector3 hitLocation => collision.GetAverageContactPosition();

        public CollisionInfo(Collision collision)
        {
            this.collision = collision;
        }
    }

    private struct TriggerInfo : ICollisionInfo
    {
        public Collider collider;
        public Rigidbody rigidbody => collider.attachedRigidbody;
        public Transform transform => collider.transform;
        public Vector3 hitLocation { get; set; }

        public TriggerInfo(Collider collider, Vector3 hitLocation)
        {
            this.collider = collider;
            this.hitLocation = hitLocation;
        }
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _ignoredObjects = new();
        _hitMap = new();

        if (projectileVFX)
            projectileVFXObj = projectileVFX.Spawn(vfxOrigin);

        WithDelay(maxLifetime.Value, InitiateDestruction);
    }

    private void OnCollisionEnter(Collision collision)
        => _handleCollision(new CollisionInfo(collision));

    private void OnTriggerEnter(Collider other)
        => _handleCollision(new TriggerInfo(other, transform.position));

    private void _handleCollision(ICollisionInfo info)
    {
        if (_ignoredObjects.Contains(info.transform))
            return;

        if (!_hasHitObject)
        {
            _hasHitObject = true;
            StartCoroutine(_handleHitObject());
        }

        if (_appliesImpulse && info.rigidbody != null)
            info.rigidbody.AddForceAtPosition(
                impulseAmount * _rigidbody.mass * (
                    impulseType switch
                    {
                        ImpulseType.InMoveDirection => _rigidbody.velocity,
                        ImpulseType.FromToDirection => Vector3.Project(_rigidbody.velocity, info.transform.position - info.hitLocation),
                        ImpulseType.Explosion => (info.transform.position - info.hitLocation).normalized,
                        _ => transform.forward
                    }
                ),
                info.hitLocation,
                ForceMode.Impulse
            );

        bool isHead = info.CompareTag(headTag);

        if (isHead || info.CompareTag(bodyTag))
        {
            Transform baseTransform = info.transform.parent;

            if (!_hitMap.ContainsKey(baseTransform) || (isHead && !_hitMap[baseTransform].isHead))
            {
                if (!_hitMap.Any())
                    StartCoroutine(_handleHitEnemies());

                _hitMap[baseTransform] = new(info.hitLocation, isHead);
            }
        }
    }

    public void IgnoreObject(Transform obj) => _ignoredObjects.Add(obj);

    public void UnignoreObject(Transform obj) => _ignoredObjects.Remove(obj);

    private IEnumerator _handleHitEnemies()
    {
        yield return new WaitForFixedUpdate();

        foreach (var (enemy, (hitLocation, isHead)) in _hitMap)
            if (allowHeadshots && isHead)
                enemy.GetComponentInParent<IShootable>()?.HitHead(hitLocation);
            else
                enemy.GetComponentInParent<IShootable>()?.Hit(hitLocation);

        _hitMap.Clear();
    }

    private IEnumerator _handleHitObject()
    {
        yield return new WaitForFixedUpdate();

        if (impactSound)
            AudioSource.PlayClipAtPoint(impactSound, soundOrigin.position);

        if (impactVFX)
            impactVFX.Spawn(vfxOrigin.position, vfxOrigin.rotation);

        if (destroyOnContact)
            InitiateDestruction();
        else
            _hasHitObject = false;

        onContact.Invoke();
        OnContactEvent?.Invoke();
    }

    public void InitiateDestruction()
    {
        if (projectileVFX && destroyDelay.Value > 0)
        {
            projectileVFXObj.Stop();
            projectileVFX.Destroy(projectileVFXObj);

            if (stopMovementOnDeath)
                Destroy(GetComponent<Rigidbody>());

            if (explodeOnDeath && explosionPrefab)
                WithDelay(explosionDelay.Value, () => Instantiate(explosionPrefab, transform.position, transform.rotation));

            Destroy(GetComponent<Collider>());
            Destroy(gameObject, projectileVFX.DestroyDelay);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (destroySound)
            AudioSource.PlayClipAtPoint(destroySound, soundOrigin.position);

        if (destroyVFX)
            destroyVFX.Spawn(vfxOrigin.position, vfxOrigin.rotation);

        onDestroy.Invoke();
        OnDestroyEvent?.Invoke();
    }
}
