using UnityEngine;

public static class FloatExtensions
{
    public static float Remap(this float f, float oldLo, float oldHi, float newLo, float newHi, bool clampResult = false)
    {
        float result = (f - oldLo) / (oldHi - oldLo) * (newHi - newLo) + newLo;
        return clampResult ? Mathf.Clamp(result, Mathf.Min(newLo, newHi), Mathf.Max(newLo, newHi)) : result;
    }
}
