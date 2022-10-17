using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateScale : MonoBehaviour
{
    [SerializeField] private new Transform transform;
    [SerializeField] private List<KeyFrame<Vector3>> frames;

    public IReadOnlyList<KeyFrame<Vector3>> Frames => frames;
    public bool isAnimating => _sequence != null;

    private Sequence _sequence;

    public void Animate()
    {
        _sequence?.Kill();
        _sequence = DOTween.Sequence();

        foreach (var f in frames)
            _sequence.Append(
                DOTween.To(
                    () => transform.localScale,
                    s => transform.localScale = s,
                    f.Value,
                    f.Duration
                ).SetEase(f.Ease)
            );

        _sequence.Play();
    }
}
