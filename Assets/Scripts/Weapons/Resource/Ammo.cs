using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class Ammo : MonoBehaviour
{
    [SerializeField] private ScopedValue<int> maxBullets;
    [ShowNonSerializedField] private int bullets = 0;
    [Foldout("Reload VFX")][SerializeField] private VisualEffectConfig vfx;
    [Foldout("Reload VFX")][SerializeField] private Transform vfxSpawnLocation;
    [Foldout("Reload Sound")][SerializeField] private AudioClip sound;
    [Foldout("Reload Sound")][SerializeField] private Transform soundSpawnLocation;
    [Foldout("Events")][SerializeField] private UnityEvent onRemoveBullet;
    [Foldout("Events")][SerializeField] private UnityEvent onRefill;

    public event UnityAction OnRemoveBulletEvent;
    public event UnityAction OnRefillEvent;

    public int Bullets => bullets;
    public int MaxBullets => maxBullets.Value;

    private void Awake()
    {
        bullets = maxBullets.Value;
    }

    public bool HasBullet() => bullets > 0;
    public void RemoveBullet()
    {
        --bullets;

        onRemoveBullet.Invoke();
        OnRemoveBulletEvent?.Invoke();
    }

    public bool CanRegain(int amount) => bullets + amount <= maxBullets.Value;
    public bool CanRegain() => CanRegain(1);

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    public void Refill()
    {
        var temp = bullets;
        bullets = maxBullets.Value;

        if (temp >= maxBullets.Value)
            return;

        if (vfx && vfxSpawnLocation)
            vfx.Spawn(vfxSpawnLocation.position, vfxSpawnLocation.rotation);

        if (sound && soundSpawnLocation)
            AudioSource.PlayClipAtPoint(sound, soundSpawnLocation.position);

        onRefill?.Invoke();
        OnRefillEvent?.Invoke();
    }

    public void Regain(int amount) => bullets = Mathf.Min(bullets + amount, maxBullets.Value);
    public void Regain() => Regain(1);
}
