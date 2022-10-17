using DG.Tweening;
using UnityEngine;

public class SmoothRotationToggleable : MonoBehaviour
{
    [SerializeField] private ScopedValue<Quaternion> relativeTarget;
    [SerializeField] private ScopedValue<float> toTargetDuration;
    [SerializeField] private ScopedValue<float> toOriginDuration;
    [SerializeField] private Ease toTargetEase = Ease.InOutCubic;
    [SerializeField] private Ease toOriginEase = Ease.InOutCubic;

    private Quaternion origin;
    private Quaternion target;
    private bool isAtOrigin = true;
    private Tween currentTween;

    private void Awake()
    {
        origin = transform.rotation;
        target = relativeTarget.Value * transform.rotation;
    }              

    public void Toggle()
    {
        if (isAtOrigin)
            ToTarget();
        else
            ToOrigin();
    }

    public void ToTarget()
    {
        currentTween?.Kill();
        currentTween = transform.DORotateQuaternion(target, toTargetDuration.Value).SetEase(toTargetEase);
        isAtOrigin = false;
    }

    public void ToTargetInstant()
    {
        currentTween?.Kill();
        transform.rotation = target;
        isAtOrigin = false;
    }

    public void ToOrigin()
    {
        currentTween?.Kill();
        currentTween = transform.DORotateQuaternion(origin, toOriginDuration.Value).SetEase(toOriginEase);
        isAtOrigin = true;
    }

    public void ToOriginInstant()
    {
        currentTween?.Kill();
        transform.rotation = origin;
        isAtOrigin = true;
    }
}
