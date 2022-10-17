using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimateColor : MonoBehaviour
{
    [SerializeField] private new Renderer renderer;
    [SerializeField] private int materialID;
    [SerializeField] private List<HDRColorKeyFrame> frames;
    [SerializeField] private string colorName;

    public IReadOnlyList<HDRColorKeyFrame> Frames => frames;
    public bool isAnimating => _sequence != null;
    public Material Material => renderer.materials[materialID];

    private Sequence _sequence;

    public void Animate()
    {
        _sequence?.Kill();
        _sequence = DOTween.Sequence();

        foreach (var f in frames)
            _sequence.Append(
                DOTween.To(
                    () => Material.GetColor(colorName),
                    (c) => Material.SetColor(colorName, c),
                    f.Value,
                    f.Duration
                ).SetEase(f.Ease)
            );

        _sequence.Play();
    }
}
