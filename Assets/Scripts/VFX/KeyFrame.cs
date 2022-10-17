using DG.Tweening;
using System;
using UnityEngine;

public interface IKeyFrame<T>
{
    public T Value { get; set; }
    public float Duration { get; set; }
    public Ease Ease { get; set; }
}

[Serializable]
public struct KeyFrame<T> : IKeyFrame<T>
{
    [SerializeField] private T value;
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;

    public T Value { get => value; set => this.value = value; }
    public float Duration { get => duration; set => duration = value; }
    public Ease Ease { get => ease; set => ease = value; }
}

[Serializable]
public struct HDRColorKeyFrame : IKeyFrame<Color>
{
    [ColorUsage(showAlpha: true, hdr: true)][SerializeField] private Color value;
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;

    public Color Value { get => value; set => this.value = value; }
    public float Duration { get => duration; set => duration = value; }
    public Ease Ease { get => ease; set => ease = value; }
}