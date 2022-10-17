using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AnimateVignetteColor : MonoBehaviour
{
    [SerializeField] private Volume volume;
    [SerializeField] private List<KeyFrame<float>> frames;

    private Sequence _sequence;

    public bool isAnimating => _sequence != null;

    public void Animate()
    {
        _sequence?.Kill();
        _sequence = DOTween.Sequence();

        foreach (var f in frames)
            _sequence.Append(
                DOTween.To(
                    () => volume.weight,
                    (w) => volume.weight = w,
                    f.Value,
                    f.Duration
                ).SetEase(f.Ease)
            );

        _sequence.Play();
    }
}
