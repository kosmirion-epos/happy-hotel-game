using UnityEngine;

public class LastShotTime : MonoBehaviour
{
    [SerializeField] private ScopedValue<float> value;
    public float Value => value.Value;

    public bool ExceedsDuration(float duration) => Time.unscaledTime - Value >= duration;

    public void UpdateTime() => value.Value = Time.unscaledTime;
}
