using DG.Tweening;
using UnityEngine;

public class SmoothPositionToggleable : MonoBehaviour
{
    [SerializeField] private ScopedValue<Vector3> relativeTarget;
    [SerializeField] private ScopedValue<float> toTargetDuration;
    [SerializeField] private ScopedValue<float> toOriginDuration;
    [SerializeField] private Ease toTargetEase = Ease.InOutCubic;
    [SerializeField] private Ease toOriginEase = Ease.InOutCubic;

    public Vector3 Origin => origin;
    public Vector3 Target => target;

    private Vector3 origin;
    private Vector3 target;
    private bool isAtOrigin = true;
    private Tween currentTween;

    private void Awake()
    {
        origin = transform.position;
        target = transform.position + relativeTarget.Value;
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
        currentTween = transform.DOMove(target, toTargetDuration.Value).SetEase(toTargetEase);
        isAtOrigin = false;
    }

    public void ToTargetInstant()
    {
        currentTween?.Kill();
        transform.position = target;
        isAtOrigin = false;
    }

    public void ToOrigin()
    {
        currentTween?.Kill();
        currentTween = transform.DOMove(origin, toOriginDuration.Value).SetEase(toOriginEase);
        isAtOrigin = true;
    }

    public void ToOriginInstant()
    {
        currentTween?.Kill();
        transform.position = origin;
        isAtOrigin = true;
    }
}
