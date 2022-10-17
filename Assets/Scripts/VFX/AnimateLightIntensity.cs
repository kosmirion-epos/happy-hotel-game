using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimateLightIntensity : ExtendedBehaviour
{
    [SerializeField] private List<KeyFrame<float>> frames;
    [SerializeField] private bool multiUse = true;

    private new Light light;
    private Sequence sequence;

    private void Awake()
    {
        light = GetComponent<Light>();
        sequence = DOTween.Sequence();
    }

    private void OnEnable()
    {
        foreach (var f in frames)
            sequence.Append(
                DOTween.To(
                    () => light.intensity,
                    (i) => light.intensity = i,
                    f.Value,
                    f.Duration
                ).SetEase(f.Ease)
            );

        sequence.Pause();

        if (multiUse)
            sequence.onComplete = () => sequence.Rewind();
    }

    public void Animate()
    {
        sequence.Restart();
    }
}
