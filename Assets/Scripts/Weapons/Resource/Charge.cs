using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class Charge : ExtendedBehaviour
{
    [SerializeField] private ScopedValue<float> chargeDuration;
    [Foldout("VFX")][SerializeField] private VisualEffectConfig chargeVFX;
    [Foldout("VFX")][SerializeField] private VisualEffectConfig chargeFullVFX;
    [Foldout("VFX")][SerializeField] private Transform vfxOrigin;
    [Foldout("Events")][SerializeField] private UnityEvent onCharge;
    [Foldout("Events")][SerializeField] private UnityEvent onDischarge;
    [Foldout("Events")][SerializeField] private UnityEvent onChargeFull;
    public event UnityAction OnChargeEvent;
    public event UnityAction OnDischargeEvent;
    public event UnityAction OnChargeFullEvent;

    private bool charging;
    private float chargeTime;

    private VisualEffect chargeVFXObj;

    public bool Charging => charging;
    public float ChargeDuration
    {
        get => chargeDuration.Value;
        set => chargeDuration.Value = value;
    }
    public bool FullyCharged
        => charging && Time.time - chargeTime >= chargeDuration.Value - 0.0001f;

    public void StartCharge()
    {
        charging = true;
        chargeTime = Time.time;

        WithDelay(chargeDuration.Value, _onChargeFull);

        if (chargeVFX)
        {
            if (chargeVFXObj)
                chargeVFX.Destroy(chargeVFXObj);

            chargeVFXObj = chargeVFX.Spawn(vfxOrigin);
        }

        onCharge.Invoke();
        OnChargeEvent?.Invoke();
    }

    private void _onChargeFull()
    {
        if (!FullyCharged)
            return;

        if (chargeFullVFX)
            chargeFullVFX.Spawn(vfxOrigin);

        onChargeFull.Invoke();
        OnChargeFullEvent?.Invoke();
    }

    public void Discharge()
    {
        if (!charging)
            return;

        charging = false;

        if (chargeVFXObj)
            chargeVFX.Destroy(chargeVFXObj);

        onDischarge.Invoke();
        OnDischargeEvent?.Invoke();
    }
}
