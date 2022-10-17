using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.Events;

public class Heat : ExtendedBehaviour
{
    [Foldout("Behaviour")][SerializeField] private ScopedValue<float> maxHeat;
    [Foldout("Behaviour")][SerializeField] private ScopedValue<float> coolingRate;
    [Foldout("Behaviour")][SerializeField] private ScopedValue<float> coolingDelay;
    [Foldout("Behaviour")][SerializeField] private ScopedValue<float> coolingThreshold;
    [Foldout("Overheat VFX")][SerializeField] private VisualEffectConfig vfx;
    [Foldout("Overheat VFX")][SerializeField] private Transform vfxSpawnLocation;
    [Foldout("Overheat Sound")][SerializeField] private AudioClip sound;
    [Foldout("Overheat Sound")][SerializeField] private Transform soundSpawnLocation;
    [Foldout("Events")][SerializeField] private UnityEvent onOverheat;
    [Foldout("Events")][SerializeField] private UnityEvent onCooled;
    public event UnityAction OnOverheatEvent;
    public event UnityAction OnCooledEvent;

    private float coolTime;
    public bool RequiresCooling { get; private set; }
    public float Value { get; private set; }
    public float Percentage => Value / maxHeat.Value;
    public float ThresholdPercentage => Value.Remap(coolingThreshold.Value, maxHeat.Value, 0, 1, clampResult: true);

    private void Update()
    {
        if (coolTime + coolingDelay.Value <= Time.time)
            Cool(coolingRate.Value * Time.deltaTime);
    }

    public void DisableCooling() => coolTime = float.PositiveInfinity;
    public void EnableCooling() => coolTime = Time.time;



    public void Cool(float amount)
    {
        Value = Mathf.Max(0, Value - amount);

        if (RequiresCooling && Value <= coolingThreshold.Value)
        {
            RequiresCooling = false;

            onCooled.Invoke();
            OnCooledEvent?.Invoke();
        }
    }

    public void AddHeat(float heat)
    {
        var newHeat = Value + heat;

        if (newHeat < maxHeat.Value)
        {
            Value = newHeat;
            return;
        }

        RequiresCooling = true;
        Value = maxHeat.Value;

        if (vfx && vfxSpawnLocation)
            vfx.Spawn(vfxSpawnLocation.position, vfxSpawnLocation.rotation);

        if (sound && soundSpawnLocation)
            AudioSource.PlayClipAtPoint(sound, soundSpawnLocation.position);

        onOverheat.Invoke();
        OnOverheatEvent?.Invoke();
    }
}
