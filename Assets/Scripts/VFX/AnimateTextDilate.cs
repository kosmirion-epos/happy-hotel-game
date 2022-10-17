using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateTextDilate : MonoBehaviour
{
    [SerializeField] private new CanvasRenderer renderer;
    [SerializeField] private int materialID;
    [SerializeField] private List<KeyFrame<float>> frames;

    public IReadOnlyList<KeyFrame<float>> Frames => frames;
    public bool isAnimating => _sequence != null;
    public Material Material => renderer.GetMaterial(materialID);

    private Sequence _sequence;

    public void Animate()
    {
        _sequence?.Kill();
        _sequence = DOTween.Sequence();

        foreach (var f in frames)
            _sequence.Append(
                DOTween.To(
                    () => Material.GetFloat("_FaceDilate"),
                    d => Material.SetFloat("_FaceDilate", d),
                    f.Value,
                    f.Duration
                ).SetEase(f.Ease)
            );

        _sequence.Play();
    }
}
